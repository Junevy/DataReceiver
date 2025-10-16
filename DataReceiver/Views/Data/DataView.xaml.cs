using DataReceiver.ViewModels.Data;
using System.Windows.Controls;

namespace DataReceiver.Views.Data
{
    /// <summary>
    /// DataView.xaml 的交互逻辑
    /// </summary>
    public partial class DataView : Page
    {
        public DataView(DataViewModel dataViewModel)
        {
            InitializeComponent();
            this.DataContext = dataViewModel;
        }
    }
}
