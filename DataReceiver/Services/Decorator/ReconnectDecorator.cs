using DataReceiver.Models.Socket.Common;
using DataReceiver.Models.Socket.Config;
using DataReceiver.Models.Socket.Interface;
using log4net;

namespace DataReceiver.Services.Decorator
{
    public class ReconnectDecorator(IReactiveConnection inner, ReconnectConfig config) 
        : ConnectionDecoratorBase(inner), IReconnectCapable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ReconnectDecorator));
        public ReconnectConfig ReconnectConfig { get; } = config;
        public CancellationTokenSource ReconnectTokenSource { get; private set; } = new();

        public override async Task DisconnectAsync()
        {
            Log.Info("Waiting for disconnect.");
            StopReconnect();
            _ = base.DisconnectAsync();
        }

        public async Task StartReconnectAsync()
        {
            Log.Info("Waiting for start reconnect task.");
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
                    Log.Warn("Reconnect task canceled.");

                    break;
                }
                catch (Exception e)
                {
                    Log.Error($"Reconnect task occured an error : {e.Message}");
                }
            }
        }

        public void StopReconnect() 
        {
            Log.Info("Waiting for stop reconnect task.");
            Runtimes.Reconnecting = false; 
            ReconnectTokenSource?.Cancel(); 
        }

        public override void Dispose()
        {
            Log.Info("Disposing the decorator.");
            ReconnectTokenSource?.Cancel();
            ReconnectTokenSource?.Dispose();
            base.Dispose();
        }
    }
}
