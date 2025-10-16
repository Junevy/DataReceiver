using CommunityToolkit.Mvvm.ComponentModel;
using System.Security.Permissions;

namespace DataReceiver.Models.Config
{
    public  abstract partial class CommunicationConfig : ObservableValidator
    {
        /// <summary>
        /// 是否开启断线重连
        /// </summary>
        [ObservableProperty]
        private bool enableReconnect = true;

        /// <summary>
        /// 重连间隔时间
        /// </summary>
        [ObservableProperty]
        private TimeSpan reconnectDelay = TimeSpan.FromSeconds(10);

        /// <summary>
        /// 连接超时时间
        /// </summary>
        [ObservableProperty]
        private TimeSpan connectTimeout = TimeSpan.FromSeconds(5);

        /// <summary>
        /// 最后活跃时间
        /// </summary>
        [ObservableProperty]
        private DateTime lastActivityTime = DateTime.MinValue;

        private string Encoding { get; set; } = "UTF-8";

        /// <summary>
        /// 缓冲区大小
        /// </summary>
        public int BufferSize { get; set; } = 1024;


        //protected CommunicationConfig()
        //{
        //    //OnPropertyChanged();
        //}

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
