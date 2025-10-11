using CommunityToolkit.Mvvm.ComponentModel;
using DataReceiver.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReceiver.ViewModels.Community
{
    public partial class FtpViewModel : SubViewModelBase
    {
        [ObservableProperty]
        private string message = "hello FtpView";

        public override void Receive()
        {
            throw new NotImplementedException();
        }

        public override void Send()
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
