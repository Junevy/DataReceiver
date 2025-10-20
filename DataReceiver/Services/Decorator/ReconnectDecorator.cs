using DataReceiver.Models.Socket.Common;
using DataReceiver.Models.Socket.Config;
using DataReceiver.Models.Socket.Interface;

namespace DataReceiver.Services.Decorator
{
    public class ReconnectDecorator(IConnection inner, ReconnectConfig config) 
        : DecoratorBase(inner), IReconnectCapable
    {
        protected Timer? timer;
        public ReconnectConfig ReconnectConfig { get; } = config;
        public CancellationTokenSource ReconncetToken
            => CancellationTokenSource.CreateLinkedTokenSource(Cts.Token);

        public override async Task<ConnectionState> ConnectAsync(CancellationToken ct = default)
        {
            //if (interval == -1) return;

            var result = await inner.ConnectAsync();

            //if (reconnectConfig?.IsEnable == true && reconnectConfig?.Delay > 0)
            //{
            //    Runtimes.Reconnecting = true;
            //    timer = new Timer(StartReconnect, Cts.Token, 6000, reconnectConfig.Delay);
            //}
            //ReconnectDecorator 使用
            //System.Threading.Timer + async void（TimerCallback 不支持 async / await），
            //可能出现并发重入、多次并行 Connect、异常丢失。
            //timer = new Timer((e) =>
            //{
            //    if (result != ConnectionState.Connected && Runtimes.Reconnecting)
            //        inner.ConnectAsync();
            //}, Cts.Token, 6000, interval); 
            return result;
        }

        public void StartReconnect(object? state)
        {

            if (Runtimes.State != ConnectionState.Connected && !Runtimes.Reconnecting)
                inner.ConnectAsync();
        }

        public override async Task DisconnectAsync()
        {
            Runtimes.Reconnecting = false;
            timer?.Dispose();
            _ = base.DisconnectAsync();
        }

        public Task StartReconnectAsync()
        {
            //inner
            throw new NotImplementedException();
        }

        public void StopReconnect()
        {
            throw new NotImplementedException();
        }
    }
}
