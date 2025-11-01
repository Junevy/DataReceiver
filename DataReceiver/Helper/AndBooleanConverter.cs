using System.Globalization;
using System.Windows.Data;

namespace DataReceiver.Helper
{
    public class AndBooleanConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // 如果所有值都是 true，返回 true，否则 false
            foreach (var v in values)
            {
                if (v is bool b)
                {
                    if (!b)
                        return false;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
