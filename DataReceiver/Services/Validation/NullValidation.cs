using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataReceiver.Services.Validation
{
    class NullValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace(value as string) || (value as string)?.Length >= 10
                ? new ValidationResult(false, "Check the HeartBeat Content!") : ValidationResult.ValidResult;
        }
    }
}
