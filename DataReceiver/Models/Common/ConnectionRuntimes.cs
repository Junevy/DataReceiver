using CommunityToolkit.Mvvm.ComponentModel;

namespace DataReceiver.Models.Common
{
    public partial class ConnectionRuntimes : ObservableObject
    {
        [ObservableProperty]
        private ConnectionState state = ConnectionState.Disconnected;

        [ObservableProperty]
        private string lastActivityTime = DateTime.MinValue.ToString("yyyy-MM-ss HH:mm:ss");

        [ObservableProperty]
        private bool reconnecting = false;

        [ObservableProperty]
        private int currentReconnectAttempts = 0;

        [ObservableProperty]
        private DateTime lastHeartBeatTime = DateTime.Now;


        //[ObservableProperty]
        //private DateTime lastReconnectTime = DateTime.MinValue;

        //[ObservableProperty]
        //private int currentHeartBeatFail = 0;


        public string DisplayStatus => State switch
        {
            ConnectionState.Connected => $"已连接，最后活跃时间：{LastActivityTime}",
            ConnectionState.Disconnected => "断开连接",
            ConnectionState.Reconnecting => "重连中",
            _ => "未连接"
        };
    }
}
