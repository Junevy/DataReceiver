using CommunityToolkit.Mvvm.Input;
using DataReceiver.Models.Socket.Config;
using DataReceiver.Models.Socket.FTP;
using Services.Config;
using Services.Dialog;
using Services.TaskSchedule;
using System.ComponentModel;

namespace DataReceiver.ViewModels.Communication
{
    public partial class FtpServerViewModel : ConnectionViewModelBase
    {
        private FtpServerModel Model { get; }
        public TaskScheduleConfig TaskScheduleConfig { get; }
        public FtpServerConfig Config => Model.Config;
        private readonly IDialogService dialog;

        public FtpServerViewModel(FtpServerModel model, 
            TaskScheduleConfig taskScheduleConfig, IDialogService dialogService) : base(model.Runtimes)
        {
            Model = model;
            TaskScheduleConfig = taskScheduleConfig;
            SubscribeState(Model);
            dialog = dialogService;
            TaskScheduleConfig.OnStateChanged += v =>
            {
                    if (v) RegisterTask();
                    else UnregisterTask();
            };
            TaskScheduleConfig.PropertyChanged += OnTaskPropertyChanged;
        }

        [RelayCommand(CanExecute = nameof(IsCanConnect))]
        public override async Task ConnectAsync()
            => await Model.ConnectAsync();

        //[RelayCommand(CanExecute = nameof(IsCanDisconnect))]
        [RelayCommand(CanExecute = nameof(IsCanDisconnect))]
        public override async Task DisconnectAsync()
            => await Model.DisconnectAsync();

        /// <summary>
        /// 注册定时任务
        /// </summary>
        [RelayCommand]
        public void RegisterTask()
        {
            TaskScheduleService.RegisterTask(
                TaskScheduleConfig.Description,
                TaskScheduleConfig.ExePath,
                TaskScheduleConfig.IntervalDays,
                TaskScheduleConfig.TaskName);
        }

        /// <summary>
        /// 注销定时任务
        /// </summary>
        [RelayCommand]
        public void UnregisterTask() 
            => TaskScheduleService.UnregisterTask(TaskScheduleConfig.TaskName);

        /// <summary>
        /// 选择工作目录（存储FTP文件的目录及需要定时清理的目录）
        /// </summary>
        [RelayCommand]
        public void SelectFolder()
        {
            string? folder = dialog.SelectFolder();
            if (!string.IsNullOrEmpty(folder))
            {
                TaskScheduleConfig.WorkingDirectory = folder;
            }
        }

        /// <summary>
        /// 修改并保存配置文件
        /// </summary>
        [RelayCommand]
        public void SaveConfig()
            => ConfigService.SaveSection(TaskScheduleConfig, nameof(TaskScheduleConfig));

        public override Task SendAsync()
            => throw new NotImplementedException();

        /// <summary>
        /// 当Runtimes 参数（State）发生变化时，更新命令的可执行状态
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
                });
            }
        }

        public void OnTaskPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            TaskScheduleConfig.IsEdited = true;
        }

        /// <summary>
        /// 释放FTP Server对象
        /// </summary>
        public override void Dispose()
        {
            Model.Dispose();
            base.Dispose();
        }
    }
}
