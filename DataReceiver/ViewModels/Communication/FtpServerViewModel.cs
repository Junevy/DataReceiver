using CommunityToolkit.Mvvm.Input;
using DataReceiver.Models.Socket.Config;
using DataReceiver.Models.Socket.FTP;
using System.ComponentModel;

namespace DataReceiver.ViewModels.Communication
{
    public partial class FtpServerViewModel(FtpServerModel model, TaskScheduleConfig taskScheduleConfig)
        : ConnectionViewModelBase(model.Runtimes)
    {
        private FtpServerModel Model { get; } = model;
        public TaskScheduleConfig TaskScheduleConfig { get; } = taskScheduleConfig;

        [RelayCommand(CanExecute = nameof(IsCanConnect))]
        public override async Task ConnectAsync()
        {
            await Model.ConnectAsync();
        }

        //[RelayCommand(CanExecute = nameof(IsCanDisconnect))]
        [RelayCommand]
        public override async Task DisconnectAsync()
        {
            await Model.DisconnectAsync();
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

        public override void Dispose()
        {
            Model.Dispose();
            base.Dispose();
        }
    }
}
