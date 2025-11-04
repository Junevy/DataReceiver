using Common;
using DataReceiver.Models.Config;
using DataReceiver.Models.Socket.Config;
using DataReceiver.Models.Socket.FTP;
using DataReceiver.Models.Socket.TCP;
using DataReceiver.ViewModels;
using DataReceiver.ViewModels.Communication;
using DataReceiver.ViewModels.Data;
using DataReceiver.ViewModels.Home;
using DataReceiver.Views;
using DataReceiver.Views.Communication;
using DataReceiver.Views.Data;
using DataReceiver.Views.Home;
using log4net;
using log4net.Config;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using NavigationService = DataReceiver.Services.Navigation.NavigationService;

namespace DataReceiver
{
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(App));
        public static new App Current => (App)Application.Current;
        public IServiceProvider Services;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //var converter = this.TryFindResource("BoolToColor");
            //if (converter == null)
            //{
            //    throw new Exception(" BoolToColor 转换器未正确注册！");
            //}
            InitialLogger();
            BuildServices();
            MainWindow = Services.GetService<MainView>();
            MainWindow!.Show();
        }

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

            var test = ConfigHelper.Build<HeartBeatConfig>();

            container.AddTransient( _ => new TcpClientViewModel(
                new TcpClientModel(ConfigHelper.Build<TcpClientConfig>()), 
                ConfigHelper.Build<ReconnectConfig>(),
                ConfigHelper.Build<HeartBeatConfig>()));

            container.AddTransient<FtpServerViewModel>();
            container.AddTransient<NavigationService>();

            // Model
            container.AddTransient<TcpClientModel>(_ => new TcpClientModel(ConfigHelper.Build<TcpClientConfig>()));
            container.AddSingleton<FtpServerModel>(_ => new FtpServerModel(ConfigHelper.Build<FtpServerConfig>()));

            //// Config
            //container.AddTransient<ReconnectConfig>();
            //container.AddTransient<HeartBeatConfig>();

            Services = container.BuildServiceProvider()!;

            Log.Info("Service container initialized.");
        }


        private void InitialLogger()
        {
            var currentPath = AppContext.BaseDirectory;
            var loggerConfigPath = Path.Combine(currentPath, "Assets\\Configs\\AppLogger.config");
            var logFilePath = Path.Combine(AppContext.BaseDirectory, "Logs\\AppLog");

            if (!Directory.Exists(logFilePath))
            {
                Directory.CreateDirectory(logFilePath);
            }

            var loggerRepository = LogManager.GetRepository();
            XmlConfigurator.Configure(loggerRepository, new FileInfo(loggerConfigPath));
        }

        public T? LoadResource<T>(string? styleName = null) where T : class
        {
            return Application.Current.Resources[styleName] as T;
        }
    }
}