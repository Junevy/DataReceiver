using DataReceiver.Models.Config;
using DataReceiver.Models.Socket.Config;
using DataReceiver.Models.Socket.Interface;
using DataReceiver.Services.Decorator;

namespace DataReceiver.Services.Factory
{
    public static class DecoratorFactory
    {
        public static IReactiveConnection CreateReconncetDecorator(IReactiveConnection conn, ReconnectConfig config)
        {
            IReactiveConnection connection = conn;
            if (config.IsEnable)
                connection = new ReconnectDecorator(connection, config);
            return connection;
        }

        public static IReactiveConnection CreateHeartBeatDecorator(IReactiveConnection conn, HeartBeatConfig config)
        {
            IReactiveConnection connection = conn;
            if (config.IsEnable)
                connection = new HeartBeatDecorator(connection, config);
            return connection;
        }



    }
}
