using DataReceiver.Models.Config;
using DataReceiver.Models.Socket.Config;
using DataReceiver.Models.Socket.FTP;
using DataReceiver.Models.Socket.Interface;
using DataReceiver.Services.Decorator;
using log4net;

namespace DataReceiver.Services.Factory
{
    public static class DecoratorFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DecoratorFactory));

        public static IConnection CreateReconncetDecorator(IConnection conn, ReconnectConfig config)
        {
            IConnection connection = conn;
            if (config.IsEnable)
            {
                Log.Info("The Reconncet task is enable.");
                connection = new ReconnectDecorator(connection, config);
                return connection;
            }
            Log.Info("The Reconncet task is unenable.");
            return connection;
        }

        public static IConnection CreateHeartBeatDecorator(IConnection conn, HeartBeatConfig config)
        {
            IConnection connection = conn;
            if (config.IsEnable)
            {
                Log.Info("The HeartBeat task is enable.");
                connection = new HeartBeatDecorator(connection, config);
                return connection;
            }
            Log.Info("The HeartBeat task is unenable.");
            return connection;
        }
    }
}
