using CommunityToolkit.Mvvm.ComponentModel;
using DataReceiver.Models.Socket.Common;

namespace DataReceiver.Models.Common
{
    public partial class ConnectionRuntimes : ObservableObject
    {
        /// <summary>
        /// Socket 的连接状态
        /// </summary>
        [ObservableProperty]
        private ConnectionState state = ConnectionState.Disconnected;

        /// <summary>
        /// Socket 的最后活跃时间，接收到消息或心跳都会重置此时间
        /// </summary>
        [ObservableProperty]
        private string lastActivityTime = DateTime.MinValue.ToString("yyyy-MM-ss HH:mm:ss");

        /// <summary>
        /// 当前是否正在重新连接
        /// </summary>
        [ObservableProperty]
        private bool reconnecting = false;

        [ObservableProperty]
        private int currentReconnectAttempts = 0;

        /// <summary>
        /// 最后心跳时间
        /// </summary>
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
