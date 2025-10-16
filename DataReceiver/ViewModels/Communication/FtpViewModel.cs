using DataReceiver.Models.CommunicationCommon;

namespace DataReceiver.ViewModels.Communication
{
    public partial class FtpViewModel : ConnectionViewModelBase
    {
        public override Task ConnectAsync()
        {
            throw new NotImplementedException();
        }

        public override void Disconnect()
        {
            throw new NotImplementedException();
        }

        //private SocketBase model;
        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override void ReceivedData(DataEventArgs<byte> args)
        {
            throw new NotImplementedException();
        }

        public override Task SendAsync(string message)
        {
            throw new NotImplementedException();
        }

    }
}
