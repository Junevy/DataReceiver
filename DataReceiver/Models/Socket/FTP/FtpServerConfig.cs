using CommunityToolkit.Mvvm.ComponentModel;
using DataReceiver.Models.Config;
using System.ComponentModel.DataAnnotations;

namespace DataReceiver.Models.Socket.Config
{
    public partial class FtpServerConfig : CommunicationConfig
    {
        [ObservableProperty]
        [Required(ErrorMessage = "路径不能为空")]
        [NotifyDataErrorInfo]
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

        [ObservableProperty]
        [Required(ErrorMessage = "保留天数不能为空")]
        [RegularExpression(@"^(36[0-5]|3[0-5][0-9]|[12][0-9]{2}|[1-9][0-9]?)$", ErrorMessage = "保留天数必须大于0天并且小于365天")]
        [NotifyDataErrorInfo]
        private int keepDays = 30;

        [ObservableProperty]
        private TimeSpan inactiveTimeOut = TimeSpan.FromMinutes(5);

    }
}
