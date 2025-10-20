using DataReceiver.Models.Common;
using DataReceiver.Models.Socket.Common;
using DataReceiver.Models.Socket.Interface;

namespace DataReceiver.Services.Decorator
{
    public class DecoratorBase(IConnection _inner) : IConnection
    {
        protected readonly IConnection inner = _inner;
        public IConnection Inner => inner;
        public CancellationTokenSource Cts => inner.Cts;
        public ConnectionRuntimes Runtimes => inner.Runtimes;

        public virtual async Task<ConnectionState> ConnectAsync(CancellationToken ct = default)
            => await inner.ConnectAsync();

        public virtual async Task DisconnectAsync() => await inner.DisconnectAsync().ConfigureAwait(false);

        public async Task<int> SendAsync(byte[] data, CancellationToken ct = default)
            => await inner.SendAsync(data);

        public virtual void Dispose() => inner.Dispose();
    }
}
