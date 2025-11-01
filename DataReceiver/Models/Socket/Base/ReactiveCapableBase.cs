using DataReceiver.Models.Common;
using DataReceiver.Models.Socket.Common;
using DataReceiver.Models.Socket.Interface;
using log4net;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DataReceiver.Models.Socket.Base
{
    public abstract class ReactiveCapableBase : IReactiveCapable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ReactiveCapableBase));

        // Reactive Extensions
        private readonly Subject<DataEventArgs<byte>> dataReceived = new();
        private readonly BehaviorSubject<StateEventArgs> stateChanged
            = new(new StateEventArgs(ConnectionState.Disconnected, ConnectionState.Disconnected, "未连接"));

        public IObservable<DataEventArgs<byte>> DataObservable => dataReceived.AsObservable();
        public IObservable<StateEventArgs> StateObservable => stateChanged.AsObservable();

        public virtual ConnectionState OnStateUpdated(ConnectionState newState, ConnectionState oldState, string message = "")
        {
            Log.Info($"On State Changed : {newState}");
            stateChanged.OnNext(new StateEventArgs(newState, oldState, message));
            return newState;
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
    }
}
