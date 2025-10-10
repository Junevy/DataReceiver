using CommunityToolkit.Mvvm.ComponentModel;
using DataReceiver.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReceiver.ViewModels.Community
{
    public partial class SubViewModelBase : ViewModelBase
    {
        [ObservableProperty]
        public string title = string.Empty;

        public SubViewModelBase(string? title = null)
        {
            Title = title ?? Title + " - " + 1;
        }
    }
}
