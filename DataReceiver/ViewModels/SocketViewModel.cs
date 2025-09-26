using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataReceiver.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataReceiver.ViewModels
{
    public partial class SocketViewModel : ViewModelBase
    {
        public ObservableCollection<PageViewModel> VMList { get; set; } = [];
        private bool CanExe => VMList.Count < 10;

        public SocketViewModel()
        {
            
        }

        [RelayCommand(CanExecute = nameof(CanExe))]
        public void AddSubPage()
        {
            var viewName = "subView" + $"{VMList.Count + 1}";
            VMList.Add(new PageViewModel(viewName, new SocketSubView()));
            
        }
    }
}
