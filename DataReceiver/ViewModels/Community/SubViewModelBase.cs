using CommunityToolkit.Mvvm.ComponentModel;
using DataReceiver.Services.Interface;
using DataReceiver.ViewModels.Base;
using System.Collections.ObjectModel;

namespace DataReceiver.ViewModels.Community
{
    /// <summary>
    /// 所有Socket的基类，用于TabControl的Item绑定
    /// </summary>
    public abstract partial class SubViewModelBase : ViewModelBase
    {
        public static int count = 0;

        [ObservableProperty]
        public string title = string.Empty;

        [ObservableProperty]
        public ObservableCollection<string> receivedMessages = [];

        protected SubViewModelBase()
        {
            count++;
            Title = title ?? "Page" + count;
        }

        public abstract void Start();

        public abstract void Stop();

        public abstract void Send();

        public abstract void Receive();

        public abstract void ReStart();
    }
}
