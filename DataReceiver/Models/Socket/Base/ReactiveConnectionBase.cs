using DataReceiver.Models.Common;
using DataReceiver.Models.Socket.Common;
using DataReceiver.Models.Socket.Interface;
using log4net;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DataReceiver.Models.Socket.Base
{
    public abstract class ReactiveConnectionBase : IReactiveConnection
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ReactiveConnectionBase));

        public ConnectionRuntimes Runtimes { get; private set; } = new();

        // Reactive Extensions
        private readonly Subject<DataEventArgs<byte>> dataReceived = new();
        private readonly BehaviorSubject<StateEventArgs> stateChanged
            = new(new StateEventArgs(ConnectionState.Disconnected, ConnectionState.Disconnected, "未连接"));

        public IObservable<DataEventArgs<byte>> DataReceived => dataReceived.AsObservable();
        public IObservable<StateEventArgs> StateChanged => stateChanged.AsObservable();

        public virtual ConnectionState OnStateUpdated(ConnectionState state, string message = "")
        {
            Log.Info($"On State Changed : {state}");
            stateChanged.OnNext(new StateEventArgs(state, Runtimes.State, message));
            return state;
        }

        public virtual int OnDataReceived(ReadOnlyMemory<byte> data, string message = "")
        {
            Log.Info($"On Date Received : {data}");
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
