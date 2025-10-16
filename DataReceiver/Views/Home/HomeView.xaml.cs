using DataReceiver.ViewModels.Home;
using System.Windows.Controls;

namespace DataReceiver.Views.Home
{
    public partial class HomeView : Page
    {
        public HomeView(HomeViewModel homeViewModel)
        {
            InitializeComponent();
            this.DataContext = homeViewModel;
        }
    }
}
