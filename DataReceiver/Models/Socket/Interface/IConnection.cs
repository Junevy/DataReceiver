using DataReceiver.Models.Common;
using System.IO;

namespace DataReceiver.Models.Socket.Interface
{
    public interface IConnection : IDisposable
    {
        CancellationTokenSource Cts { get; }
        ConnectionRuntimes Runtimes { get; }

        /// <summary>
        /// 连接Socket
        /// </summary>
        /// <param name="ct">控制令牌</param>
        /// <returns>返回连接结果</returns>
        Task<ConnectionState> ConnectAsync(CancellationToken ct = default);

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="ct">控制令牌</param>
        /// <returns> 返回实际发送的字节数，发送超时、失败、发生异常 则返回-1。</returns>
        Task<int> SendAsync(byte[] data, CancellationToken ct = default);

        //Task<int> ReceiveAsync(Stream stream, CancellationToken ct = default);

        Task DisconnectAsync();
    }
}