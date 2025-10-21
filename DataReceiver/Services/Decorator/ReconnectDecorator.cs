using DataReceiver.Models.Socket.Common;
using DataReceiver.Models.Socket.Config;
using DataReceiver.Models.Socket.Interface;

namespace DataReceiver.Services.Decorator
{
    public class ReconnectDecorator(IReactiveConnection inner, ReconnectConfig config) 
        : ConnectionDecoratorBase(inner), IReconnectCapable
    {
        public ReconnectConfig ReconnectConfig { get; } = config;
        public CancellationTokenSource ReconnectTokenSource { get; private set; } = new();

        public override async Task DisconnectAsync()
        {
            StopReconnect();
            _ = base.DisconnectAsync();
        }

        public async Task StartReconnectAsync()
        {
            if (ReconnectConfig.Delay <= 0 || Runtimes.Reconnecting) return;
            if (ReconnectTokenSource == null || ReconnectTokenSource.IsCancellationRequested) 
                ReconnectTokenSource = new();
            Runtimes.Reconnecting = true;

            while (!ReconnectTokenSource.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(ReconnectConfig.Delay, ReconnectTokenSource.Token);
                    if (Runtimes.State != ConnectionState.Connected)
                        await Inner.ConnectAsync();
                }
                catch (OperationCanceledException)
                {
                    // Log...
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void StopReconnect() 
        { 
            Runtimes.Reconnecting = false; 
            ReconnectTokenSource?.Cancel(); 
        }

        public override void Dispose()
        {
            ReconnectTokenSource?.Cancel();
            ReconnectTokenSource?.Dispose();
            base.Dispose();
        }
    }
}
