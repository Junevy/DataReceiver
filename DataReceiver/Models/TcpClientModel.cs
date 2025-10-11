namespace DataReceiver.Models
{
    public class TcpClientModel : SocketModelBase
    {
        public string Ip { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string HeartBeat { get; set; } = "PING";
        public bool IsHeartBeat { get; set; } = false;

        public override void Start()
        {
            
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public override void Receive()
        {
            throw new NotImplementedException();
        }

        public override void ReStart()
        {
            throw new NotImplementedException();
        }

        public override void Send()
        {
            throw new NotImplementedException();
        }

    }
}
