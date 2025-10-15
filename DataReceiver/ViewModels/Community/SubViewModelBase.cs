using CommunityToolkit.Mvvm.ComponentModel;
using DataReceiver.Models.Common;
using DataReceiver.Models.CommunicationCommon;
using DataReceiver.Models.Socket.Base;
using DataReceiver.ViewModels.Base;
using System.Collections.ObjectModel;

namespace DataReceiver.ViewModels.Community
{
    /// <summary>
    /// 所有Socket的基类，用于TabControl的Item绑定
    /// </summary>
    public abstract partial class SubViewModelBase : ViewModelBase
    {
        protected static int count = 0;
        //protected SocketBase Model { get; set; } = default!;

        [ObservableProperty]
        public string title = string.Empty;

        [ObservableProperty]
        private bool state = false;

        [ObservableProperty]
        private bool isConnected = false;

        [ObservableProperty]
        private bool isEditAble = true;

        public ObservableCollection<string> ReceivedDataCollection { get; set; } = [];

        protected SubViewModelBase()
        {
            count++;
            Title = title ?? "Page" + count;
        }

        public virtual void Subscribe(SocketBase model)
        {
            model.DataReceived.Subscribe(data =>
            {
                ReceivedDataCollection.Add(data.Message ?? "Empty");
            });

            model.StateChanged.Subscribe(e =>
            {
                if (e.newState == ConnectionState.Connected)
                {
                    State = true;
                }
                else
                    State = false;
            });
        }

        public abstract Task ConnectAsync();

        public abstract Task SendAsync(string message);

        public abstract void ReceivedData(DataEventArgs<byte> args);

        public abstract void Disconnect();

        public abstract override void Dispose();
    }
}
