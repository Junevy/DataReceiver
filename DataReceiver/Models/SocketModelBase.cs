using DataReceiver.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReceiver.Models
{
    public abstract class SocketModelBase : ISocket
    {
        public bool IsConnected { get; set; } = false;
        public bool IsAutoConnect { get; set; } = true;
        public string SendMessage { get; set; } = string.Empty;

        public abstract void Receive();

        public abstract void Send();

        public abstract void Start();

        public abstract void Stop();

        public abstract void ReStart();
    }
}
