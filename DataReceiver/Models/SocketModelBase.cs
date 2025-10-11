using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReceiver.Models
{
    public abstract class SocketModelBase
    {
        public bool IsConnected { get; set; } = false;
        public bool IsAutoConnect { get; set; } = true;
    }
}
