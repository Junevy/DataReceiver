using DataReceiver.Models.Socket.Config;

namespace DataReceiver.Models.Socket.Interface
{
    /// <summary>
    /// 重连功能接口
    /// </summary>
    public interface IReconnectCapable : IConnection
    {
        public ReconnectConfig ReconnectConfig { get; }
        public CancellationTokenSource ReconncetToken { get; }

        Task StartReconnectAsync();
        void StopReconnect();
    }
}
