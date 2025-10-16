using DataReceiver.ViewModels.Communication;
using System.Windows.Controls;

namespace DataReceiver.Views.Communication
{
    /// <summary>
    /// SocketView.xaml 的交互逻辑
    /// </summary>
    public partial class CommunicationView : Page
    {
        public CommunicationView(CommunicationViewModel socketViewModel)
        {
            InitializeComponent();
            this.DataContext = socketViewModel;
        }

        public CommunicationView()
        {
            InitializeComponent();

        }
    }
}
