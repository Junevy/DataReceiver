using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DataReceiver.Helper
{
    public class ColorConverter : IValueConverter
    {
        public Brush TrueBrush { get; set; } = Brushes.MediumPurple;
        public Brush FalseBrush { get; set; } = Brushes.SlateGray;
        public Brush ErrorBrush { get; set; } = Brushes.Red;
        public Brush WatingBrush { get; set; } = Brushes.SkyBlue;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = System.Convert.ToInt32(value);
            if (state == 0) { return FalseBrush; }
            else if (state == 1) { return TrueBrush; }
            else if (state == 6) { return ErrorBrush; }
            else { return  WatingBrush; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? TrueBrush : FalseBrush;
        }
    }
}
