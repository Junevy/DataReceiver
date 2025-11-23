using DataReceiver.Models.Common;
using DataReceiver.Models.Config;
using DataReceiver.Models.Socket.Common;
using DataReceiver.Models.Socket.FTP;
using DataReceiver.Models.Socket.Interface;
using FubarDev.FtpServer;
using log4net;
using System.Collections;

namespace DataReceiver.Models.Socket.Base
{
    public abstract class ConnectionBase<TSocket, KConfig>(KConfig config) : ReactiveCapableBase, IConnection
        where TSocket : class where KConfig : CommunicationConfig
    {
        protected Task? receiveTask;
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConnectionBase<TSocket, KConfig>));

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
            if (Socket is IDisposable socket) 
                socket?.Dispose();

            //FubarDev.FtpServer.FtpServer

            //else if (Socket is IFtpServer ftp)
            //{
            //    try
            //    {
            //        ftp?.PauseAsync((Cts == null || Cts.IsCancellationRequested) ? CancellationToken.None : Cts.Token);
            //    }
            //    catch (Exception e) { Log.Warn($"Pause Ftp Server error : {e.Message}"); }

            //    try
            //    {
            //        ftp?.StopAsync((Cts == null || Cts.IsCancellationRequested) ? CancellationToken.None : Cts.Token).GetAwaiter().GetResult();
            //    }
            //    catch (Exception e) { Log.Warn($"Stop Ftp Server error : {e.Message}"); }
            //}

            try
            {
                if (Cts != null && !Cts.IsCancellationRequested)
                {
                    Cts.Cancel();
                    Cts.Dispose();
                }
            }
            catch { Log.Warn("Cancel token error!"); }

            try { GC.SuppressFinalize(this); } catch { }
            base.Dispose();
        }

        public abstract Task<ConnectionState> ConnectAsync(CancellationToken ct = default);

        public abstract Task<int> SendAsync(byte[] data, CancellationToken ct = default);

        public abstract Task DisconnectAsync();
    }
}