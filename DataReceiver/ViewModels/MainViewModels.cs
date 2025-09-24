using CommunityToolkit.Mvvm.ComponentModel;
using DataReceiver.Helper.Control;
using System.Collections.ObjectModel;

namespace DataReceiver.ViewModels
{
    public partial class MainViewModels : ObservableObject
    {
        public ObservableCollection<NavigationListBox> Navigator = [];
    }
}
