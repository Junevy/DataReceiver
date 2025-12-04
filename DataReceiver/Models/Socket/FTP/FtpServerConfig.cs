using CommunityToolkit.Mvvm.ComponentModel;
using DataReceiver.Models.Config;
using System.ComponentModel.DataAnnotations;

namespace DataReceiver.Models.Socket.Config
{
    public partial class FtpServerConfig : CommunicationConfig
    {
        [ObservableProperty]
        private string rootPath = @$"D:/FTP/Default/{DateTime.Now:yyyy-MM-dd}";

        [ObservableProperty]
        [Required(ErrorMessage = "公共用户名不能为空")]
        [NotifyDataErrorInfo] 
        private string userName = "admin";

        [ObservableProperty]
        [Required(ErrorMessage = "公共密码不能为空")]
        [NotifyDataErrorInfo]
        private string password = "123456";

        [ObservableProperty]
        private int maxConnections = 30;

        [ObservableProperty]
        private bool allowAnonymous = true;

        [ObservableProperty]
        private TimeSpan inactiveCheckInterval = TimeSpan.FromMinutes(1);

        //[ObservableProperty]
        //private bool enableScheduleClean = false;

        [ObservableProperty]
        private int keepDays = 30;



        //[ObservableProperty]
        //private int scanIntervalDays = 1;

        [ObservableProperty]
        private TimeSpan inactiveTimeOut = TimeSpan.FromMinutes(5);

    }
}
