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
            ReconnectConfig = reconnectConfig;
            HeartBeatConfig = heartBeatConfig;
            Title = "TCP Client" + GetNextId();
            base.Subscribe(Model);
        }

        [RelayCommand(CanExecute = nameof(IsCanConnect))]
        public override async Task ConnectAsync()
        {

            IConnection decorator = DecoratorFactory.CreateReconncetDecorator(Model, ReconnectConfig);
            decorator = DecoratorFactory.CreateHeartBeatDecorator(decorator, HeartBeatConfig);

            var test = decorator.Describe();
            Console.WriteLine(test);

            //await decorator.ConnectAsync();
            //await decorator.TryStartHeartBeat();
        }

        [RelayCommand(CanExecute = nameof(IsCanDisconnect))]
        public override async Task DisconnectAsync()
        {
            await Decorator.DisconnectAsync();
        }

        [RelayCommand(CanExecute = nameof(IsCanDisconnect))]
        public override Task SendAsync()
        {
            //Encoding test = Encoding.UTF8;
            var data = Encoding.UTF8.GetBytes(Config.SendMessage);
            return Decorator.SendAsync(data);
        }

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
            Model.Dispose();
            base.Dispose();
        }

        public Task StartReconnectAsync()
        {
            throw new NotImplementedException();
        }

        public void StopReconnect()
        {
            throw new NotImplementedException();
        }

        public Task StartHeartBeatAsync(byte[] response)
        {
            throw new NotImplementedException();
        }

        public void StopHeartBeat()
        {
            throw new NotImplementedException();
        }
    }
}
