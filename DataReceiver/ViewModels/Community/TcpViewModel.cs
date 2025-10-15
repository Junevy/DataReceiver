using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataReceiver.Models.CommunicationCommon;
using DataReceiver.Models.Config;
using DataReceiver.Models.Socket;
using System.ComponentModel;

namespace DataReceiver.ViewModels.Community
{
    public partial class TcpViewModel : SubViewModelBase
    {
        //[ObservableProperty]
        //[NotifyPropertyChangedFor(nameof(CanExecute))]

        private readonly TcpClientModel model;

        public TcpConfig Config { get; }

        [ObservableProperty]
        private string socketMessage = string.Empty;


        public TcpViewModel(TcpClientModel m)
        {
            model = m;
            Title = "TCP Client" + count;
            Config = model.Config;
            Config.PropertyChanged += OnConfigPropertyChanged;
            base.Subscribe(model);
        }

        public override void ReceivedData(DataEventArgs<byte> args)
        {

        }

        [RelayCommand]
        public override async Task ConnectAsync()
        {
            await model.ConnectAsync();
        }

        [RelayCommand]
        public override Task SendAsync(string message)
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        public override void Disconnect()
        {
        }

        private void OnConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(Config.Reconnecting)))
            {
                ConnectCommand.NotifyCanExecuteChanged();
            }
        }

        /// <summary>
        /// 释放Model及ViewModel资源
        /// </summary>
        public override void Dispose()
        {
            Config.PropertyChanged -= OnConfigPropertyChanged;
        }
    }
}
