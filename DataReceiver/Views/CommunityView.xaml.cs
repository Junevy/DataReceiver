using DataReceiver.ViewModels;
using DataReceiver.ViewModels.Community;
using System.Windows.Controls;

namespace DataReceiver.Views
{
    /// <summary>
    /// SocketView.xaml 的交互逻辑
    /// </summary>
    public partial class CommunityView : Page
    {
        public CommunityView(CommunityViewModel socketViewModel)
        {
            InitializeComponent();
            this.DataContext = socketViewModel;
        }

        public CommunityView()
        {
            InitializeComponent();

        }
    }
}
