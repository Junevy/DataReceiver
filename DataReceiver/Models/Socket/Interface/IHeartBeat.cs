using DataReceiver.Models.Config;

namespace DataReceiver.Models.Socket.Interface
{
    public interface IHeartBeat
    {
        HeartBeatConfig HeartBeatConfig { get; }
    }
}
