using DataReceiver.Models.Socket.Interface;
using DataReceiver.Services.Decorator;
using System.Text;
namespace DataReceiver.Services.Extentions
{
    /// <summary>
    /// IConnection 的扩展方法
    /// </summary>
    public static class ConnectionFeatureExtensions
    {
        public static async Task TryReconnect(this IReactiveConnection connection)
        {
            var reconnectFeature = connection.As<IReconnectCapable>();
            if (reconnectFeature != null) { await reconnectFeature.StartReconnectAsync(); }
        }

        public static async Task TryStartHeartBeat(this IReactiveConnection connection, byte[] response)
        {
            var heartBeatFeature = connection.As<IHeartBeatCapable>();
            if (heartBeatFeature != null) { await heartBeatFeature.StartHeartBeatAsync(response); }
        }

        /// <summary>
        /// 从多层 Decorator嵌套中获取指定类型或接口类型
        /// </summary>
        /// <typeparam name="T">执行类型或接口</typeparam>
        /// <param name="connection">连接对象</param>
        /// <returns></returns>
        public static T? As<T>(this IReactiveConnection connection) where T : class
        {
            if (connection is T self) return self;
            if (connection is ConnectionDecoratorBase decorator) return decorator.Inner.As<T>();
            return connection as T;
        }

        /// <summary>
        /// 判断是否支持指定功能
        /// </summary>
        /// <typeparam name="T">指定类型或功能</typeparam>
        /// <param name="connection">连接对象</param>
        /// <returns></returns>
        public static bool IsHas<T>(this IReactiveConnection connection) where T : class
            => connection.As<T>() is not null;

        /// <summary>
        /// 获取最底层的链接对象
        /// </summary>
        /// <param name="connection">连接对象</param>
        /// <returns>最底层的连接对象</returns>
        public static IReactiveConnection Unwrap(this IReactiveConnection connection)
        {
            while (connection is ConnectionDecoratorBase decorator)
            {
                connection = decorator.Inner;
            }
            return connection;
        }

        /// <summary>
        /// 调试用：打印装饰链结构。
        /// </summary>
        public static string Describe(this IConnection connection)
        {
            var sb = new StringBuilder();
            var current = connection;
            int level = 0;

            while (current is ConnectionDecoratorBase decorator)
            {
                sb.AppendLine($"{new string(' ', level * 2)}↳ {current.GetType().Name}");
                current = decorator.Inner;
                level++;
            }

            sb.AppendLine($"{new string(' ', level * 2)}└─ {current.GetType().Name}");
            return sb.ToString();
        }
    }
}
