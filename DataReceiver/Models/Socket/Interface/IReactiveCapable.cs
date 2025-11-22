using DataReceiver.Models.Socket.Common;

namespace DataReceiver.Models.Socket.Interface
{
    public interface IReactiveCapable
    {
        public IObservable<DataEventArgs<byte>> DataObservable { get; }
        public IObservable<StateEventArgs> StateObservable { get; }

        /// <summary>
        /// 更新连接状态的唯一方法，触发状态变更的事件流
        /// </summary>
        /// <param name="newState">当前状态</param>
        /// <param name="oldState">原先状态</param>
        /// <param name="message">附带信息</param>
        /// <returns>返回当前状态</returns>
        ConnectionState OnStateUpdated(ConnectionState newState, ConnectionState oldState, string message = "");

        /// <summary>
        /// 收到数据的信息流，用于通知、传递新的消息至该信息流的订阅者。
        /// </summary>
        /// <param name="data">接收到的数据</param>
        /// <param name="message">接收到数据时附带的消息</param>
        /// <returns>接收到数据的长度</returns>
        int OnDataReceived(ReadOnlyMemory<byte> data, string message = "");
    }
}
