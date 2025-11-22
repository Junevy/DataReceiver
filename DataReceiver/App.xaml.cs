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
using FubarDev.FtpServer;
using FubarDev.FtpServer.AccountManagement;
using FubarDev.FtpServer.FileSystem.DotNet;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
        public IServiceProvider Services { get; private set; }

        // 需要检查目录是否存在
        public string ConfigPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                         "DataReceiverConfigs");
        public readonly string configName = "appsettings.json";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //var converter = this.TryFindResource("BoolToColor");
            //if (converter == null)
            //{
            //    throw new Exception(" BoolToColor 转换器未正确注册！");
            //}
            InitialLogger();

            // Check appsettings config.
            if (!Directory.Exists(ConfigPath) || !File.Exists(Path.Combine(ConfigPath, configName)))
            {
                Log.Fatal("Config directory or config files does not existed! Please execute \"install\" script!");
                MessageBox.Show("Config directory or config files does not existed! Please execute \"install\" script!");
                Current.Shutdown();
            }

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


            /* ==============================================================
             * Config
             * ============================================================= */
            var configuration = new ConfigurationBuilder()
                        .SetBasePath(ConfigPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();
            container.AddSingleton<IConfiguration>(configuration);

            container.Configure<TcpClientConfig>(configuration.GetSection(nameof(TcpClientConfig)));
            container.Configure<ReconnectConfig>(configuration.GetSection(nameof(ReconnectConfig)));
            container.Configure<HeartBeatConfig>(configuration.GetSection(nameof(HeartBeatConfig)));
            container.Configure<FtpServerConfig>(configuration.GetSection(nameof(FtpServerConfig)));

            container.AddSingleton(sp => sp.GetRequiredService<IOptions<TcpClientConfig>>().Value);
            container.AddSingleton(sp => sp.GetRequiredService<IOptions<ReconnectConfig>>().Value);
            container.AddSingleton(sp => sp.GetRequiredService<IOptions<HeartBeatConfig>>().Value);
            container.AddSingleton(sp => sp.GetRequiredService<IOptions<FtpServerConfig>>().Value);

            container.AddSingleton<TaskScheduleConfig>();

            /* ==============================================================
             * View
             * ============================================================= */
            container.AddSingleton(sp => new Frame { NavigationUIVisibility = NavigationUIVisibility.Hidden });
            container.AddSingleton<MainView>();
            container.AddSingleton<DataView>();
            container.AddSingleton<HomeView>();
            container.AddSingleton<CommunicationView>();
            container.AddTransient<TcpView>();
            container.AddTransient<FtpView>();


            /* ==============================================================
             * ViewModel
             * ============================================================= */
            container.AddTransient<MainViewModel>();
            container.AddTransient<DataViewModel>();
            container.AddTransient<HomeViewModel>();
            container.AddTransient<CommunicationViewModel>();
            container.AddTransient<FtpServerViewModel>();
            container.AddTransient<TcpClientViewModel>();


            /* ==============================================================
             * Model
             * ============================================================= */
            container.AddTransient<TcpClientModel>();
            container.AddSingleton<FtpServerModel>();

            container.AddSingleton<NavigationService>();

            /* ==============================================================
             * Ftp Server
             * ============================================================= */
            var config = configuration.GetSection(nameof(FtpServerConfig)).Get<FtpServerConfig>()
                ?? new FtpServerConfig();
            container.AddFtpServer(builder =>
            {
                builder.UseDotNetFileSystem();
                if (config.AllowAnonymous)
                    builder.EnableAnonymousAuthentication();
            });
            //var ftpServerBuilder = Services.GetRequiredService<IFtpServerBuilder>();

            // Configure the FTP server
            container.Configure<FtpServerOptions>(opt =>
            {
                opt.ServerAddress = config.Ip;
                opt.Port = config.Port;
                opt.MaxActiveConnections = config.MaxConnections;
                opt.ConnectionInactivityCheckInterval = config.InactiveCheckInterval;
            });
            // use Config.RootPath as root folder
            container.Configure<DotNetFileSystemOptions>(opt =>
            {
                opt.RootPath = config.RootPath;
                opt.FlushAfterWrite = true;  // 写入后立即刷新
                opt.StreamBufferSize = config.BufferSize * 1000;
            });
            // Config connection parameters
            container.Configure<FtpConnectionOptions>(opt =>
            {
                //opt.DefaultEncoding = Config.Encoding;
                opt.DefaultEncoding = System.Text.Encoding.UTF8;
                opt.InactivityTimeout = config.InactiveTimeOut;
            });
            // Authentication
            container.AddSingleton<IMembershipProvider>(
                new UserShipProvider(config.UserName, config.Password)
            );

            Services = container.BuildServiceProvider()!;

            Log.Info("Service provider initialized.");
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


#region old code
//container.AddTransient<NavigationService>();

//// View
//container.AddSingleton<Frame>(_
//    => new Frame { NavigationUIVisibility = NavigationUIVisibility.Hidden });
//container.AddSingleton<MainView>();
//container.AddSingleton<DataView>();
//container.AddSingleton<HomeView>();
//container.AddSingleton<CommunicationView>();
//container.AddTransient<TcpView>();
//container.AddTransient<FtpView>();

//// ViewModel
//container.AddTransient<MainViewModel>();
//container.AddTransient<DataViewModel>();
//container.AddTransient<HomeViewModel>();
//container.AddTransient<CommunicationViewModel>();
//container.AddTransient<FtpServerViewModel>();
//container.AddTransient(_ => new TcpClientViewModel(
//    new TcpClientModel(ConfigService.Get<TcpClientConfig>()),
//    ConfigService.Get<ReconnectConfig>(),
//    ConfigService.Get<HeartBeatConfig>()));

//// Model
//container.AddTransient<TcpClientModel>(_ => new TcpClientModel(ConfigService.Get<TcpClientConfig>()));
//container.AddSingleton<FtpServerModel>();
//container.AddSingleton<FtpServerConfig>(_ => ConfigService.Get<FtpServerConfig>());
//var test = ConfigService.Get<HeartBeatConfig>();

//container.Configure

#endregion