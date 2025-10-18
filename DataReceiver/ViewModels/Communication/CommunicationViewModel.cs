using CommunityToolkit.Mvvm.Input;
using DataReceiver.Models.Socket;
using DataReceiver.ViewModels.Base;
using HandyControl.Controls;
using HandyControl.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace DataReceiver.ViewModels.Communication
{
    public partial class CommunicationViewModel : ViewModelBase
    {
        /// <summary>
        /// 用于ViewModel映射View，显示在TableControl中
        /// </summary>
        public ObservableCollection<ConnectionViewModelBase> VMList { get; set; } = [];

        //当前选中的侧边栏Item Name
        public string currentItem = string.Empty;
        // 限制最大的Tab数量为2
        private bool CanAdd => VMList.Count < 2;
        //private bool CanExecute => !Config.Reconnecting;

        [RelayCommand]
        public void GetCurrentItem(object? value)
        {
            if ((value as FunctionEventArgs<object>)?.Info is not SideMenuItem item) return;
            currentItem = item.Name.ToString() ?? string.Empty;
        }

        [RelayCommand(CanExecute = nameof(CanAdd))]
        public void AddSubPage()
        {
            if (currentItem is null) return;

            switch (currentItem)
            {
                case "TCP":
                    VMList.Add(App.Current.Services.GetRequiredService<TcpViewModel>());
                    break;
                case "FTP":
                    VMList.Add(App.Current.Services.GetRequiredService<FtpViewModel>());
                    break;
                case "UDP":
                    VMList.Add(App.Current.Services.GetRequiredService<TcpViewModel>());
                    break;
                default:
                    currentItem = string.Empty;
                    return;
            }
            Growl.Info($"{currentItem} added!");
            currentItem = string.Empty;
            AddSubPageCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        public void Closing(object? value)
        {
            var tab = (value as CancelRoutedEventArgs)?.OriginalSource as ConnectionViewModelBase;
            //if (tab is TcpViewModel tempModel || tab is FtpViewModel tempModel) tempModel.Dispose();
            tab?.Dispose();
            VMList.Remove(tab!);
        }

        public override void Dispose()
        {
            VMList.Clear();
            //VMList = null;
        }
    }
}


