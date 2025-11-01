using DataReceiver.Models.Socket.Config;
using DataReceiver.Models.Socket.Interface;

namespace DataReceiver.Services.Decorator
{
    internal class AutoCleanDirectoryDecorator : IAutoCleanCapable
    {
        public FtpServerConfig Config => throw new NotImplementedException();

        public CancellationTokenSource TokenSource => throw new NotImplementedException();

        public async Task StartAutoClean()
        {
            throw new NotImplementedException();
        }

        public void StopAutoClean()
        {
            throw new NotImplementedException();
        }
    }
}
