using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataReceiver.Services.Validation;
using System.Windows;

namespace DataReceiver.ViewModels.Community
{
    public partial class TcpViewModel : SubViewModelBase
    {
        [ObservableProperty]
        private string ip = string.Empty;

        [ObservableProperty]
        private string port = string.Empty;

        [ObservableProperty]
        private bool isHeartBeat = false;

        [ObservableProperty]
        private bool isAutoConnect = true;  

        private bool CanButtonExecute => !string.IsNullOrWhiteSpace(Ip)
            && !string.IsNullOrWhiteSpace(Port);

        [RelayCommand(CanExecute = nameof(CanButtonExecute))]
        public void Start()
        {
            MessageBox.Show($"Start TCP Server at {Ip}:{Port}");
        }

        partial void OnIpChanged(string value)
        {
            StartCommand.NotifyCanExecuteChanged();
        }

        partial void OnPortChanged(string value)
        {
            StartCommand.NotifyCanExecuteChanged();
        }
    }
}
