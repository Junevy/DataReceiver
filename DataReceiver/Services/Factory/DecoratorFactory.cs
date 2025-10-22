using DataReceiver.Models.Config;
using DataReceiver.Models.Socket.Config;
using DataReceiver.Models.Socket.Interface;
using DataReceiver.Services.Decorator;
using log4net;

namespace DataReceiver.Services.Factory
{
    public static class DecoratorFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DecoratorFactory));

        public static IReactiveConnection CreateReconncetDecorator(IReactiveConnection conn, ReconnectConfig config)
        {
            IReactiveConnection connection = conn;
            if (config.IsEnable)
            {
                Log.Info("The Reconncet task is enable.");
                connection = new ReconnectDecorator(connection, config);
            }
            Log.Info("The Reconncet task is unenable.");
            return connection;
        }

        public static IReactiveConnection CreateHeartBeatDecorator(IReactiveConnection conn, HeartBeatConfig config)
        {
            IReactiveConnection connection = conn;
            if (config.IsEnable)
            {
                Log.Info("The Reconncet task is enable.");
                connection = new HeartBeatDecorator(connection, config);
            }
            Log.Info("The HeartBeat task is unenable.");

            return connection;
        }



    }
}
