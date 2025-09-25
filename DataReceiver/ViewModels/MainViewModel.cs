using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace DataReceiver.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public ObservableCollection<ListBoxItem> Navigator { get; private set; } = [];

        [ObservableProperty]
        private string message = "test";

        [RelayCommand]
        public void OnWindowLoaded()
        {
            AddItem("data_geom", "SocketView");
            AddItem("data_geom", "HomeView");
            AddItem("data_geom", "DataView");
        }

        [RelayCommand]
        public void FindView(object? viewName)
        {
            string str = "DataReceiver.Views";
            var test = str + "." + viewName as string;

            if (viewName is null) return;

            //Activator.CreateInstance(viewName);
            var type = Type.GetType(test);

        }


        [RelayCommand]
        public void Test(object? param)
        {
            if (param == null)
                MessageBox.Show("TEst");
            else
                MessageBox.Show(param as string);
            FindView(param as string);
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

            Navigator.Add(new ListBoxItem
            {
                Name = viewName,
                Width = width +10,
                Height = height +10,
                Content = new Path
                {
                    Data = geometry,
                    Stroke = Brushes.Gray,
                    Fill = Brushes.Gray,
                    Stretch = Stretch.Uniform,
                    Width = 30,
                    Height = 20,
                },
                Style = App.Current.LoadResource<Style>("SiderBar_Style"),
            });

            Navigator[0].IsSelected = true;
        }
    }
}
