using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataReceiver.ViewModels.Base;
using HandyControl.Controls;
using HandyControl.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace DataReceiver.ViewModels.Community
{
    public partial class CommunityViewModel : ViewModelBase
    {
        /// <summary>
        /// 用于ViewModel映射View，显示在TableControl中
        /// </summary>
        public ObservableCollection<SubViewModelBase> VMList { get; set; } = [];

        //当前选中的侧边栏Item Name
        public string currentItem = string.Empty;
        // 限制最大的Tab数量为2
        private bool CanAdd => VMList.Count < 2;

        [RelayCommand]
        public void GetCurrentItem(object? value)
        {
            var item = (value as FunctionEventArgs<object>)?.Info as SideMenuItem;
            if (item is null) return;
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
            var tab = (value as CancelRoutedEventArgs)?.OriginalSource as SubViewModelBase;
            VMList.Remove(tab!);
        }
    }
}


