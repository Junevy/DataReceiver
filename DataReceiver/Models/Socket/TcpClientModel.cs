using DataReceiver.Models.Config;
using DataReceiver.Models.Socket.Base;
using DataReceiver.Models.Socket.Common;
using System.Buffers;
using System.IO;
using System.Net.Sockets;

namespace DataReceiver.Models.Socket
{
    public class TcpClientModel(TcpConfig config) : ConnectionBase<TcpClient, TcpConfig>(config)
    {
        private Stream? Stream { get; set; }

        public override async Task<ConnectionState> ConnectAsync(CancellationToken ct = default)
        {
            if (Runtimes.State is ConnectionState.Connecting)
                return Runtimes.State;
            if (Runtimes.State is ConnectionState.Connected)
                await DisconnectAsync();

            //初始化
            CleanConnectionAsync();
            OnStateUpdated(ConnectionState.Connecting, "Connecting...");
            Socket = new();
            if (Cts == null || Cts.IsCancellationRequested)
                Cts = new();
            ConfigSocket();

            var timeoutTask = Task.Delay(Config.ConnectTimeout, Cts.Token);
            try
            {
                //连接
                using var anyTask = await Task.WhenAny(timeoutTask, Socket.ConnectAsync(Config.Ip, Config.Port))
                    .ConfigureAwait(false);
                if (anyTask == timeoutTask || !Socket.Connected)
                {
                    Socket.Close();
                    return OnStateUpdated(ConnectionState.Disconnected, "Connect timeout or error.");
                }
                //连接成功
                Runtimes.LastActivityTime = DateTime.Now.ToString("yyyy-MM-ss HH:mm:ss");
                Stream = Socket.GetStream();

                //配置Socket
                if (Config.EnableKeepAlive)
                    KeepSocketAlive();

                //启动消息接收
                receiveTask = Task.Run(() => { _ = ReceiveAsync(Cts.Token); });
                return OnStateUpdated(ConnectionState.Connected, "Connection Successful!");
            }
            catch (OperationCanceledException)
            {
                // Log..
                return OnStateUpdated(ConnectionState.Disconnected, "ConnectTask Cancelled");
            }
            catch (Exception ex)
            {
                return OnStateUpdated(ConnectionState.Disconnected, $"Connect error: {ex.Message}");
            }
        }

        public override async Task DisconnectAsync()
        {
            CleanConnectionAsync();
            OnStateUpdated(ConnectionState.Disconnected, "Disconnected.");
        }

        public override async Task<int> SendAsync(byte[] data, CancellationToken ct = default)
        {
            if (Runtimes.State != ConnectionState.Connected) return -1;
            if (data == null || data.Length == 0) return -1;

            using var lts = CancellationTokenSource.CreateLinkedTokenSource(Cts.Token);
            lts.CancelAfter(Config.SendTimeOut);
            await sendLock.WaitAsync(Cts.Token);
            try
            {
                await Stream!.WriteAsync(data, 0, data.Length, lts.Token);
                Runtimes.LastActivityTime = DateTime.Now.ToString("yyyy-MM-ss HH:mm:ss");
                return data.Length;
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
                DisconnectAsync().GetAwaiter().GetResult();
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

                    Runtimes.LastActivityTime = DateTime.Now.ToString("yyyy-MM-ss HH:mm:ss");
                    OnDataReceived(buffer, "Data received.");
                }
                return -1;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
                // 判断是否是手动取消，避免重复操作。
                //if (Cts != null || !Cts.IsCancellationRequested)
                //同步执行，finally中使用异步操作不合理，甚至可能卡死ui。
                DisconnectAsync().GetAwaiter().GetResult();
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

            try { } catch { return; }
            try
            {
                if (Config is null) throw new ArgumentNullException("Config is null!");
                Socket.NoDelay = !Config.EnableNagle;
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

        private void CleanConnectionAsync()
        {
            try
            {
                if (Cts is not null && !Cts.IsCancellationRequested)
                {
                    Cts.Cancel();
                    Cts.Dispose();
                }
            }
            catch { }

            //var t = Interlocked.Exchange(ref receiveTask, null);
            //if (t != null)
            //{
            //    try { await t.ConfigureAwait(false); } catch { /* swallow cancel */ }
            //}

            // 等待正在进行的接收任务完成
            //if (receiveTask != null)
            //{
            //    try { await receiveTask.ConfigureAwait(false); } catch { } finally { receiveTask = null; }
            //}

            try { Stream?.Close(); } catch { }
            try { Stream?.Dispose(); } catch { }
            try { Socket?.Close(); } catch { }
            try { Socket?.Dispose(); } catch { }
        }

        public override void Dispose()
        {
            DisconnectAsync().GetAwaiter().GetResult();
            base.Dispose();
        }
    }
}
