using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace TestForInitium.Converters
{
    public class MimeTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string answer = (string)value;
            if (answer != null)
            {
                return answer = "MimeType: " + value;
            }
            else
                return answer;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
