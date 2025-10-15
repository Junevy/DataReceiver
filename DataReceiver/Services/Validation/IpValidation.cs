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
        public static bool Validate(string value, string port)
        {
            var ip = value as string ?? "None";
            var result = System.Net.IPAddress.TryParse(ip, out _);
            var portResult = int.TryParse(port, out int num) && num > 1024 && num <= 65535;

            if (ip is not null && result && portResult)
            {
                var splited = ip.Split('.');
                if (splited.Length != 4 || splited.All(x => int.Parse(x) <= 0 && int.Parse(x) >= 255))
                    return false;
                return true;
            }
            return false;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var ip = value as string ?? "None";
            var result = System.Net.IPAddress.TryParse(ip, out _);

            if (ip is not null && result)
            {
                var splited = ip.Split('.');
                if (splited.Length != 4 || splited.All( x => int.Parse(x) <= 0 && int.Parse(x) >= 255))
                    return new ValidationResult(false, "请输入正确的IP地址");
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "请输入正确的IP地址");
        }
    }
}
