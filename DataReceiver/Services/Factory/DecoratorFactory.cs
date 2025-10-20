using DataReceiver.Models.Config;
using DataReceiver.Models.Socket.Config;
using DataReceiver.Models.Socket.Interface;
using DataReceiver.Services.Decorator;

namespace DataReceiver.Services.Factory
{
    public static class DecoratorFactory
    {
        public static IConnection CreateReconncetDecorator(IConnection conn, ReconnectConfig config)
        {
            IConnection connection = conn;
            if (config.IsEnable)
                connection = new ReconnectDecorator(connection, config);
            return connection;
        }

        public static IConnection CreateHeartBeatDecorator(IConnection conn, HeartBeatConfig config)
        {
            IConnection connection = conn;
            if (config.IsEnable)
                connection = new HeartBeatDecorator(connection, config);
            return connection;
        }



    }
}
