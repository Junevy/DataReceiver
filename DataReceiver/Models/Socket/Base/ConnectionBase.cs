using DataReceiver.Models.Common;
using DataReceiver.Models.Config;
using DataReceiver.Models.Socket.Common;
using DataReceiver.Models.Socket.Interface;
using FubarDev.FtpServer;

namespace DataReceiver.Models.Socket.Base
{
    public abstract class ConnectionBase<TSocket, KConfig>(KConfig config) : ReactiveCapableBase, IConnection 
        where TSocket : class where KConfig : CommunicationConfig
    {
        protected Task? receiveTask;
        protected readonly SemaphoreSlim sendLock = new(1, 1);

        public ConnectionRuntimes Runtimes { get; private set; } = new();
        public CancellationTokenSource Cts { get; protected set; } = new();

        /// <summary>
        /// Socket对象
        /// </summary>
        protected TSocket Socket { get; set; } = default!;

        /// <summary>
        /// Socket配置
        /// </summary>
        public KConfig Config { get; } = config 
            ?? throw new ArgumentNullException($"Socket配置文件为空: {nameof(Config)}");

        /// <summary>
        /// 释放所有资源
        /// </summary>
        public override void Dispose()
        {
            try
            {
                if (Cts != null && !Cts.IsCancellationRequested)
                {
                    Cts.Cancel();
                    Cts.Dispose();
                }
            } catch { }

            try 
            { 
                if (Socket is IDisposable socket) socket?.Dispose();
                else if (Socket is IFtpServer ftp) ftp?.StopAsync(Cts?.Token ?? CancellationToken.None).GetAwaiter().GetResult();
            } catch { }

            try { GC.SuppressFinalize(this); } catch { }
            base.Dispose();
        }

        public abstract Task<ConnectionState> ConnectAsync(CancellationToken ct = default);

        public abstract Task<int> SendAsync(byte[] data, CancellationToken ct = default);

        public abstract Task DisconnectAsync();
    }
}