using CommunityToolkit.Mvvm.ComponentModel;
using DataReceiver.ViewModels.Base;

namespace DataReceiver.ViewModels.Data
{
    public partial class DataViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string message = "hello DataView";

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
