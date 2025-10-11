using DataReceiver.ViewModels;
using DataReceiver.ViewModels.Community;
using DataReceiver.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using DataView = DataReceiver.Views.DataView;
using NavigationService = DataReceiver.Services.Navigation.NavigationService;

namespace DataReceiver
{
    public partial class App : Application
    {
        public static new App Current => (App)Application.Current;
        public IServiceProvider Services;

        private void BuildServices()
        {
            var container = new ServiceCollection();

            container.AddSingleton<Frame>(_
                => new Frame { NavigationUIVisibility = NavigationUIVisibility.Hidden });
            container.AddSingleton<MainView>();
            container.AddSingleton<DataView>();
            container.AddSingleton<HomeView>();
            container.AddSingleton<CommunityView>();
            container.AddTransient<TcpView>();
            container.AddTransient<FtpView>();

            container.AddTransient<MainViewModel>();
            container.AddTransient<DataViewModel>();
            container.AddTransient<HomeViewModel>();
            container.AddTransient<CommunityViewModel>();
            container.AddTransient<TcpViewModel>();
            container.AddTransient<FtpViewModel>();
            container.AddTransient<NavigationService>();

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
