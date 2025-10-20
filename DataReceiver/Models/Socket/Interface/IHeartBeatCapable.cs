using DataReceiver.Models.Config;

namespace DataReceiver.Models.Socket.Interface
{
    /// <summary>
    /// 心跳功能接口
    /// </summary>
    public interface IHeartBeatCapable : IConnection
    {
        HeartBeatConfig HeartBeatConfig { get; }

        /// <summary>
        /// 用于控制心跳任务的启动与停止
        /// </summary>
        CancellationTokenSource HeartBeatToken { get; }

        Task StartHeartBeatAsync(byte[] response);
        void StopHeartBeat();


    }
}
