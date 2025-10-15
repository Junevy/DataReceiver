using DataReceiver.Models.Config;

namespace DataReceiver.Models.Socket.Base
{
    public abstract class ConnectionBase<TSocket, KConfig>(KConfig config)
        : SocketBase
        where TSocket : IDisposable
        where KConfig : CommunicationConfig
    {
        /// <summary>
        /// Socket对象
        /// </summary>
        protected TSocket Socket { get; set; } = default!;

        /// <summary>
        /// Socket配置
        /// </summary>
        public KConfig Config { get; }
            = config ?? throw new ArgumentNullException($"Socket配置文件为空: {nameof(Config)}");

        /// <summary>
        /// 释放所有资源
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (cts != null && !cts.IsCancellationRequested)
            {
                cts?.Cancel();
                cts?.Dispose();
            }
            Socket?.Dispose();
        }
    }
}

#region old code
//protected virtual void Dispose(bool disposing)
//{
//    if (disposing)
//    {
//        cts?.Cancel();
//        cts?.Dispose();
//        dataReceived?.OnCompleted();
//        dataReceived?.Dispose();
//        stateChanged?.OnCompleted();
//        stateChanged?.Dispose();
//        Socket?.Dispose();
//    }
//}

/// <summary>
/// 取消令牌源
/// </summary>
//public CancellationTokenSource cts = new();

///// <summary>
///// 当前Socket状态
///// </summary>
//public ConnectionState State { get; private set; } = ConnectionState.Disconnected;

//// Reactive Extensions
//private readonly Subject<DataEventArgs<byte>> dataReceived = new();
//private readonly BehaviorSubject<StateEventArgs> stateChanged
//    = new(new StateEventArgs(ConnectionState.Disconnected, ConnectionState.Disconnected, "未连接"));
//public IObservable<DataEventArgs<byte>> DataReceived => dataReceived.AsObservable();
//public IObservable<StateEventArgs> StateChanged => stateChanged.AsObservable();

//public abstract Task<ConnectionState> ConnectAsync(CancellationToken ct);
//public abstract void Disconnect();
//public abstract Task<int> SendAsync(byte[] data, CancellationToken ct = default);
//public abstract Task<int> ReceiveAsync(Stream stream, CancellationToken ct = default);

//protected virtual ConnectionState OnStateUpdated(ConnectionState state, string message = "")
//{
//    var oldState = State;
//    State = state;
//    stateChanged.OnNext(new StateEventArgs(state, oldState, message));
//    return state;
//}

//protected virtual int OnDataReceived(ReadOnlyMemory<byte> data, string message = "")
//{
//    dataReceived.OnNext(new DataEventArgs<byte>(data, data.Length, DateTime.Now)
//    {
//        Message = message
//    });
//    return data.Length;
//}
#endregion
