using DataReceiver.Models.Config;
using DataReceiver.Models.Socket.Common;
using DataReceiver.Models.Socket.Interface;

namespace DataReceiver.Services.Decorator
{
    class HeartBeatDecorator(IConnection inner, HeartBeatConfig config) 
        : DecoratorBase(inner), IHeartBeatCapable
    {
        protected Timer? timer;
        public HeartBeatConfig HeartBeatConfig { get; } = config;
        public CancellationTokenSource HeartBeatToken
            => CancellationTokenSource.CreateLinkedTokenSource(Cts.Token);

        public override async Task<ConnectionState> ConnectAsync(CancellationToken ct = default)
        {
            //var result = await inner.ConnectAsync();

            //if (heartBeatConfig?.IsEnable == true)
            //{
            //    timer = new(StartHeartBeat, Cts.Token, 0, heartBeatConfig.Interval);
            //    Runtimes.LastHeartBeatTime = DateTime.Now;
            //}

            //return result;
            return ConnectionState.Connected;
        }

        private void StartHeartBeat(object? state)
        {
            //if (heartBeatConfig is null) timer?.Dispose();
            //if (state is null) return; // Log

            //var ct = (CancellationToken)state;

            //// 连接手动或异常断开连接后，刷新最后心跳时间，
            //// 防止重连成功后 DateTime.Now 与 LastHeartBeatTime 的差大于 心跳的Timeout，导致心跳功能自动关闭。
            //if (inner.Runtimes.State is not ConnectionState.Connected ||
            //    (ct == null && ct.IsCancellationRequested))
            //{
            //    //whenConnectInterrupt = DateTime.Now;
            //    Runtimes.LastHeartBeatTime = DateTime.Now;
            //    return;
            //}

            ////Runtimes.LastHeartBeatTime = whenConnectInterrupt = DateTime.Now;

            //if (DateTime.Now - Runtimes.LastHeartBeatTime > heartBeatConfig?.Timeout)
            //    if (heartBeatConfig?.EnableTimeout == true)
            //        timer?.Dispose();

            //inner.SendAsync(Encoding.UTF8.GetBytes(heartBeatConfig?.Response));
        }

        public override async Task DisconnectAsync()
        {
            timer?.Dispose();
            inner?.Dispose();
        }

        public Task StartHeartBeatAsync(byte[] response)
        {
            throw new NotImplementedException();
        }

        public void StopHeartBeat()
        {
            throw new NotImplementedException();
        }
    }
}