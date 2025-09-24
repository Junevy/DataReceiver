using DataReceiver.ViewModels;
using DataReceiver.Views;
using HandyControl.Tools;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace DataReceiver
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static new App Current = (App)Application.Current;
        public IServiceProvider Services;
        //private Style;

        public App()
        {
            var container = new ServiceCollection();
            container.AddSingleton<MainView>();
            container.AddSingleton<MainViewModels>();
            container.AddSingleton<DataView>();
            container.AddSingleton<DataViewModel>(); 
            container.AddSingleton<SocketView>();
            container.AddSingleton<SocketViewModel>();

            Services = container.BuildServiceProvider();
        }

        public Style? LoadResource(string styleName)
        {
            return Application.Current.Resources[styleName] as Style;
        }

        public Style? LoadResource<T>()
        {
            return Application.Current.Resources[typeof(T)] as Style;
        }
    }

}
