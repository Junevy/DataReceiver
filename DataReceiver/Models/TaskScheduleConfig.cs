
using CommunityToolkit.Mvvm.ComponentModel;

namespace DataReceiver.Models.Socket.Config
{
    public partial class TaskScheduleConfig : ObservableValidator
    {
        [ObservableProperty]
        private bool isEnable = false;

        [ObservableProperty]
        private string exePath = string.Empty;

        [ObservableProperty]
        private string arguments = string.Empty;

        [ObservableProperty]
        private string workingDirectory = string.Empty;

        [ObservableProperty]
        private string taskName = "ScheduleCleanTask";

        [ObservableProperty]
        private string description = "Data Receiver Scheduled Task";

        [ObservableProperty]
        private string author = "DataReceiverApp";

        [ObservableProperty]
        private short intervalDays = 1;

        [ObservableProperty]
        private DateTime startBoundary = DateTime.Now + TimeSpan.FromHours(2); // 默认每天凌晨2点执行

    }
}
