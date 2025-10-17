using CommunityToolkit.Mvvm.ComponentModel;

namespace DataReceiver.Models.Common
{
    public partial class ConnectionRuntimes : ObservableObject
    {
        [ObservableProperty]
        private ConnectionState state = ConnectionState.Disconnected;

        [ObservableProperty]
        private bool reconnecting = false;

        [ObservableProperty]
        private TimeSpan lastActivityTime = TimeSpan.MinValue;

        public string DisplayStatus => State switch
        {
            ConnectionState.Connected => $"已连接，最后活跃时间：{LastActivityTime}",
            ConnectionState.Disconnected => "断开连接",
            ConnectionState.Reconnecting => "重连中",
            _ => "未连接"
        };
    }
}
