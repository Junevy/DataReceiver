using DataReceiver.ViewModels;
using DataReceiver.Views;
using HandyControl.Tools;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Navigation;

namespace DataReceiver
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static new App Current => (App)Application.Current;
        public IServiceProvider Services;

        // Current 一直为 Null
        //public App()
        //{
           
        //}

        private void BuildServices()
        {
            var container = new ServiceCollection();

            container.AddSingleton<MainView>();
            container.AddSingleton<MainViewModel>();
            container.AddSingleton<DataView>();
            container.AddSingleton<DataViewModel>();
            container.AddSingleton<SocketView>();
            container.AddSingleton<SocketViewModel>();

            Services = container.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            BuildServices();
            MainWindow = Services.GetService<MainView>();
            MainWindow!.Show();
        }


        //protected override void OnLoadCompleted(NavigationEventArgs e)
        //{
        //    base.OnLoadCompleted(e);
        //    App.Current.LoadResource("data_geom");
        //}


        public T? LoadResource<T>(string styleName) where T : class
        {
            return Application.Current.Resources[styleName] as T;
        }

        public T? LoadResource<T>() where T : class
        {
            return Application.Current.Resources[typeof(T)] as T;
        }
    }

}
