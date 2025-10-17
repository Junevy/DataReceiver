using DataReceiver.Models.Common;
using DataReceiver.Models.Socket.Interface;
using HandyControl.Tools;
using System.Text;

namespace DataReceiver.Services.Decorator
{
    public class DecoratorBase(IConnection _inner) : IConnection
    {
        protected readonly IConnection inner = _inner;
        public CancellationTokenSource Cts  => inner.Cts;
        public ConnectionRuntimes Runtimes { get; private set; } = _inner.Runtimes;

        public virtual async Task<ConnectionState> ConnectAsync(CancellationToken ct = default)
        {
            return await inner.ConnectAsync();
        }

        public virtual async Task DisconnectAsync()
        {
            await inner.DisconnectAsync();
        }

        public async Task<int> SendAsync(byte[] data, CancellationToken ct = default)
        {
            return await inner.SendAsync(data);
        }

        public virtual void Dispose()
        {
            // 快照不应该管理生命周期。
            //if (Cts != null && !Cts.IsCancellationRequested)
            //{
            //    Cts.Cancel();
            //    Cts.Dispose();
            //}
            inner.Dispose();
        }
    }
}
