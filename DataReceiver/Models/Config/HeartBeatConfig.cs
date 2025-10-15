using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReceiver.Models.Config
{
    public class HeartBeatConfig
    {
        /// <summary>
        /// 心跳内容
        /// </summary>
        public string HeartBeat { get; set; } = "Junevy";
        public bool Enable { get; set; } = true;
        public TimeSpan HeartbeatInterval { get; set; } = TimeSpan.FromSeconds(5);

        /// <summary>
        /// 心跳超时时间
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(20);

        /// <summary>
        /// 最大失败次数
        /// </summary>
        public int MaxFailedCount { get; set; } = 5;
    }
}
