namespace DataReceiver.Models
{
    public class TcpClientModel 
    {
        public string Ip { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string HeartBeat { get; set; } = "PING";
        public bool IsHeartBeat { get; set; } = false;

        public bool IsConnected { get; set; } = false;
        public bool IsAutoConnect { get; set; } = true;


    }
}
