namespace DataReceiver.Models.Common
{
    public enum ConnectionState
    {
        // true
        Connected = 0,

        // false
        Connecting = 1,
        Reconnecting = 2,
        Reconnected = 3,
        Disconnecting = 4,
        Error = 5,
        Disconnected = -1
    }
}
