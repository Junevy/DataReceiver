using DataReceiver.ViewModels.Community;
using System.Windows.Controls;

namespace DataReceiver.Views
{
    /// <summary>
    /// SocketView.xaml 的交互逻辑
    /// </summary>
    public partial class ConnectionView : Page
    {
        public ConnectionView(ConnectionViewModel socketViewModel)
        {
            InitializeComponent();
            this.DataContext = socketViewModel;
        }

        public ConnectionView()
        {
            InitializeComponent();

        }
    }
}
