using CommunityToolkit.Mvvm.ComponentModel;

namespace DataReceiver.Models.Socket.Config
{
    public partial class ReconnectConfig : ObservableValidator
    {
        /// <summary>
        /// 是否开启断线重连
        /// </summary>
        [ObservableProperty]
        private bool isEnable = true;

        /// <summary>
        /// 重连间隔时间，Unit：ms
        /// </summary>
        [ObservableProperty]
        private int delay = 10000;

        /// <summary>
        /// 最大重试次数
        /// </summary>
        [ObservableProperty]
        private int maxRetryCount = 100;

        /// <summary>
        /// 是否启用最大限制
        /// </summary>
        [ObservableProperty]
        private bool enableMaxLimit = false;

        /// <summary>
        /// 延迟增长因子
        /// </summary>
        [ObservableProperty]
        public double backoffMultiplier = 2.0;
    }
}
