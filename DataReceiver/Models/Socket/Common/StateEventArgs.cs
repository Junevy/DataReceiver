namespace DataReceiver.Models.Socket.Common
{
    /// <summary>
    /// 连接状态变更事件参数
    /// </summary>
    /// <param name="newState">当前状态</param>
    /// <param name="oldState">此前状态</param>
    /// <param name="message">信息</param>
    public class StateEventArgs
        (ConnectionState newState, ConnectionState oldState, string message) : EventArgs
    {
        public ConnectionState NewState { get; } = newState;
        public ConnectionState OldState { get; } = oldState;
        public string Message { get; } = message;
        public DateTime TimeStamp { get; } = DateTime.MinValue;
    }
}
