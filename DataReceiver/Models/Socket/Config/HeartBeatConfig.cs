using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataReceiver.Models.Config
{
    public partial class HeartBeatConfig : ObservableValidator
    {
        [ObservableProperty]
        public bool isEnable = true;

        /// <summary>
        /// 响应的心跳内容
        /// </summary>
        [ObservableProperty]
        [Required(ErrorMessage = "心跳内容不能为空")]
        public string response = "PING";

        /// <summary>
        /// 接收的心跳内容
        /// </summary>
        [ObservableProperty]
        [Required(ErrorMessage = "心跳内容不能为空")]
        public string request = "PONG";

        [ObservableProperty]
        [Required(ErrorMessage = "心跳间隔不能为空")]
        public int interval = 10000;

        /// <summary>
        /// 心跳超时时间
        /// </summary>
        [ObservableProperty]
        [Required(ErrorMessage = "心跳超时不能为空")]
        public TimeSpan timeout = TimeSpan.FromSeconds(30);

        /// <summary>
        /// 失败次数达到最大时，是否禁用心跳功能
        /// </summary>
        [ObservableProperty]
        public bool enableTimeout = false;

        /// <summary>
        /// 最大失败次数
        /// </summary>
        [ObservableProperty]
        [Required(ErrorMessage = "失败次数不能为空")]
        public int maxFailedCount = 5;

        //partial void OnHeartBeatChanged(string value)
        //{
        //    Console.WriteLine(value);
        //}
    }
}
