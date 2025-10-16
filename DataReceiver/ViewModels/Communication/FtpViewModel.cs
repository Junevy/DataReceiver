using DataReceiver.Models.Common;
using DataReceiver.Models.CommunicationCommon;
using DataReceiver.Models.Socket;
using System.ComponentModel;

namespace DataReceiver.ViewModels.Communication
{
    public partial class FtpViewModel : ConnectionViewModelBase
    {
        public FtpViewModel(TcpClientModel model) : base(model.Runtimes)
        {
        }

        public override Task ConnectAsync()
        {
            throw new NotImplementedException();
        }

        public override Task DisconnectAsync()
        {
            throw new NotImplementedException();
        }



        //private SocketBase model;
        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override void OnRuntimesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        //public override void ReceivedData(DataEventArgs<byte> args)
        //{
        //    throw new NotImplementedException();
        //}

        public override Task SendAsync(string message)
        {
            throw new NotImplementedException();
        }

    }
}
