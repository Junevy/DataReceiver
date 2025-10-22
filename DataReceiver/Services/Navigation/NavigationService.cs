using DataReceiver.Services.Factory;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DataReceiver.Services.Navigation
{
    public class NavigationService(Frame frame) : INavigation
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DecoratorFactory));
        private const string PackagePath = "DataReceiver.Views";
        private readonly Frame mainFrame = frame;

        /// <summary>
        /// 导航实现
        /// </summary>
        /// <typeparam name="T"> 需要导航的Page </typeparam>
        public void NavigateTo<T>() where T : class
        {
            //var view = FindView<T>();
            //if (view is null) return;

            var page = App.Current.Services.GetService<T>() as Page;
            Log.Info($"Navigate to {typeof(T)}");
            mainFrame.Navigate(page);

            //navigate to...
        }

        public Type? FindView<T>(object? viewName = null)
        {
            var test = PackagePath + "." + viewName as string;
            var type = Type.GetType(test);
            try
            {
                Log.Error($"FindView successful: {typeof(T)}");
                return type;
            }
            catch (Exception e)
            {
                //Log...}return null
                Log.Error($"FindView error: {typeof(T)}");

                return null;
            }
        }



    }
}
