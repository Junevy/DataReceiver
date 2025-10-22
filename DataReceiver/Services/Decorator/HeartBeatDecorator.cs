using DataReceiver.Models.Config;
using DataReceiver.Models.Socket.Common;
using DataReceiver.Models.Socket.Interface;
using log4net;

namespace DataReceiver.Services.Decorator
{
    class HeartBeatDecorator(IReactiveConnection inner, HeartBeatConfig config)
        : ConnectionDecoratorBase(inner), IHeartBeatCapable
    {
        //private DateTime flagTime = DateTime.MinValue;
        private int flag = 0;
        private static readonly ILog Log = LogManager.GetLogger(typeof(HeartBeatDecorator));
        public HeartBeatConfig HeartBeatConfig { get; } = config;
        public CancellationTokenSource HeartBeatTokenSource { get; private set; } = new();

        public override async Task DisconnectAsync()
        {
            Log.Info("Waiting for disconnect.");
            StopHeartBeat();
            _ = base.DisconnectAsync();
        }

        public async Task StartHeartBeatAsync(byte[] response)
        {
            Log.Info("Waiting for start heartbeat task.");
            Runtimes.LastHeartBeatTime = DateTime.Now;
            flag = 0;

            if (HeartBeatTokenSource == null || HeartBeatTokenSource.IsCancellationRequested)
                HeartBeatTokenSource = new();
            response ??= [0x50, 0x6f, 0x6e, 0x67];  // Pong

            while (!HeartBeatTokenSource.IsCancellationRequested)
            {
                try
                {
                    if (DateTime.Now - Runtimes.LastHeartBeatTime > HeartBeatConfig.Timeout)
                    {
                        flag++;
                        if (flag >= HeartBeatConfig.MaxFailedCount)
                        {
                            if (HeartBeatConfig.EnableTimeout)
                            {
                                _ = DisconnectAsync();
                                break;
                            }
                            continue;
                        }
                    }

                    await Task.Delay(HeartBeatConfig.Interval, HeartBeatTokenSource.Token);
                    if (Runtimes.State != ConnectionState.Connected) continue;
                    await inner.SendAsync(response, HeartBeatTokenSource.Token);
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
            HeartBeatTokenSource?.Cancel();
        }

        public override void Dispose()
        {
            Log.Info("Disposing the decorator.");
            HeartBeatTokenSource?.Cancel();
            HeartBeatTokenSource?.Dispose();
            base.Dispose();
        }
    }
}