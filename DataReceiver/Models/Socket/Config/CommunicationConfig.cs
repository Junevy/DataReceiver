using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataReceiver.Models.Config
{
    public abstract partial class CommunicationConfig : ObservableValidator
    {
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
        private int port = 9008;

        /// <summary>
        /// 连接超时时间，Unit：ms
        /// </summary>
        [ObservableProperty]
        private int connectTimeout = 5000;

        ///// <summary>
        ///// 最后活跃时间
        ///// </summary>
        [ObservableProperty]
        private DateTime lastActivityTime = DateTime.MinValue;

        [ObservableProperty]
        private string encoding = "utf-8";

        /// <summary>
        /// 缓冲区大小，Unit：byte
        /// </summary>
        [ObservableProperty]
        private int bufferSize = 1024;

        //[ObservableProperty]
        //private string sendMessage = string.Empty;

        [ObservableProperty]
        private int sendTimeOut = 2000;


        ///// <summary>
        ///// 重连策略配置
        ///// </summary>
        //public class ReconnectPolicyConfig
        //{
        //    /// <summary>
        //    /// 最大重试次数，-1表示无限重试
        //    /// </summary>
        //    public int MaxRetryCount { get; set; } = -1;

        //    /// <summary>
        //    /// 初始延迟
        //    /// </summary>
        //    public TimeSpan InitialDelay { get; set; } = TimeSpan.FromSeconds(1);

        //    /// <summary>
        //    /// 最大延迟
        //    /// </summary>
        //    public TimeSpan MaxDelay { get; set; } = TimeSpan.FromSeconds(30);

        //    /// <summary>
        //    /// 延迟增长因子
        //    /// </summary>
        //    public double BackoffMultiplier { get; set; } = 2.0;
        //}
    }
}
