using DataReceiver.Models.Socket.Common;

namespace DataReceiver.Models.Socket.Interface
{
    public interface IReactiveConnection : IConnection
    {
        /// <summary>
        /// 订阅Subject的信息流，唯一控制State的方法
        /// </summary>
        /// <param name="state">新的Socket连接状态</param>
        /// <param name="message">Socket连接状态发生变化时的附带消息</param>
        /// <returns>当前Socket连接状态</returns>
        ConnectionState OnStateUpdated(ConnectionState state, string message = "");

        /// <summary>
        /// 订阅Socket接收到数据时的信息流
        /// </summary>
        /// <param name="data">接收到的数据</param>
        /// <param name="message">接收到数据时附带的消息</param>
        /// <returns>接收到数据的长度</returns>
        int OnDataReceived(ReadOnlyMemory<byte> data, string message = "");
    }
}
