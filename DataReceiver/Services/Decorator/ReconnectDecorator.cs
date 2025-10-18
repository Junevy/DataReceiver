using DataReceiver.Models.Common;
using DataReceiver.Models.Socket.Config;
using DataReceiver.Models.Socket.Interface;

namespace DataReceiver.Services.Decorator
{
    public class ReconnectDecorator : DecoratorBase
    {
        protected Timer? timer;
        protected int interval = 20;

        public ReconnectDecorator(IConnection inner) : base(inner)
        {
            if (inner is IReconnect isRcn && isRcn.ReconnectConfig is ReconnectConfig isRcnc)
                interval = isRcnc.Delay;
            else
                interval = -1;
        }

        public override async Task<ConnectionState> ConnectAsync(CancellationToken ct = default)
        {
            //if (interval == -1) return;

            var result = await inner.ConnectAsync();

            if (interval != -1 && interval > 100)
            {
                Runtimes.Reconnecting = true;
                timer = new Timer(StartReconnect, Cts.Token, 6000, interval);
            }
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
            if (Runtimes.State != ConnectionState.Connected && Runtimes.Reconnecting)
                //if()
                inner.ConnectAsync();
        }

        public override async Task DisconnectAsync()
        {
            Runtimes.Reconnecting = false;
            timer?.Dispose();
            _ = base.DisconnectAsync();
        }
    }
}
