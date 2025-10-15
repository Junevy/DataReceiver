using DataReceiver.Models.CommunicationModel;
using System.Text;

namespace DataReceiver.Services.Decorator
{
    class HeartBeatDecorator
    {
        //    private readonly ISocket inner = socket;
        //    private readonly HeartBeatConfig config = config;
        //    private Timer? heartBeatTimer;

        //    public async Task<Result> ConnectAsync(CancellationToken ct)
        //    {
        //        var result = await inner.ConnectAsync(ct);
        //        if (config.Enable && result == Result.Success)
        //        {
        //            heartBeatTimer = new Timer(
        //                async _ => await SendHeartBeat(),
        //                null,
        //                config.Interval,
        //                config.Interval);
        //        }
        //        return result;
        //    }

        //    public async Task SendHeartBeat()
        //    {
        //        var data = Encoding.UTF8.GetBytes(config.HeartBeat);
        //        await inner.SendAsync(data);
        //    }

        //    public async Task<Result> SendAsync(ReadOnlyMemory<byte> data, CancellationToken ct = default)
        //    {
        //        var result = await inner.SendAsync(data, ct);
        //        return result;
        //    }

        //    public void DisconnectAsync()
        //    {
        //        inner.DisconnectAsync();
        //    }

        //    public ValueTask DisposeAsync()
        //    {
        //        var result = inner.DisposeAsync();
        //        return result;
        //    }
        //}
    }
}
