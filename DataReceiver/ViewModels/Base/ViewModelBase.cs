using CommunityToolkit.Mvvm.ComponentModel;

namespace DataReceiver.ViewModels.Base
{
    public abstract partial class ViewModelBase : ObservableObject, IDisposable
    {
        public abstract void Dispose();
    }
}
