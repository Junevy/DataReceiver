
using DataReceiver.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace DataReceiver.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(MainViewModel mainViewModel, Frame frame)
        {
            InitializeComponent();
            this.DataContext = mainViewModel;
            MainFrameContainer.Content = frame;

        }
    }
}
