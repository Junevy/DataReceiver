
using CommunityToolkit.Mvvm.ComponentModel;

namespace DataReceiver.Models.Socket.Config
{
    public partial class TaskScheduleConfig : ObservableValidator
    {
        public event Action<bool>? OnStateChanged;

        [ObservableProperty]
        private bool isEdited = false;

        [ObservableProperty]
        private bool isEnable = false;

        //[ObservableObject]
        public bool CanEdit => !IsEnable;

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
        private TimeSpan startBoundary = TimeSpan.FromHours(4); // 默认每天凌晨2点执行

        partial void OnIsEnableChanged(bool value) => OnStateChanged?.Invoke(value);
    }
}
