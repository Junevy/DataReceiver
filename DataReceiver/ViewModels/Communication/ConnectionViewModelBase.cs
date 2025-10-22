using CommunityToolkit.Mvvm.ComponentModel;
using DataReceiver.Models.Common;
using DataReceiver.Models.Socket.Base;
using DataReceiver.Models.Socket.Common;
using DataReceiver.ViewModels.Base;
using log4net;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Markup;

namespace DataReceiver.ViewModels.Communication
{
    /// <summary>
    /// 所有Socket的基类，自动 Binding 至 tabitem 的 DataContext
    /// </summary>
    public abstract partial class ConnectionViewModelBase : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConnectionViewModelBase));
        private readonly Object stateLock = new();
        private static int count = 1;
        /// <summary>
        /// Tab item 的 title自增序号
        /// </summary>
        /// <returns> 自增的序号 </returns>
        protected static int GetNextId() => Interlocked.Increment(ref count);

        protected readonly CompositeDisposable disposables = [];  // 统一管理 Observer 订阅生命周期

        /// <summary>
        /// Tabcontrol item的Header
        /// </summary>
        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private string sendMessage = string.Empty;

        /// <summary>
        /// Socket的运行状态
        /// </summary>
        public ConnectionRuntimes Runtimes { get; }

        /// <summary>
        /// 最多显示消息的条数
        /// </summary>
        protected const int MAXCOLLECTIONSIZE = 200;

        /// <summary>
        /// 存储Socket消息的列表，可Binding到UI，最多消息数量 MAXCOLLECTIONSIZE
        /// </summary>
        public ObservableCollection<string> ReceivedDataCollection { get; set; } = [];

        protected bool IsCanConnect => Runtimes.State == ConnectionState.Disconnected;
        protected bool IsCanDisconnect => Runtimes.State == ConnectionState.Connected
                                       || Runtimes.State == ConnectionState.Connecting;

        public ConnectionViewModelBase(ConnectionRuntimes runtimes)
        {
            Title = title ?? "Page" + GetNextId();
            Runtimes = runtimes;
            Runtimes.PropertyChanged -= OnRuntimesPropertyChanged;
            Runtimes.PropertyChanged += OnRuntimesPropertyChanged;
        }

        public virtual void SubscribeState(ReactiveConnectionBase subscriber)
        {
            subscriber.DataReceived.ObserveOn(SynchronizationContext.Current)
                .Subscribe(data =>
                {
                    var msg = Encoding.UTF8.GetString(data.Data.Span.ToArray());

                    Log.Info($"Date received : {msg}");
                    if (ReceivedDataCollection.Count > MAXCOLLECTIONSIZE)
                        ReceivedDataCollection.RemoveAt(0);
                    ReceivedDataCollection.Add(msg ?? "Empty");
                }).DisposeWith(disposables);
        }

        public virtual void SubscribeData(ReactiveConnectionBase subscriber)
        {
            subscriber.StateChanged.ObserveOn(SynchronizationContext.Current)
                .Subscribe(e =>
                {
                    Log.Info($"State changed {Runtimes.State} to : {e.NewState}");

                    Runtimes.State = e.NewState;

                }).DisposeWith(disposables);
        }

        /// <summary>
        /// Runtimes内Property更改时的回调函数，用于刷新UI Socket的状态。
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e" ref="ConnectionRuntimes"> ConnectionRuntimes </param>
        public abstract void OnRuntimesPropertyChanged(object sender, PropertyChangedEventArgs e);
        public abstract Task ConnectAsync();
        public abstract Task SendAsync();
        public abstract Task DisconnectAsync();
        public override void Dispose()
        {
            disposables.Dispose();
            Runtimes.PropertyChanged -= OnRuntimesPropertyChanged;
            GC.SuppressFinalize(this);
        }
    }
}
