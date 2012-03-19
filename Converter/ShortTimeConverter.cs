using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace QuickBlox.SuperSample.Converters
{
    /// <summary>
    /// String to DateTime Converter
    /// </summary>
    public class ShortTimeConverter : IValueConverter
    {
        /// <summary>
        /// Convert from DateTime to string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string formattedDate = "";

            DateTime? date = value as DateTime?;

            if (date != null && date.HasValue)
            {
                formattedDate = date.Value.ToString();
            }

            return formattedDate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
