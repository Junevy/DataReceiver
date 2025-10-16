using DataReceiver.Models.Common;
using DataReceiver.Models.Config;
using DataReceiver.Models.Socket.Base;
using System.Buffers;
using System.IO;
using System.Net.Sockets;

namespace DataReceiver.Models.Socket
{
    public class TcpClientModel : ConnectionBase<TcpClient, TcpConfig>
    {
        private Stream Stream { get; set; }

        public TcpClientModel(TcpConfig config) : base(config)
        {
            Socket = new TcpClient();
        }

        public override async Task<ConnectionState> ConnectAsync(CancellationToken ct = default)
        {
            if (ConnectionState.Reconnecting == Runtimes.State) return Runtimes.State;
            if (ConnectionState.Connecting == Runtimes.State) return Runtimes.State;
            if (ConnectionState.Connected == Runtimes.State) await DisconnectAsync();

            //初始化
            OnStateUpdated(ConnectionState.Connecting, "Connecting...");
            Socket ??= new();
            ConfigSocket();

            var timeoutTask = Task.Delay(Config.ConnectTimeout);
            try
            {
                //连接
                using var anyTask = await Task.WhenAny(timeoutTask, Socket.ConnectAsync(Config.Ip, Config.Port));
                if (anyTask == timeoutTask || !Socket.Connected)
                    return OnStateUpdated(ConnectionState.Disconnected, "Connect timeout or error.");
                //连接成功
                Config.LastActivityTime = DateTime.Now;
                Stream = Socket.GetStream();

                //配置Socket
                if (Config.EnableKeepAlive)
                    KeepSocketAlive();
                if (cts.IsCancellationRequested)
                    cts = new();

                //启动消息接收
                receiveTask = ReceiveAsync(cts.Token).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        // Log...
                    }
                });
                return OnStateUpdated(ConnectionState.Connected, "Connection Successful!");
            }
            catch (Exception ex)
            {
                return OnStateUpdated(ConnectionState.Disconnected, $"Connect error: {ex.Message}");
            }
        }

        public override async Task DisconnectAsync()
        {
            await CleanConectionAsync();
            OnStateUpdated(ConnectionState.Disconnected, "Disconnected.");
            //Dispose();
        }

        protected override async Task<int> ReceiveAsync(CancellationToken ct = default)
        {
            var stream = Stream ?? throw new Exception("Stream is null");
            var buffer = ArrayPool<byte>.Shared.Rent(Config.BufferSize);
            int bytesRead;
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, ct);
                    }
                    catch (OperationCanceledException) { break; }
                    catch (Exception e)
                    {
                        if (Socket is not null)
                            OnStateUpdated(Runtimes.State, $"Receive data error: {e.Message} ");
                        break;
                    }
                    //连接已关闭（被动或主动）
                    if (bytesRead == 0)
                    {
                        OnStateUpdated(ConnectionState.Disconnected, $"Connection was disconnected: [{Config.Ip} : {Config.Port}]");
                        break;
                    }
                    // Copy数据
                    var data = new byte[bytesRead];
                    Array.Copy(buffer, 0, data, 0, bytesRead);

                    Config.LastActivityTime = DateTime.UtcNow;
                    OnDataReceived(data, "Data received.");
                }
                return -1;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
                //if (Socket != null && !Socket.Connected)
                await DisconnectAsync();
            }
        }

        public override Task<int> SendAsync(byte[] data, CancellationToken ct = default)
        {
            throw new Exception();
        }

        private void KeepSocketAlive()
        {
            //开启自动探测
            Socket?.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            var keepAlive = new byte[12];
            BitConverter.GetBytes((uint)1).CopyTo(keepAlive, 0); //开启keep alive
            BitConverter.GetBytes((uint)Config.KeepAliveInterval).CopyTo(keepAlive, 4); //空闲多久开始探测
            BitConverter.GetBytes((uint)Config.KeepAliveInterval).CopyTo(keepAlive, 8);
            Socket?.Client.IOControl(IOControlCode.KeepAliveValues, keepAlive, null);
        }

        private void ConfigSocket()
        {
            if (Config is null) throw new ArgumentNullException("Config is null!");
            try
            {
                Socket.NoDelay = Config.EnableNagle;
                Socket.ReceiveBufferSize = Config.BufferSize;
                Socket.SendBufferSize = Config.BufferSize;
                //Socket.SendTimeout = Config.ConnectTimeout
                //Socket.ReceiveTimeout = Config.ConnectTimeout
            }
            catch (Exception e)
            {
                OnDataReceived(new byte[4], $"Config Socket Error: {e.Message}");
            }
        }

        private async Task CleanConectionAsync()
        {
            try
            {
                if (cts is not null && !cts.IsCancellationRequested)
                {
                    cts.Cancel();
                }
            }
            catch { }

            // 等待正在进行的接收任务完成
            //if (receiveTask != null)
            //{
            //    try { await receiveTask.ConfigureAwait(false); } catch { } finally { receiveTask = null; }
            //}

            try { cts?.Dispose(); } catch { }
            try { Stream?.Close(); } catch { }
            try { Stream?.Dispose(); } catch { }
            try { Socket?.Close(); } catch { }
            try { Socket?.Dispose(); } catch { }
            Socket = null;
        }

    }
}

#region old code2

//public override async Task<Result> ConnectAsync(CancellationToken ct)
//{
//    stateChanged.OnNext(ConnectionState.Connecting);
//    try
//    {
//        client = new();
//        using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
//        cts.CancelAfter(config.ConnectTimeout);
//        await client.ConnectAsync(config.Ip, config.Port);

