using CommunityToolkit.Mvvm.Input;
using DataReceiver.Models.Socket.Config;
using DataReceiver.Models.Socket.FTP;
using HandyControl.Controls;
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
                if (v) 
                {
                    RegisterTask();
                    Growl.Success($"Auto clean task registered.");
                }
                else
                {
                    UnregisterTask();
                    Growl.Warning($"Auto clean task canceled.");
                }
            };

            TaskScheduleConfig.PropertyChanged += OnConfigPropertyChanged;
            Config.PropertyChanged += OnConfigPropertyChanged;
            Title = "FTP Server - Stop";
        }

        [RelayCommand(CanExecute = nameof(IsCanConnect))]
        public override async Task ConnectAsync() 
        { 
            await Model.ConnectAsync();
            Title = "FTP Server - Running";
        }

        [RelayCommand(CanExecute = nameof(IsCanDisconnect))]
        public override async Task DisconnectAsync()
        {
            await Model.DisconnectAsync();
            Title = "FTP Server - Stop";
        }

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
                Config.RootPath = folder;
            }
        }

        /// <summary>
        /// 当 Config 的参数发生变化时，自动保存到本地Json文件中
        /// </summary>
        /// <param name="sender">参数发生变化的对象（Config）</param>
        /// <param name="e">发生变化的信息（含参数名）</param>
        private void OnConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender == TaskScheduleConfig)
            {
                SaveConfig(TaskScheduleConfig, nameof(TaskScheduleConfig));
            }
            else if (sender == Config)
            {
                SaveConfig(Config, nameof(FtpServerConfig));
            }
        }

        /// <summary>
        /// Config 参数发生变化时，即时保存到本地Json文件中
        /// </summary>
        /// <typeparam name="T">Config 对象类型</typeparam>
        /// <param name="config">Config 对象</param>
        /// <param name="sectionName">Config 名称（Section Name of Json）</param>
        public void SaveConfig<T>(T config, string sectionName)
            => ConfigService.SaveSection(config, sectionName);

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

        /// <summary>
        /// 释放FTP Server对象，释放后，本次程序的生命周期内不可再次启动（Bug，待修复）。
        /// </summary>
        public override void Dispose()
        {
            Config.PropertyChanged -= OnConfigPropertyChanged;
            TaskScheduleConfig.PropertyChanged -= OnConfigPropertyChanged;
            Model.Dispose();
            base.Dispose();
        }
    }
}
