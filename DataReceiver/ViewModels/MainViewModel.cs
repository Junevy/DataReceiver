using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataReceiver.Services.Navigation;
using DataReceiver.ViewModels.Base;
using DataReceiver.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace DataReceiver.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        public ObservableCollection<ListBoxItem> NaviList { get; private set; } = [];
        private readonly NavigationService Navigator;

        public MainViewModel(NavigationService navigator)
        {
            this.Navigator = navigator;
        }

        /// <summary>
        /// Load the bar When window initialized
        /// </summary>
        [RelayCommand]
        public void OnWindowLoaded()
        {
            Navigator.NavigateTo<HomeView>();

            AddItem("home_geom", "HomeView");
            AddItem("socket_geom", "SocketView");
            AddItem("data_geom", "DataView");
        }

        [RelayCommand]
        public void Navigate(object? viewName)
        {
            if (viewName is null) return;
            switch (viewName)
            {
                case "HomeView":
                    Navigator.NavigateTo<HomeView>(); break;
                case "DataView":
                    Navigator.NavigateTo<DataView>(); break;
                case "SocketView":
                    Navigator.NavigateTo<ConnectionView>(); break;
                default:
                    return;
            }
        }

        /// <summary>
        /// 动态添加 siderbar
        /// </summary>
        /// <param name="resourceName"> 资源路径，可以是Style或任何资源</param>
        /// <param name="viewName"> 需要导航的ViewModel </param>
        /// <param name="width"> 控件宽度，默认40</param>
        /// <param name="height">控件宽度，默认40</param>
        public void AddItem(string resourceName, string viewName, int width = 40, int height = 40)
        {
            var geometry = App.Current.LoadResource<Geometry>(resourceName);
            NaviList.Add(new ListBoxItem
            {
                Name = viewName,
                Width = width + 10,
                Height = height + 5,
                Content = geometry,
                Style = App.Current.LoadResource<Style>("SiderBar_Style"),
            });
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