//        if (config.EnableKeepAlive)
//            KeepSocketAlive();
//        stateChanged.OnNext(ConnectionState.Open);

//        _ = ReceiveLoopAsync();
//        return Result.Success;
//    }
//    catch (Exception ex)
//    {
//        stateChanged.OnNext(ConnectionState.Closed);
//        return Result.Failure;
//    }
//}


//public async Task ReceiveLoopAsync()
//{
//    var buffer = new byte[1024];
//    try
//    {
//        MessageReceived.Invoke(this, "Start Receiving");
//        while (!cts.IsCancellationRequested)
//        {
//            var bytes = await stream.ReadAsync(buffer, 0, buffer.Length, cts.Token);
//            var message = Encoding.UTF8.GetString(buffer, 0, bytes);

//            if (bytes == 0) { ReStart(); return; }
//            if (message.Equals(HeartBeat)) _ = SendAsync(HeartBeat);
//            else OnReceivedMessage?.Invoke(message);
//            LastActivedTime = DateTime.Now;
//        }
//    }
//    catch (TaskCanceledException)
//    {
//    }
//    catch (ObjectDisposedException)
//    {
//        Stop();
//    }
//}
#endregion


#region old code
//public string Ip { get; set; } = "192.168.31.163";
//public int Port { get; set; } = 9007;
//public string HeartBeat { get; set; } = "PING";
//public bool IsHeartBeat { get; set; } = false;
//public TcpClient? Client { get; set; }


//private CancellationTokenSource cts;

//public bool IsConnected => Client != null
//                   && !(Client.Client.Poll(10000, SelectMode.SelectRead)
//                        && Client.Client.Available == 0) && Client.Connected;

//public override async Task StartAsync()
//{


//    Stop();
//    cts = new();
//    Client ??= new();

//    var timeoutTask = Task.Delay(Timeout, cts.Token);
//    var anyTask = await Task.WhenAny(timeoutTask, Client?.ConnectAsync(Ip, Port));

//    //连接超时或失败
//    if (anyTask == timeoutTask || !IsConnected)
//    {
//        OnReceivedMessage?.Invoke($"Connect error : [{Ip}:{Port}]. Start auto connect after 10 seconds.");
//        _ = StartAutoConnectAsync();
//        return;
//    }

//    KeepSocketAlive();
//    OnStateChanged?.Invoke(IsConnected);

//    if (IsHeartBeat) _ = StartHeartBeatAsync();
//    if (IsAutoConnect) _ = StartAutoConnectAsync();

//    LastActivedTime = DateTime.Now;
//    _ = ReceiveAsync(Client!.GetStream());
//}

//public override void Stop()
//{
//    if (cts != null && !cts.IsCancellationRequested)
//    {
//        cts?.Cancel();
//        cts?.Dispose();
//    }
//    Client?.Close();
//    Client?.Dispose();
//    Client = null;
//    OnStateChanged?.Invoke(IsConnected);
//}

//public override async Task ReceiveAsync(Stream stream)
//{
//    var buffer = new byte[1024];
//    try
//    {
//        MessageReceived.Invoke(this, "Start Receiving");
//        while (!cts.IsCancellationRequested)
//        {
//            var bytes = await stream.ReadAsync(buffer, 0, buffer.Length, cts.Token);
//            var message = Encoding.UTF8.GetString(buffer, 0, bytes);

//            if (bytes == 0) { ReStart(); return; }
//            if (message.Equals(HeartBeat)) _ = SendAsync(HeartBeat);
//            else OnReceivedMessage?.Invoke(message);
//            LastActivedTime = DateTime.Now;
//        }
//    }
//    catch (TaskCanceledException)
//    {
//    }
//    catch (ObjectDisposedException)
//    {
//        Stop();
//    }
//}

//public override async Task SendAsync(string message)
//{
//    try
//    {
//        var msg = Encoding.UTF8.GetBytes(message);
//        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token);
//        timeoutCts.CancelAfter(Timeout);
//        await Client!.GetStream().WriteAsync(msg, 0, msg.Length, timeoutCts.Token);
//    }
//    catch (TaskCanceledException)
//    {
//        if (IsConnected) Stop();
//    }
//}

//public override void ReStart()
//{
//    Stop();
//    _ = StartAsync();
//}

//public async Task StartHeartBeatAsync()
//{
//    while (cts != null && !cts.IsCancellationRequested)
//    {
//        await Task.Delay(Interval);
//        if (!IsHeartBeat) return;
//        //if (!IsConnected) continue;1` 
//        _ = SendAsync(HeartBeat);
//    }
//}

//public async Task StartAutoConnectAsync()
//{
//    if (isReconnect) return;

//    isReconnect = true;
//    while (IsAutoConnect && !cts.IsCancellationRequested)
//    {
//        await Task.Delay(Interval);
//        if (!IsConnected && !cts.IsCancellationRequested)
//            ReStart();
//    }
//    isReconnect = false;
//}

///// <summary>
///// 能否使用装饰器模式或抽象成接口？
///// </summary>
//private void KeepSocketAlive()
//{
//    //开启自动探测
//    Client?.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
//    var keepAlive = new byte[12];
//    BitConverter.GetBytes((uint)1).CopyTo(keepAlive, 0); //开启keep alive
//    BitConverter.GetBytes((uint)60000).CopyTo(keepAlive, 4); //空闲多久开始探测
//    BitConverter.GetBytes((uint)60000).CopyTo(keepAlive, 8);
//    Client?.Client.IOControl(IOControlCode.KeepAliveValues, keepAlive, null);
//}

#endregion
