using CommunityToolkit.Mvvm.ComponentModel;
using DataReceiver.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReceiver.ViewModels
{
    public partial class DataViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string message = "hello DataView";
    }
}
