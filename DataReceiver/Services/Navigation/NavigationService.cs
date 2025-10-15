using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DataReceiver.Services.Navigation
{
    public class NavigationService : INavigation
    {
        private const string PackagePath = "DataReceiver.Views";
        private readonly Frame mainFrame;

        public NavigationService(Frame frame)
        {
            mainFrame = frame;
        }

        /// <summary>
        /// 导航实现
        /// </summary>
        /// <typeparam name="T"> 需要导航的Page </typeparam>
        public void NavigateTo<T>() where T : class
        {
            //var view = FindView<T>();
            //if (view is null) return;

            var page = App.Current.Services.GetService<T>() as Page;
            mainFrame.Navigate(page);

            //navigate to...
        }

        public Type? FindView<T>(object? viewName = null)
        {
            var test = PackagePath + "." + viewName as string;
            var type = Type.GetType(test);
            try
            {
                return type;
            }
            catch (Exception e)
            {
                //Log...}return null
                return null;
            }
        }



    }
}
