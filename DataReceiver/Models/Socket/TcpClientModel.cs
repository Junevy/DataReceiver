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
        private Stream? Stream { get; set; }

        public TcpClientModel(TcpConfig config) : base(config)
        {
            Socket = new TcpClient();
        }

        public override async Task<ConnectionState> ConnectAsync(CancellationToken ct = default)
        {
            //if (ConnectionState.Reconnecting == Runtimes.State) return Runtimes.State;
            //if (ConnectionState.Connecting == Runtimes.State) return Runtimes.State;
            if (ConnectionState.Connected == Runtimes.State) await DisconnectAsync();

            //初始化
            await CleanConectionAsync();
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
                if (Cts == null || Cts.IsCancellationRequested)
                    Cts = new();

                //启动消息接收
                receiveTask = ReceiveAsync(Cts.Token).ContinueWith(t =>
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
            Dispose();
        }

        public override async Task<int> SendAsync(byte[] data, CancellationToken ct = default)
        {
            if (Runtimes.State != ConnectionState.Connected) return -1;
            if (data == null || data.Length == 0) return -1;

            await sendLock.WaitAsync(Cts.Token);
            try
            {
                using var ctsSend = CancellationTokenSource.CreateLinkedTokenSource(Cts.Token);
                ctsSend.CancelAfter(Config.SendTimeOut);
                await Stream!.WriteAsync(data, 0, data.Length, ctsSend.Token);
                await Stream.FlushAsync(Cts.Token);
                Config.LastActivityTime = DateTime.Now;
                return 1;
            }
            catch (OperationCanceledException)
            {
                // 发送超时 Log
                return -1;
            }
            catch (Exception e)
            {
                // 发送时发生异常通常意味着断开了连接。

                // Log...
                _ = DisconnectAsync();
                return -1;
            }
            finally
            {
                sendLock.Release();
            }
        }

        protected async Task<int> ReceiveAsync(CancellationToken ct = default)
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

                // 判断是否是手动取消，避免重复操作。
                if (Cts != null && !Cts.IsCancellationRequested)
                    await DisconnectAsync();
            }
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

            try {  } catch { return; }
            try
            {
                if (Config is null) throw new ArgumentNullException("Config is null!");
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
                // 自动重连中则不取消任务
                if (Cts is not null && !Cts.IsCancellationRequested)
                {
                    Cts.Cancel();
                    Cts.Dispose();
                    Cts = null;
                }
            }
            catch { }

            // 等待正在进行的接收任务完成
            //if (receiveTask != null)
            //{
            //    try { await receiveTask.ConfigureAwait(false); } catch { } finally { receiveTask = null; }
            //}

            try { Stream?.Close(); } catch { }
            try { Stream?.Dispose(); } catch { }
            try { Socket?.Close(); } catch { }
            try { Socket?.Dispose(); } catch { }
            Socket = null;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
