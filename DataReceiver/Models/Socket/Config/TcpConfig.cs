using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataReceiver.Models.Config
{
    public partial class TcpConfig : CommunicationConfig
    {
        //public HeartBeatConfig? HeartBeatConfig { get; private set; }
        //public ReconnectConfig? ReconnectConfig { get; private set; }

        [ObservableProperty]
        [Required(ErrorMessage = "IP地址不能为空")]
        [RegularExpression(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$",
            ErrorMessage = "请输入有效的IP地址")]
        [NotifyDataErrorInfo] // 自动触发验证
        private string ip = "192.168.31.163";

        [ObservableProperty]
        [Required(ErrorMessage = "端口号不能为空")]
        [Range(1025, 65535, ErrorMessage = "端口号必须在1025到65535之间")]
        [NotifyDataErrorInfo]
        private int port = 9007;

        [ObservableProperty]
        private bool enableNagle = true;

        [ObservableProperty]
        private bool enableKeepAlive = true;

        /// <summary>
        /// KeepAlive时间（毫秒）
        /// </summary>
        [ObservableProperty]
        [Range(1, 100000, ErrorMessage = "值必须位于1-100000之间")]
        [Required(ErrorMessage = "KeepAlive时间不能为空")]
        [NotifyDataErrorInfo]
        private int keepAliveTime = 60000;

        /// <summary>
        /// Keep-Alive间隔（毫秒）
        /// </summary>
        [ObservableProperty]
        [Range(1, 100000, ErrorMessage = "值必须位于1-100000之间")]
        [Required(ErrorMessage = "KeepAlive时间不能为空")]
        [NotifyDataErrorInfo]
        private int keepAliveInterval = 10000;
    }
}
