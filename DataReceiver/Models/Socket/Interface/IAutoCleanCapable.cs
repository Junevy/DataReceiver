using DataReceiver.Models.Socket.Config;

namespace DataReceiver.Models.Socket.Interface
{
    internal interface IAutoCleanCapable
    {
        public FtpServerConfig Config { get; }

        public CancellationTokenSource TokenSource { get; }

        Task StartAutoClean();

        void StopAutoClean();
    }
}
