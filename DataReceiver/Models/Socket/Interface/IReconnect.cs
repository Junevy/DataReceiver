using DataReceiver.Models.Socket.Config;

namespace DataReceiver.Models.Socket.Interface
{
    public interface IReconnect
    {
        ReconnectConfig ReconnectConfig { get; }
    }
}
