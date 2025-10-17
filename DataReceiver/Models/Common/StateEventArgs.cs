using DataReceiver.Models.Common;

namespace DataReceiver.Models.CommunicationCommon
{
    /// <summary>
    /// 连接状态变更事件参数
    /// </summary>
    /// <param name="newState">新状态</param>
    /// <param name="oldState">旧状态</param>
    /// <param name="message">携带信息</param>
    public class StateEventArgs
        (ConnectionState newState, ConnectionState oldState, string message) 
        : EventArgs
    {
        public ConnectionState NewState { get; } = newState;
        public ConnectionState OldState { get; } = oldState;
        public string Message { get; } = message;
        public DateTime TimeStamp { get; } = DateTime.MinValue;
    }
}
