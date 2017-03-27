using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Team4Clock
{
    class BCConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, 
            System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
            {
                {
                    return Brushes.White;
                }
            }
            return Brushes.DimGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, 
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();    // no reason to implement convert-back
        }
    }
}
