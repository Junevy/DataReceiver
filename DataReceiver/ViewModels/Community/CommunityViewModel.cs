using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataReceiver.Services.Interface;
using DataReceiver.ViewModels.Base;
using HandyControl.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace DataReceiver.ViewModels.Community
{
    public partial class CommunityViewModel : ViewModelBase
    {
        /// <summary>
        /// 用于ViewModel映射View，显示在TableControl中
        /// </summary>
        public ObservableCollection<SubViewModelBase> VMList { get; set; } = [];
        private bool CanAdd => VMList.Count < 3;

        [RelayCommand(CanExecute = nameof(CanAdd))]
        public void AddSubPage()
        {
            if (VMList.Count == 0)
                VMList.Add(App.Current.Services.GetRequiredService<TcpViewModel>());
            else
                VMList.Add(App.Current.Services.GetRequiredService<FtpViewModel>());
        }

        [RelayCommand]
        public void Closing(object? item)
        {
            var tab = (item as CancelRoutedEventArgs)?.OriginalSource as SubViewModelBase;
            VMList.Remove(tab!);
            MessageBox.Show(tab?.Title);
            MessageBox.Show(VMList.Count.ToString());
        }
    }
}


