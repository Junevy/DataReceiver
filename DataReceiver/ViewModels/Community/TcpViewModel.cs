using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataReceiver.Models;
using DataReceiver.Services.Interface;
using System.Windows;

namespace DataReceiver.ViewModels.Community
{
    public partial class TcpViewModel : SubViewModelBase
    {
        #region properties
        private readonly TcpClientModel model;

        [ObservableProperty]
        private string ip = string.Empty;

        [ObservableProperty]
        private string port = string.Empty;

        [ObservableProperty]
        private bool isConnected = false;

        [ObservableProperty]
        private bool isAutoConnect = true;

        [ObservableProperty]
        private string heartBeat = "PING";

        [ObservableProperty]
        private bool isHeartBeat = false;

        #endregion

        public TcpViewModel()
        {
            model = new();
            Title = "TCP Client" + "-" +  count;
        }

        private bool CanStartExecute => !string.IsNullOrWhiteSpace(Ip)
            && !string.IsNullOrWhiteSpace(Port) && !IsConnected;

        [RelayCommand(CanExecute = nameof(CanStartExecute))]
        public override void Start()
        {
            IsConnected = true;
            MessageBox.Show($"Start TCP Server at {Ip}:{Port}");
            ReceivedMessages.Add($"Connect Success : [{Ip}] : [{Port}]");
        }

        [RelayCommand(CanExecute = nameof(IsConnected))]
        public override void Stop()
        {
            IsConnected = false;
        }

        public override void Send()
        {
            throw new NotImplementedException();
        }

        public override void Receive()
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        public void Closed()
        {
            MessageBox.Show("TCP Closed");
        }

        #region 属性变化时同步到Model
        partial void OnIpChanged(string value)
        {
            model.Ip = value;
            StartCommand.NotifyCanExecuteChanged();
        }

        partial void OnPortChanged(string value)
        {
            model.Port = value;
            StartCommand.NotifyCanExecuteChanged();
        }

        partial void OnIsConnectedChanged(bool value)
        {
            model.IsConnected = value;
            StartCommand.NotifyCanExecuteChanged();
            StopCommand.NotifyCanExecuteChanged();
        }

        partial void OnIsHeartBeatChanged(bool value)
        {
            model.IsHeartBeat = value;
        }

        partial void OnHeartBeatChanged(string value)
        {
            model.HeartBeat = value;
        }

        partial void OnIsAutoConnectChanged(bool value)
        {
            model.IsAutoConnect = value;
        }
        #endregion
    }
}
