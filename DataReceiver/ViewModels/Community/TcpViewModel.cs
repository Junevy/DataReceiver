using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataReceiver.Models;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Navigation;

namespace DataReceiver.ViewModels.Community
{
    public partial class TcpViewModel : SubViewModelBase
    {
        [ObservableProperty]
        private TcpClientModel model;

        #region Properties
        [ObservableProperty]
        private string ip = "192.168.31.163";

        [ObservableProperty]
        private string port = "9007";

        [ObservableProperty]
        private bool isConnected = false;

        [ObservableProperty]
        private bool isAutoConnect = true;

        [ObservableProperty]
        private string heartBeat = "PING";

        [ObservableProperty]
        private bool isHeartBeat = false;

        [ObservableProperty]
        public string sendMessage = string.Empty;

        private bool CanStop => Model is not null && Model.IsConnected;
        private bool CanStart => Model is not null && !Model.IsConnected;
        #endregion

        public TcpViewModel()
        {
            //Client = new();
            int.TryParse(Port, out int parsedPort);

            Model = new()
            {
                Ip = Ip,
                Port = parsedPort,
                IsAutoConnect = IsAutoConnect,
                HeartBeat = HeartBeat,
                IsHeartBeat = IsHeartBeat,
            };
            Title = Ip + ":" + Port;
        }

        [RelayCommand(CanExecute = nameof(CanStart))]
        public override void Start()
        {
            //_ = Model.StartAsync();

            Task.Run(async () => await Model.StartAsync());

            Model.OnReceivedMessage -= OnReceivedMessage;
            Model.OnReceivedMessage += OnReceivedMessage;
            Model.OnStateChanged -= OnStateChanged;
            Model.OnStateChanged += OnStateChanged;
        }

        [RelayCommand(CanExecute = nameof(CanStop))]
        public override void Stop()
        {
            Model.Stop();
        }

        [RelayCommand(CanExecute = nameof(CanStop))]
        public override void Send()
        {
            _ = Model.SendAsync(SendMessage);
        }

        public override void Receive()
        {
        }

        public override void ReStart()
        {
            Model.ReStart();
        }

        [RelayCommand]
        public void Closed()
        {
            MessageBox.Show("TCP Closed");
        }

        #region 属性变化时同步到Model

        public void OnReceivedMessage(string message)
        {
            ReceivedMessages.Add(message);
        }

        public void OnStateChanged(bool state)
        {
            IsConnected = state;
        }

        partial void OnIpChanged(string value)
        {
            Model.Ip = value;
            StartCommand.NotifyCanExecuteChanged();
        }

        partial void OnPortChanged(string value)
        {
            int.TryParse(Port, out int parsedPort);
            Model.Port = parsedPort;
            StartCommand.NotifyCanExecuteChanged();
        }

        partial void OnIsConnectedChanged(bool value)
        {
            StartCommand.NotifyCanExecuteChanged();
            StopCommand.NotifyCanExecuteChanged();
            SendCommand.NotifyCanExecuteChanged();
        }

        partial void OnIsHeartBeatChanged(bool value)
        {
            Model.IsHeartBeat = value;
            if(value) _ = Model.StartHeartBeatAsync();
        }

        partial void OnHeartBeatChanged(string value)
        {
            model.HeartBeat = value;
        }

        partial void OnIsAutoConnectChanged(bool value)
        {
            model.IsAutoConnect = value;
            if(value) _ = Model.StartAutoConnectAsync();
        }

        partial void OnSendMessageChanged(string value)
        {
        }
        #endregion
    }
}
