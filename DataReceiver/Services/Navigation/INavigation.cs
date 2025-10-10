using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReceiver.Services.Navigation
{
    internal interface INavigation
    {
        public void NavigateTo<T>() where T : class;
    }
}
