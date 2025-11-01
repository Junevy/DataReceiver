using DataReceiver.Models.Socket.Common;
using DataReceiver.Models.Socket.Config;
using DataReceiver.Models.Socket.Interface;
using log4net;

namespace DataReceiver.Services.Decorator
{
    public class ReconnectDecorator(IConnection inner, ReconnectConfig config) 
        : ConnectionDecoratorBase(inner), IReconnectCapable
    {
        private static readonly new ILog Log = LogManager.GetLogger(typeof(ReconnectDecorator));
        public ReconnectConfig Config { get; } = config;
        public CancellationTokenSource TokenSource { get; private set; } = new();

        public override async Task DisconnectAsync()
        {
            Log.Info("Waiting for disconnect.");
            StopReconnect();
            _ = inner.DisconnectAsync();
        }

        public async Task StartReconnectAsync()
        {
            Log.Info("Waiting for start reconnect task.");
            if (Config.Delay <= 0 || Runtimes.Reconnecting) return;
            if (TokenSource == null || TokenSource.IsCancellationRequested)
                TokenSource = new();
            Runtimes.Reconnecting = true;

            while (!TokenSource.IsCancellationRequested && Config.IsEnable)
            {
                try
                {
                    await Task.Delay(Config.Delay, TokenSource.Token);
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
            TokenSource?.Cancel(); 
        }

        public override void Dispose()
        {
            Log.Info("Disposing the decorator.");
            TokenSource?.Cancel();
            TokenSource?.Dispose();
            inner.Dispose();
        }
    }
}
