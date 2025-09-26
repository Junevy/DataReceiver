using DataReceiver.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DataReceiver.Services
{
    public class NavigationService
    {
        private const string PackagePath = "DataReceiver.Views";
        private readonly Frame mainFrame;

        public NavigationService(Frame frame)
        {
            this.mainFrame = frame;
        }

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
            catch (Exception ex)
            {
                //Log...}return null
                return null;
            }
        }



    }
}
