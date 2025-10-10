using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataReceiver.Services.Validation
{
    internal class IpValidation : ValidationRule
    {
        //internal static bool HasError(string ip, string port)
        //{
        //    return System.Net.IPAddress.TryParse(ip, out var address)
        //        && (int.TryParse(port, out int num) || num < 0 || num > 65535);
        //}

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            //string ip = value as string ?? "None";
            return System.Net.IPAddress.TryParse(value as string, out var address)
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "请输入正确的IP地址");
        }
    }
}
