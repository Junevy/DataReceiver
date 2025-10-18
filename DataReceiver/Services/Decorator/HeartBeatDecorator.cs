using DataReceiver.Models.Common;
using DataReceiver.Models.Config;
using DataReceiver.Models.Socket.Interface;
using System.Text;

namespace DataReceiver.Services.Decorator
{
    class HeartBeatDecorator : DecoratorBase
    {
        protected Timer? timer;
        protected HeartBeatConfig? config;

        public HeartBeatDecorator(IConnection inner) : base(inner)
        {
            if (inner is IHeartBeat isHb && isHb.HeartBeatConfig is HeartBeatConfig isHbc)
                config = isHbc;
            else
                config = null;
        }

        public override async Task<ConnectionState> ConnectAsync(CancellationToken ct = default)
        {
            var result = await inner.ConnectAsync();

            if (config?.Enable == true)
            {
                timer = new(StartHeartBeat, Cts.Token, 0, config.Interval);
                Runtimes.LastHeartBeatTime = DateTime.Now;
            }

            return result;
        }

        private void StartHeartBeat(object? state)
        {
            if (config is null) timer?.Dispose();
            if (state is null) return; // Log

            var ct = (CancellationToken)state;

            // 连接手动或异常断开连接后，刷新最后心跳时间，
            // 防止重连成功后 DateTime.Now 与 LastHeartBeatTime 的差大于 心跳的Timeout，导致心跳功能自动关闭。
            if (inner.Runtimes.State is not ConnectionState.Connected ||
                (ct == null && ct.IsCancellationRequested))
            {
                //whenConnectInterrupt = DateTime.Now;
                Runtimes.LastHeartBeatTime = DateTime.Now;
                return;
            }

            //Runtimes.LastHeartBeatTime = whenConnectInterrupt = DateTime.Now;

            if (DateTime.Now - Runtimes.LastHeartBeatTime > config?.Timeout)
                if (config?.EnableTimeout == true)
                    timer?.Dispose();

            inner.SendAsync(Encoding.UTF8.GetBytes(config?.Response));
        }

        public override async Task DisconnectAsync()
        {
            timer?.Dispose();
            inner?.Dispose();
        }
    }
}