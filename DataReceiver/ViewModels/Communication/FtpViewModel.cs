using DataReceiver.Models.Socket;
using System.ComponentModel;

namespace DataReceiver.ViewModels.Communication
{
    public partial class FtpViewModel(TcpClientModel model) : ConnectionViewModelBase(model.Runtimes)
    {
        public override Task ConnectAsync()
        {
            throw new NotImplementedException();
        }

        public override Task DisconnectAsync()
        {
            throw new NotImplementedException();
        }

        public override void OnRuntimesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override Task SendAsync()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
