using DataReceiver.Models.Common;
using DataReceiver.Models.Socket.Interface;

namespace DataReceiver.Services.Decorator
{
    public class ReconnectDecorator(IConnection inner, int interval) : DecoratorBase(inner)
    {
        protected Timer? timer;
        protected int interval = interval;

        public override async Task<ConnectionState> ConnectAsync(CancellationToken ct = default)
        {
            Runtimes.Reconnecting = true;

            timer = new Timer(async (e) =>
            {
                if (Runtimes.State != ConnectionState.Connected && Runtimes.Reconnecting)
                    await inner.ConnectAsync();
            }, null, 0, interval);

            return ConnectionState.Reconnecting;
        }

        public override async Task DisconnectAsync()
        {
            Runtimes.Reconnecting = false;
            //timer
            timer?.Dispose();
            await base.DisconnectAsync();
        }
    }
}
