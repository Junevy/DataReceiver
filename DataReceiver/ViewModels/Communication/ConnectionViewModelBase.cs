using CommunityToolkit.Mvvm.ComponentModel;
using DataReceiver.Models.Common;
using DataReceiver.Models.Socket.Base;
using DataReceiver.Models.Socket.Interface;
using DataReceiver.ViewModels.Base;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace DataReceiver.ViewModels.Communication
{
    /// <summary>
    /// 所有Socket的基类，用于TabControl的Item绑定
    /// </summary>
    public abstract partial class ConnectionViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Tab item 的 title自增序号
        /// </summary>
        /// <returns></returns>
        protected static int count = 1;
        protected IConnection decorator;
        protected static int GetNextId() => Interlocked.Increment(ref count);
        private readonly CompositeDisposable disposables = [];  // 统一管理 Observer 订阅生命周期

        [ObservableProperty]
        public string title = string.Empty;

        /// <summary>
        /// Socket的运行状态
        /// </summary>
        public ConnectionRuntimes Runtimes { get; }

        /// <summary>
        /// 接收Socket数据的列表，用于Binding到UI
        /// </summary>
        public ObservableCollection<string> ReceivedDataCollection { get; set; } = [];

        protected bool IsCanConnect => Runtimes.State != ConnectionState.Connected;
        protected bool IsCanDisconnect => Runtimes.State == ConnectionState.Connected;

        protected ConnectionViewModelBase(ConnectionRuntimes runtimes)
        {
            Title = title ?? "Page" + GetNextId();
            Runtimes = runtimes;

            Runtimes.PropertyChanged -= OnRuntimesPropertyChanged;
            Runtimes.PropertyChanged += OnRuntimesPropertyChanged;
        }

        public virtual void Subscribe(ConnectionReactiveBase subscriber)
        {
            // 无法执行回调方法，线程问题？并不是线程问题
            subscriber.DataReceived.ObserveOn(SynchronizationContext.Current)
                .Subscribe(data =>
                {
                    ReceivedDataCollection.Add(data.Message ?? "Empty");
                }).DisposeWith(disposables);

            subscriber.StateChanged.ObserveOn(SynchronizationContext.Current)
                .Subscribe(e =>
                {
                    Runtimes.State = e.newState;
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
