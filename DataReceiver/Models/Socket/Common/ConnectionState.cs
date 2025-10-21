namespace DataReceiver.Models.Socket.Common
{
    public enum ConnectionState
    {
        // true
        Connected = 1,

        // false
        Connecting = 2,
        //Reconnecting = 3,
        Reconnected = 4,
        Disconnecting = 5,
        Error = -1,
        Disconnected = 0

    }
}
