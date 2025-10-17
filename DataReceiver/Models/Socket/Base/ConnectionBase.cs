using DataReceiver.Models.Config;

namespace DataReceiver.Models.Socket.Base
{
    public abstract class ConnectionBase<TSocket, KConfig>(KConfig config)
        : ConnectionReactiveBase where TSocket : IDisposable where KConfig : CommunicationConfig
    {
        protected Task? receiveTask;
        protected readonly SemaphoreSlim sendLock = new(1, 1);

        /// <summary>
        /// Socket对象
        /// </summary>
        protected TSocket Socket { get; set; } = default!;

        /// <summary>
        /// Socket配置
        /// </summary>
        public KConfig Config { get; }
            = config ?? throw new ArgumentNullException($"Socket配置文件为空: {nameof(Config)}");

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
            }
            catch { }

            try { Socket?.Dispose(); } catch { }

            try { GC.SuppressFinalize(this); } catch { }

            base.Dispose();
        }
    }
}