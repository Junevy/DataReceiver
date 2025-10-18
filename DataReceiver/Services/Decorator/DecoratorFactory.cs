using DataReceiver.Models.Socket.Interface;

namespace DataReceiver.Services.Decorator
{
    public static class DecoratorFactory
    {
        public static IConnection CreateDecorator(IConnection conn, bool enableReconnect = false, bool enableHeartBeat = false)
        {
            IConnection connection = conn;

            if (enableReconnect)
                connection = new ReconnectDecorator(connection);

            if (enableHeartBeat)
                connection = new HeartBeatDecorator(connection);

            return connection;
        }
    }
}
