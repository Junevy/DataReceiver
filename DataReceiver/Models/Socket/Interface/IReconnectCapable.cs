using DataReceiver.Models.Socket.Config;

namespace DataReceiver.Models.Socket.Interface
{
    /// <summary>
    /// 重连功能接口
    /// </summary>
    public interface IReconnectCapable : IReactiveConnection
    {
        ReconnectConfig ReconnectConfig { get; }
        CancellationTokenSource ReconnectTokenSource { get; }

        Task StartReconnectAsync();
        void StopReconnect();
    }
}
