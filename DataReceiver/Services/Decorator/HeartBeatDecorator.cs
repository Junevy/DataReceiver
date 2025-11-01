using DataReceiver.Models.Config;
using DataReceiver.Models.Socket.Common;
using DataReceiver.Models.Socket.Interface;
using log4net;

namespace DataReceiver.Services.Decorator
{
    class HeartBeatDecorator(IConnection inner, HeartBeatConfig config)
        : ConnectionDecoratorBase(inner), IHeartBeatCapable
    {
        private int flag = 0;
        private static readonly new ILog Log = LogManager.GetLogger(typeof(HeartBeatDecorator));
        public HeartBeatConfig Config { get; } = config;
        public CancellationTokenSource TokenSource { get; private set; } = new();

        public override async Task DisconnectAsync()
        {
            Log.Info("Waiting for disconnect.");
            StopHeartBeat();
            _ = inner.DisconnectAsync();
        }

        public async Task StartHeartBeatAsync(byte[] response)
        {
            Log.Info("Waiting for start heartbeat task.");
            Runtimes.LastHeartBeatTime = DateTime.Now;
            flag = 0;

            if (TokenSource == null || TokenSource.IsCancellationRequested)
                TokenSource = new();
            response ??= [0x50, 0x6f, 0x6e, 0x67];  // Pong

            while (!TokenSource.IsCancellationRequested)
            {
                try
                {
                    if (DateTime.Now - Runtimes.LastHeartBeatTime > Config.Timeout)
                    {
                        flag++;
                        Log.Info($"Filed Count : {flag}");

                        if (flag >= Config.MaxFailedCount && Config.EnableTimeout)
                        {
                            Log.Warn($"Filed count achived: {flag}, waiting for disconnect the socket.");
                            _ = DisconnectAsync();
                            break;
                        }
                    }

                    await Task.Delay(Config.Interval, TokenSource.Token);
                    if (Runtimes.State != ConnectionState.Connected) continue;

                    await inner.SendAsync(response, TokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    Log.Warn("HeartBeat task canceled.");
                    break;
                }
                catch (Exception e)
                {
                    Log.Error($"HeartBeat task occured an error : {e.Message}");
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void StopHeartBeat()
        {
            Log.Info("Waiting for stop heartbeat task.");
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