
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataReceiver.Models.Socket.Config
{
    public partial class TaskScheduleConfig : ObservableValidator
    {
        public event Action<bool>? OnStateChanged;

        //[ObservableProperty]
        //private bool isEdited = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanEdit))] // Notify the CanEdit when IsEnable changed.
        private bool isEnable = false;

        /// <summary>
        /// Expresstion some textbox can edit when IsEnable is false 
        /// </summary>
        public bool CanEdit => !IsEnable;

        [ObservableProperty]
        [Required (ErrorMessage = "定时执行程序路径不能为空！")]
        [NotifyDataErrorInfo]
        private string exePath = "D:/DataReceiver/null.exe";

        [ObservableProperty]
        private string arguments = string.Empty;

        //[ObservableProperty]
        //private string workingDirectory = string.Empty;

        [ObservableProperty]
        [Required(ErrorMessage = "任务计划的名称不能为空！")]
        [NotifyDataErrorInfo]
        private string taskName = "ScheduleCleanTask";

        [ObservableProperty]
        private string description = "Data Receiver Scheduled Task";

        [ObservableProperty]
        [Required(ErrorMessage = "任务计划的作者不能为空！")]
        [NotifyDataErrorInfo]
        private string author = "DataReceiverApp";

        /// <summary>
        /// Gets or sets the number of days between scheduled task executions.
        /// </summary>
        /// <remarks>The value must be greater than 0 and less than 23. Validation attributes ensure that
        /// the property cannot be set to an invalid value.</remarks>
        [ObservableProperty]
        [Required(ErrorMessage = "任务计划的执行间隔天数不能为空！")]
        [RegularExpression(@"^(?:0?[1-9]|1\d|2[0-3])$", ErrorMessage = "间隔天数必须大于0天并且小于23天")]
        [NotifyDataErrorInfo]
        private short intervalDays = 1;

        /// <summary>
        /// Execution start time boundary
        /// </summary>
        [ObservableProperty]
        private TimeSpan startBoundary = TimeSpan.FromHours(4); // 默认每天凌晨2点执行

        partial void OnIsEnableChanged(bool value) => OnStateChanged?.Invoke(value);
    }
}
