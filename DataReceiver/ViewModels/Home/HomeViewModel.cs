using CommunityToolkit.Mvvm.ComponentModel;
using DataReceiver.ViewModels.Base;

namespace DataReceiver.ViewModels.Home
{
    public partial class HomeViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string message = "hello world";
        public override void Dispose()
        {
           
        }
    }
}
