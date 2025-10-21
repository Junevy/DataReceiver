using DataReceiver.Models.Config;
using DataReceiver.Models.Socket.Common;
using DataReceiver.Models.Socket.Interface;

namespace DataReceiver.Services.Decorator
{
    class HeartBeatDecorator(IReactiveConnection inner, HeartBeatConfig config)
        : ConnectionDecoratorBase(inner), IHeartBeatCapable
    {
        public HeartBeatConfig HeartBeatConfig { get; } = config;
        public CancellationTokenSource HeartBeatTokenSource { get; private set; } = new();

        public override async Task DisconnectAsync()
        {
            StopHeartBeat();
            _ = base.DisconnectAsync();
        }

        public async Task StartHeartBeatAsync(byte[] response)
        {
            if (HeartBeatTokenSource == null || HeartBeatTokenSource.IsCancellationRequested)
                HeartBeatTokenSource = new();
            response ??= [0x50, 0x6f, 0x6e, 0x67];  // Pong

            while (!HeartBeatTokenSource.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(HeartBeatConfig.Interval, HeartBeatTokenSource.Token);

                    if (Runtimes.State != ConnectionState.Connected) continue;
                    await inner.SendAsync(response, HeartBeatTokenSource.Token);
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

        public void StopHeartBeat() => HeartBeatTokenSource?.Cancel();

        public override void Dispose()
        {
            HeartBeatTokenSource?.Cancel();
            HeartBeatTokenSource?.Dispose();
            base.Dispose();
        }
    }
}