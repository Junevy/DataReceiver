using DataReceiver.ViewModels;
using System.Windows.Controls;

namespace DataReceiver.Views
{
    /// <summary>
    /// SocketView.xaml 的交互逻辑
    /// </summary>
    public partial class SocketView : Page
    {
        public SocketView(SocketViewModel socketViewModel)
        {
            InitializeComponent();
            this.DataContext = socketViewModel;
        }

        public SocketView()
        {
            InitializeComponent();

        }
    }
}
