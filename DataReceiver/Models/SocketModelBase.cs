using DataReceiver.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReceiver.Models
{
    public abstract class SocketModelBase : ISocket
    {
        //public bool IsConnected { get; set; } = false;
        public bool IsAutoConnect { get; set; } = true;
        //public string SendMessage { get; set; } = string.Empty;
        public DateTime LastActivedTime { get; set; } = DateTime.MinValue;

        public abstract Task StartAsync();

        public abstract void Stop();

        public abstract void ReStart();

        public abstract Task SendAsync(string message);
        public abstract Task ReceiveAsync(Stream stream);
    }
}
