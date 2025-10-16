using DataReceiver.Models.Common;
using DataReceiver.Models.CommunicationCommon;
using DataReceiver.Models.Socket.Interface;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DataReceiver.Models.Socket.Base
{
    public abstract class ReactiveBase 
    {
        /// <summary>
        /// 当前Socket状态
        /// </summary>
        public ConnectionState State { get; private set; } = ConnectionState.Disconnected;

        // Reactive Extensions
        private readonly Subject<DataEventArgs<byte>> dataReceived = new();
        private readonly BehaviorSubject<StateEventArgs> stateChanged
            = new(new StateEventArgs(ConnectionState.Disconnected, ConnectionState.Disconnected, "未连接"));
        public IObservable<DataEventArgs<byte>> DataReceived => dataReceived.AsObservable();
        public IObservable<StateEventArgs> StateChanged => stateChanged.AsObservable();

        protected virtual ConnectionState OnStateUpdated(ConnectionState state, string message = "")
        {
            var oldState = State;
            State = state;
            stateChanged.OnNext(new StateEventArgs(state, oldState, message));
            return state;
        }

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
            dataReceived?.OnCompleted();
            dataReceived?.Dispose();
            stateChanged?.OnCompleted();
            stateChanged?.Dispose();
        }
    }
}
