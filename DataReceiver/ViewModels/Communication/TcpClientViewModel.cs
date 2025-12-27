using CommunityToolkit.Mvvm.Input;
using DataReceiver.Models.Config;
using DataReceiver.Models.Socket.Config;
using DataReceiver.Models.Socket.Interface;
using DataReceiver.Models.Socket.TCP;
using DataReceiver.Services.Extentions;
using DataReceiver.Services.Factory;
using log4net;
using System.ComponentModel;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Text;

namespace DataReceiver.ViewModels.Communication
{
    public partial class TcpClientViewModel : ConnectionViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TcpClientViewModel));

        private IConnection Decorator { get; set; }
        private TcpClientModel Model { get; set; }
        public TcpClientConfig Config => Model.Config;
        public ReconnectConfig ReconnectConfig { get; }
        public HeartBeatConfig HeartBeatConfig { get; }

        public TcpClientViewModel(TcpClientModel model,
            ReconnectConfig reconnectConfig, HeartBeatConfig heartBeatConfig)
            : base(model.Runtimes)
        {
            Model = model;
            Decorator = model;
            ReconnectConfig = reconnectConfig;
            HeartBeatConfig = heartBeatConfig;
            Title = "TCP Client" + GetNextId();
            SubscribeState(Model);
            SubscribeData(Model);
        }

        [RelayCommand(CanExecute = nameof(IsCanConnect))]
        public override async Task ConnectAsync()
        {
            Log.Info($"[{Config.Ip}:{Config.Port}]: Waiting for connect.");
            Decorator = DecoratorFactory.CreateReconncetDecorator(Model, ReconnectConfig);
            Decorator = DecoratorFactory.CreateHeartBeatDecorator(Decorator, HeartBeatConfig);

            await Decorator.ConnectAsync();
            Task.Run(async () => { await Decorator.TryReconnect(); });

            var response = Encoding.UTF8.GetBytes(HeartBeatConfig.Response);
            //await Decorator.TryStartHeartBeat(response);
            Task.Run(async () => { await Decorator.TryStartHeartBeat(response); });


            Title = $"[{Config.Ip} : {Config.Port}]";
        }

        [RelayCommand(CanExecute = nameof(IsCanDisconnect))]
        public override async Task DisconnectAsync()
        {
            Log.Info($"[{Config.Ip}:{Config.Port}]: Waiting for disconnect.");
            await Decorator.DisconnectAsync();
        }

        [RelayCommand(CanExecute = nameof(IsCanDisconnect))]
        public override async Task SendAsync()
        {
            var data = Encoding.UTF8.GetBytes(SendMessage);
            Log.Info($"[{Config.Ip}:{Config.Port}]: Send data: {data}");
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

        public override void SubscribeData(IReactiveCapable subscriber)
        {
            subscriber.DataObservable.ObserveOn(SynchronizationContext.Current)
                .Subscribe(data =>
                {
                    var msg = Encoding.UTF8.GetString(data.Data.Span.ToArray());
                    var info = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss:fff")
                              + $"  [{Config.Ip}:{Config.Port}] :  "
                              + msg;
                    Log.Info($"Date received : {msg}");

                    // 处理心跳逻辑
                    if (HeartBeatConfig.Request.Equals(msg))
                    {
                        Runtimes.LastHeartBeatTime = DateTime.Now;
                        return;
                    }

                    if (ReceivedDataCollection.Count > MAXCOLLECTIONSIZE)
                        ReceivedDataCollection.RemoveAt(0);
                    ReceivedDataCollection.Add(info ?? "Empty");
                }).DisposeWith(disposables);
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
