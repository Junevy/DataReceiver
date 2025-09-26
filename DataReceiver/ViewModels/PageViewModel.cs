using DataReceiver.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReceiver.ViewModels
{
    public class PageViewModel
    {
        public string Title { get; } = "Page";
        public SocketSubView SubView { get; }

        public PageViewModel(string title, SocketSubView subView)
        {
            this.Title = title;
            SubView = subView;
        }
    }
}
