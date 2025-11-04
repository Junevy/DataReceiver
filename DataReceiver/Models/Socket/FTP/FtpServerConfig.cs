using CommunityToolkit.Mvvm.ComponentModel;
using DataReceiver.Models.Config;

namespace DataReceiver.Models.Socket.Config
{
    public partial class FtpServerConfig : CommunicationConfig
    {
        [ObservableProperty]
        private string rootPath = @$"D:/FTP/Default/{DateTime.Now:yyyy-MM-dd}";

        [ObservableProperty]
        private string userName = "admin";

        [ObservableProperty]
        private string password = "123456";

        [ObservableProperty]
        private int maxConnections = 30;

        [ObservableProperty]
        private bool allowAnonymous = true;

        [ObservableProperty]
        private TimeSpan inactiveCheckInterval = TimeSpan.FromMinutes(1);

        [ObservableProperty]
        private bool enableAutoDelete = false;

        [ObservableProperty]
        private int keepDays = 30;

        [ObservableProperty]
        private TimeSpan inactiveTimeOut = TimeSpan.FromMinutes(5);

    }
}
