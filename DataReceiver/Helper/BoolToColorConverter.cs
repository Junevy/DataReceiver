using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace DataReceiver.Helper
{
    internal class BoolToColorConverter : IValueConverter
    {
        //private System.Windows.Media.Color OK = Colors.Red;
        //private System.Windows.Media.Color NG = Colors.Blue;

        public System.Windows.Media.Brush TrueBrush { get; set; } = System.Windows.Media.Brushes.MediumPurple;
        public System.Windows.Media.Brush FalseBrush { get; set; } = System.Windows.Media.Brushes.SlateGray;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? TrueBrush : FalseBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? TrueBrush : FalseBrush;
        }
    }
}
