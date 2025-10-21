using CommunityToolkit.Mvvm.Input;
using DataReceiver.Models.Config;
using DataReceiver.Models.Socket;
using DataReceiver.Models.Socket.Config;
using DataReceiver.Models.Socket.Interface;
using DataReceiver.Services.Extentions;
using DataReceiver.Services.Factory;
using System.ComponentModel;
using System.Text;

namespace DataReceiver.ViewModels.Communication
{
    public partial class TcpViewModel : ConnectionViewModelBase
    {
        private IReactiveConnection Decorator { get; set; }
        private TcpClientModel Model { get; set; }
        public TcpConfig Config => Model.Config;
        public ReconnectConfig ReconnectConfig { get; }
        public HeartBeatConfig HeartBeatConfig { get; }

        public TcpViewModel(TcpClientModel model,
                            ReconnectConfig reconnectConfig,
                            HeartBeatConfig heartBeatConfig)
                            : base(model.Runtimes)
        {
            Model = model;
            Decorator = model;
            ReconnectConfig = reconnectConfig;
            HeartBeatConfig = heartBeatConfig;
            Title = "TCP Client" + GetNextId();
            base.Subscribe(Model);
        }

        [RelayCommand(CanExecute = nameof(IsCanConnect))]
        public override async Task ConnectAsync()
        {
            Decorator = DecoratorFactory.CreateReconncetDecorator(Model, ReconnectConfig);
            Decorator = DecoratorFactory.CreateHeartBeatDecorator(Decorator, HeartBeatConfig);

            await Decorator.ConnectAsync();
            var response = Encoding.UTF8.GetBytes(HeartBeatConfig.Response);
            _ = Decorator.TryReconnect();
            _ = Decorator.TryStartHeartBeat(response);

            Title = $"[{Config.Ip} : {Config.Port}]";
        }

        [RelayCommand(CanExecute = nameof(IsCanDisconnect))]
        public override async Task DisconnectAsync()
        {
            await Decorator.DisconnectAsync();
        }

        [RelayCommand(CanExecute = nameof(IsCanDisconnect))]
        public override async Task SendAsync()
        {
            var data = Encoding.UTF8.GetBytes(SendMessage);
            await Decorator.SendAsync(data);
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
                    SendCommand.NotifyCanExecuteChanged();
                });
            }
        }

        /// <summary>
        /// 释放Model及ViewModel资源
        /// </summary>
        public override void Dispose()
        {
            Decorator.Dispose();
            base.Dispose();
        }
    }
}
