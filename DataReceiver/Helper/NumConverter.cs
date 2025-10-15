using System.Globalization;
using System.Windows.Data;

namespace DataReceiver.Helper
{
    public class NumConverter : IValueConverter
    {
        public System.Windows.Media.Brush TrueBrush { get; set; }
            = System.Windows.Media.Brushes.MediumPurple;
        public System.Windows.Media.Brush FalseBrush { get; set; }
            = System.Windows.Media.Brushes.SlateGray;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = System.Convert.ToInt32(value);
            if (0 <= state && state <= 3)
                return TrueBrush;
            else
                return FalseBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? TrueBrush : FalseBrush;
        }


        //public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
