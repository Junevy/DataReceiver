using System.Globalization;
using System.Windows.Controls;

namespace DataReceiver.Services.Validation
{
    public class NumValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return int.TryParse(value as string, out int num) && num > 1024 && num <= 65535
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "请输入1024-65535之间的端口");
        }
    }
}
