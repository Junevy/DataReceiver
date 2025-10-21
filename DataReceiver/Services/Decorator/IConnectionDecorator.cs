using DataReceiver.Models.Socket.Interface;

namespace DataReceiver.Services.Decorator
{
    /// <summary>
    /// 待完成，表示 某个对象是否支持被装饰，或装饰某个对象。
    /// </summary>
    public interface IConnectionDecorator
    {
        /// <summary>
        /// 是否支持包装
        /// </summary>
        /// <param name="connection">该对象是否支持包装</param>
        /// <returns>bool值，表示该对象是否支持被某个装饰器包装</returns>
        bool CanDecorate(IConnection connection);

        /// <summary>
        /// 包装对象
        /// </summary>
        /// <param name="connection">需要包装的对象</param>
        /// <returns>被包装的对象</returns>
        IConnection Decorate(IConnection connection);
    }
}
