using CommunityToolkit.Mvvm.Input;
using DataReceiver.Models.Config;
using DataReceiver.Models.Socket;
using DataReceiver.Services.Decorator;
using System.ComponentModel;
using System.Text;

namespace DataReceiver.ViewModels.Communication
{
    public partial class TcpViewModel : ConnectionViewModelBase
    {
        private TcpClientModel Model { get; set; }

        public TcpConfig Config { get; }

        public TcpViewModel(TcpClientModel m) : base(m.Runtimes)
        {
            Model = m;
            Title = "TCP Client" + count;
            Config = Model.Config;
            
            base.Subscribe(Model);

            decorator = new DecoratorBase(Model);
        }

        [RelayCommand(CanExecute = nameof(IsCanConnect))]
        public override async Task ConnectAsync()
        {
            if (Config.EnableReconnect)
                decorator = new ReconnectDecorator(Model, Config.ReconnectDelay);

            //if (Config.HeartBeatConfig.Enable)
            //decorator = new HeartBeatDecorator(modelm, Config.HeartBeatConfig.HeartbeatInterval);

            await decorator.ConnectAsync();
        }

        [RelayCommand(CanExecute = nameof(IsCanDisconnect))]
        public override async Task DisconnectAsync()
        {
            await decorator.DisconnectAsync();
        }

        [RelayCommand(CanExecute = nameof(IsCanDisconnect))]
        public override Task SendAsync()
        {
            //Encoding test = Encoding.UTF8;
            var data = Encoding.UTF8.GetBytes(Config.SendMessage);
            return decorator.SendAsync(data);
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
            //Runtimes.PropertyChanged -= OnRuntimesPropertyChanged;
            Model.Dispose();
            base.Dispose();
        }
    }
}
