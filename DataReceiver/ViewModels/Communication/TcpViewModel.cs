using CommunityToolkit.Mvvm.Input;
using DataReceiver.Models.Config;
using DataReceiver.Models.Socket;
using System.ComponentModel;

namespace DataReceiver.ViewModels.Communication
{
    public partial class TcpViewModel : ConnectionViewModelBase
    {
        private readonly TcpClientModel model;
        public TcpConfig Config { get; }

        public TcpViewModel(TcpClientModel m) : base(m.Runtimes)
        {
            model = m;
            Title = "TCP Client" + count;
            Config = model.Config;
            base.Subscribe(model);
        }

        [RelayCommand(CanExecute = nameof(IsCanConnect))]
        public override async Task ConnectAsync()
        {
            await model.ConnectAsync();
        }

        [RelayCommand(CanExecute = nameof(IsCanDisconnect))]
        public override async Task DisconnectAsync()
        {
            await model.DisconnectAsync();
        }

        [RelayCommand]
        public override Task SendAsync(string message)
        {
            throw new NotImplementedException();
        }

        public override void OnRuntimesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Runtimes.State))
            {
                ConnectCommand.NotifyCanExecuteChanged();
                DisconnectCommand.NotifyCanExecuteChanged();
            }
        }

        /// <summary>
        /// 释放Model及ViewModel资源
        /// </summary>
        public override void Dispose()
        {
            Runtimes.PropertyChanged -= OnRuntimesPropertyChanged;
        }
    }
}
