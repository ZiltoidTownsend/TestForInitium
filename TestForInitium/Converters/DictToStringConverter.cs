using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using TestForInitium.Models;

namespace TestForInitium.Converters
{
    public class DictToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string answer = "";
            if (parameter.ToString() == "Count")
            {
                var a = (int)value[0];
                var b = (int)value[1];
                var c = (float)a / b * 100;
                answer = value[0].ToString() + " / " + value[1].ToString() + "   " + c + "%";
            }
            else
            {
                double a = (double)value[0];
                double b = (int)value[1];
                answer = (a / b).ToString() + " байт";
            }
            return answer;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
