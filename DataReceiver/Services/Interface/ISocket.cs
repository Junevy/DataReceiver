using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReceiver.Services.Interface
{
    public interface ISocket    
    {
        Task StartAsync();
        void Stop();
        Task SendAsync(string message);
        Task ReceiveAsync(Stream stream);
        void ReStart();
    }
}
