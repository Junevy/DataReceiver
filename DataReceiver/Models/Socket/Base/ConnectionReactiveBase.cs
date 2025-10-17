using DataReceiver.Models.Common;
using DataReceiver.Models.CommunicationCommon;
using DataReceiver.Models.Socket.Interface;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DataReceiver.Models.Socket.Base
{
    public abstract class ConnectionReactiveBase : IConnection
    {
        public CancellationTokenSource Cts { get; protected set; } = new();
        public ConnectionRuntimes Runtimes { get; private set; } = new();

        // Reactive Extensions
        private readonly Subject<DataEventArgs<byte>> dataReceived = new();
        private readonly BehaviorSubject<StateEventArgs> stateChanged
            = new(new StateEventArgs(ConnectionState.Disconnected, ConnectionState.Disconnected, "未连接"));
        public IObservable<DataEventArgs<byte>> DataReceived => dataReceived.AsObservable();
        public IObservable<StateEventArgs> StateChanged => stateChanged.AsObservable();

        /// <summary>
        /// 订阅Subject的信息流，唯一控制State的方法
        /// </summary>
        /// <param name="state">新的Socket连接状态</param>
        /// <param name="message">Socket连接状态发生变化时的附带消息</param>
        /// <returns>当前Socket连接状态</returns>
        protected virtual ConnectionState OnStateUpdated(ConnectionState state, string message = "")
        {
            var oldState = Runtimes.State;
            Runtimes.State = state; //*********多线程环境下不安全！*************
            stateChanged.OnNext(new StateEventArgs(state, oldState, message));
            return state;
        }

        /// <summary>
        /// 订阅Socket接收到数据时的信息流
        /// </summary>
        /// <param name="data">接收到的数据</param>
        /// <param name="message">接收到数据时附带的消息</param>
        /// <returns>接收到数据的长度</returns>
        protected virtual int OnDataReceived(ReadOnlyMemory<byte> data, string message = "")
        {
            dataReceived.OnNext(new DataEventArgs<byte>(data, data.Length, DateTime.Now)
            {
                Message = message
            });
            return data.Length;
        }

        public virtual void Dispose()
        {
            try { dataReceived?.OnCompleted(); dataReceived?.Dispose(); } catch { }
            try { stateChanged?.OnCompleted(); stateChanged?.Dispose(); } catch { }
            GC.SuppressFinalize(this);
        }

        public abstract Task<ConnectionState> ConnectAsync(CancellationToken ct = default);

        public abstract Task<int> SendAsync(byte[] data, CancellationToken ct = default);

        public abstract Task DisconnectAsync();
    }
}
