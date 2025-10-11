using System.IO;
using System.Net.Sockets;
using System.Text;

namespace DataReceiver.Models
{
    public class TcpClientModel : SocketModelBase
    {
        public string Ip { get; set; } = "192.168.31.163";
        public int Port { get; set; } = 9007;
        public string HeartBeat { get; set; } = "PING";
        public bool IsHeartBeat { get; set; } = false;
        public TcpClient? Client { get; set; }
        private TimeSpan Timeout { get; set; } = TimeSpan.FromMilliseconds(5000);
        private TimeSpan Interval { get; set; } = TimeSpan.FromMilliseconds(10000);

        public event Action<string>? OnReceivedMessage;
        public event Action<bool>? OnStateChanged;

        private CancellationTokenSource cts;

        public bool IsConnected => Client != null
                           && !(Client.Client.Poll(10000, SelectMode.SelectRead)
                                && Client.Client.Available == 0) && Client.Connected;

        public override async Task StartAsync()
        {
            cts ??= new();
            Client ??= new();
            //var timeoutTask = Task.Delay(Timeout, cts.Token);
            //Task anyTask = await Task.WhenAny(timeoutTask, Client?.ConnectAsync(Ip, Port));

            //if (anyTask == timeoutTask)
            //{
            //    Stop();
            //    return;
            //}
            Client?.ConnectAsync(Ip, Port);
            OnStateChanged?.Invoke(IsConnected);

            if (IsHeartBeat) _ = StartHeartBeatAsync();
            if (IsAutoConnect) _ = StartAutoConnectAsync();

            LastActivedTime = DateTime.Now;
            _ = ReceiveAsync(Client!.GetStream());
        }

        public override void Stop()
        {
            if (!cts.IsCancellationRequested)
            {
                cts.Cancel();
                cts.Dispose();
            }

            Client?.Close();
            Client?.Dispose();
            Client = null;
            OnStateChanged?.Invoke(IsConnected);
        }

        public override async Task ReceiveAsync(Stream stream)
        {
            var buffer = new byte[1024];
            while (!cts.IsCancellationRequested)
            {
                var bytes = await stream.ReadAsync(buffer, 0, buffer.Length, cts.Token);
                var message = Encoding.UTF8.GetString(buffer, 0, bytes);

                if (bytes == 0)
                {
                    Stop();
                    return;
                }

                if (message.Equals(HeartBeat))
                    _ = SendAsync(HeartBeat);
                else
                    OnReceivedMessage?.Invoke(message);

                LastActivedTime = DateTime.Now;
            }
        }

        public override void ReStart()
        {
            Stop();
            _ = StartAsync();
        }

        public override async Task SendAsync(string message)
        {
            try
            {
                var msg = Encoding.UTF8.GetBytes(message);
                using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token);
                timeoutCts.CancelAfter(Timeout);
                await Client!.GetStream().WriteAsync(msg, 0, msg.Length, timeoutCts.Token);
            }
            catch (TaskCanceledException)
            {
                if (IsConnected) Stop();
            }
        }

        public async Task StartHeartBeatAsync()
        {
            while (!cts.IsCancellationRequested)
            {
                await Task.Delay(Interval);
                if (!IsHeartBeat) return;
                if (!IsConnected) continue;
                _ = SendAsync(HeartBeat);
            }
        }

        public async Task StartAutoConnectAsync()
        {
            while (IsAutoConnect)
            {
                await Task.Delay(Interval);
                //if (!IsAutoConnect) return;
                if (!IsConnected)
                {
                    ReStart();
                }
            }
        }
    }
}
