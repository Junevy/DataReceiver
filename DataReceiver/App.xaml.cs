using DataReceiver.Models.Config;
using DataReceiver.Models.Socket;
using DataReceiver.Models.Socket.Config;
using DataReceiver.ViewModels;
using DataReceiver.ViewModels.Communication;
using DataReceiver.ViewModels.Data;
using DataReceiver.ViewModels.Home;
using DataReceiver.Views;
using DataReceiver.Views.Communication;
using DataReceiver.Views.Data;
using DataReceiver.Views.Home;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using NavigationService = DataReceiver.Services.Navigation.NavigationService;

namespace DataReceiver
{
    public partial class App : Application
    {
        public static new App Current => (App)Application.Current;
        public IServiceProvider Services;

        /// <summary>
        /// 注册容器
        /// </summary>
        private void BuildServices()
        {
            var container = new ServiceCollection();

            // View
            container.AddSingleton<Frame>(_
                => new Frame { NavigationUIVisibility = NavigationUIVisibility.Hidden });
            container.AddSingleton<MainView>();
            container.AddSingleton<DataView>();
            container.AddSingleton<HomeView>();
            container.AddSingleton<CommunicationView>();
            container.AddTransient<TcpView>();
            container.AddTransient<FtpView>();

            // ViewModel
            container.AddTransient<MainViewModel>();
            container.AddTransient<DataViewModel>();
            container.AddTransient<HomeViewModel>();
            container.AddTransient<CommunicationViewModel>();
            container.AddTransient<TcpViewModel>();
            container.AddTransient<FtpViewModel>();
            container.AddTransient<NavigationService>();

            // Model
            container.AddTransient<TcpClientModel>(_ => new TcpClientModel(new Models.Config.TcpConfig()));

            // Config
            container.AddTransient<ReconnectConfig>();
            container.AddTransient<HeartBeatConfig>();

            Services = container.BuildServiceProvider()!;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //var converter = this.TryFindResource("BoolToColor");
            //if (converter == null)
            //{
            //    throw new Exception(" BoolToColor 转换器未正确注册！");
            //}

            BuildServices();
            MainWindow = Services.GetService<MainView>();
            MainWindow!.Show();
        }
        public T? LoadResource<T>(string? styleName = null) where T : class
        {
            return Application.Current.Resources[styleName] as T;
        }
    }
}
