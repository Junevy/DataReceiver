using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataReceiver.Models.Config
{
    public partial class HeartBeatConfig : ObservableValidator
    {
        /// <summary>
        /// 心跳内容
        /// </summary>
        [ObservableProperty]
        [Required(ErrorMessage = "心跳内容不能为空")]
        public string heartBeat = "PING";

        [ObservableProperty]
        public bool enable = true;

        [ObservableProperty]
        [Required(ErrorMessage = "心跳间隔不能为空")]
        public int heartbeatInterval = 10;

        /// <summary>
        /// 心跳超时时间
        /// </summary>
        [ObservableProperty]
        [Required(ErrorMessage = "心跳超时不能为空")]
        public int timeout = 30;

        /// <summary>
        /// 最大失败次数
        /// </summary>
        [ObservableProperty]
        [Required(ErrorMessage = "失败次数不能为空")]
        public int maxFailedCount = 5;

        partial void OnHeartBeatChanged(string value)
        {
            Console.WriteLine(value);
        }
    }
}
