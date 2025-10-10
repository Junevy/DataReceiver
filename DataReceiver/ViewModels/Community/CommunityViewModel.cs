using CommunityToolkit.Mvvm.Input;
using DataReceiver.ViewModels.Base;
using DataReceiver.Views;
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
        private bool CanAdd => VMList.Count < 3;

        [RelayCommand(CanExecute = nameof(CanAdd))]
        public void AddSubPage()
        {
            //if (!CanAdd)
            if( VMList.Count == 0)
                VMList.Add(App.Current.Services.GetRequiredService<TcpViewModel>());
            else
                VMList.Add(App.Current.Services.GetRequiredService<FtpViewModel>());
        }
    }
}
