using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReceiver.Services.Interface
{
    public interface ISocket    
    {
        void Start();
        void Stop();
        void Send();
        void Receive();
        void ReStart();
    }
}
