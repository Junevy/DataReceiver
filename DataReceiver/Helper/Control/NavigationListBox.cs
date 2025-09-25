using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataReceiver.Helper.Control
{
    public sealed class NavigationListBox : ListBoxItem
    {
        //private Uri _icon;
        //private string _content;

        public NavigationListBox(Uri icon, string content)
        {
            Content = icon;
        }

    }
}
