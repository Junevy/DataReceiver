using CommunityToolkit.Mvvm.Input;
using DataReceiver.Models.Socket.Config;
using DataReceiver.Models.Socket.FTP;
using Services.TaskSchedule;
using System.ComponentModel;

namespace DataReceiver.ViewModels.Communication
{
    public partial class FtpServerViewModel : ConnectionViewModelBase
    {
        private FtpServerModel Model { get; }
        public TaskScheduleConfig TaskScheduleConfig { get; }
        public FtpServerConfig Config => Model.Config;

        public FtpServerViewModel(FtpServerModel model, TaskScheduleConfig taskScheduleConfig) : base(model.Runtimes)
        {
            Model = model;
            TaskScheduleConfig = taskScheduleConfig;
            SubscribeState(Model);

            TaskScheduleConfig.ExePath 
                = "D:\\Desktop\\WorkSpace\\WPF\\DataReceiver" +
                "\\RegularCleanupTask\\bin\\Debug\\net472\\RegularCleanupTask.exe";
            TaskScheduleConfig.TaskName = "RegularCleanupTask";
            TaskScheduleConfig.Description = "Data Receiver Regular Cleanup Task";
            TaskScheduleConfig.OnStateChanged += v =>
            {
                    if (v) RegisterTask();
                    else UnregisterTask();
            };
        }

        [RelayCommand(CanExecute = nameof(IsCanConnect))]
        public override async Task ConnectAsync()
        {
            await Model.ConnectAsync();
        }

        //[RelayCommand(CanExecute = nameof(IsCanDisconnect))]
        [RelayCommand(CanExecute = nameof(IsCanDisconnect))]
        public override async Task DisconnectAsync()
        {
            await Model.DisconnectAsync();
        }

        [RelayCommand]
        public void RegisterTask()
        {
            TaskScheduleService.RegisterTask(
                TaskScheduleConfig.Description,
                TaskScheduleConfig.ExePath,
                TaskScheduleConfig.IntervalDays,
                TaskScheduleConfig.TaskName);
        }

        [RelayCommand]
        public void UnregisterTask()
        {
            TaskScheduleService.UnregisterTask(TaskScheduleConfig.TaskName);
        }

        public override Task SendAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 当Runtimes 参数发生变化时，通知命令更新其状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnRuntimesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Runtimes.State))
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    ConnectCommand.NotifyCanExecuteChanged();
                    DisconnectCommand.NotifyCanExecuteChanged();
                    //SendCommand.NotifyCanExecuteChanged();
                });
            }
        }

        //public void OnTask

        public override void Dispose()
        {
            Model.Dispose();
            base.Dispose();
        }
    }
}
