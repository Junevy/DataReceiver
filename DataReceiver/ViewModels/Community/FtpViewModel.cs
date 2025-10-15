using DataReceiver.Models.CommunicationCommon;
using DataReceiver.Models.CommunicationModel;
using System.CodeDom;
using System.IO;

namespace DataReceiver.ViewModels.Community
{
    public partial class FtpViewModel : SubViewModelBase
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
