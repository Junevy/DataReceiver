using DataReceiver.Models.Common;
using DataReceiver.Models.Socket.Common;
using DataReceiver.Models.Socket.Interface;

namespace DataReceiver.Services.Decorator
{
    public class ConnectionDecoratorBase(IReactiveConnection _inner) : IReactiveConnection
    {
        protected readonly IReactiveConnection inner = _inner;
        public IReactiveConnection Inner => inner;
        //public CancellationTokenSource Cts => inner.Cts;
        public ConnectionRuntimes Runtimes => inner.Runtimes;

        public virtual async Task<ConnectionState> ConnectAsync(CancellationToken ct = default)
            => await inner.ConnectAsync();

        public virtual async Task DisconnectAsync() => await inner.DisconnectAsync().ConfigureAwait(false);

        public async Task<int> SendAsync(byte[] data, CancellationToken ct = default)
            => await inner.SendAsync(data);

        public virtual void Dispose() => inner.Dispose();

        public ConnectionState OnStateUpdated(ConnectionState state, string message = "")
            => inner.OnStateUpdated(state, message);

        public int OnDataReceived(ReadOnlyMemory<byte> data, string message = "")
            => inner.OnDataReceived(data, message);
    }
}
