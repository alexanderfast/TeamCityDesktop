using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TeamCityDesktop.Converters
{
    public class BoolToVisibility : IValueConverter
    {
        public Visibility OnTrue { get; set; }
        public Visibility OnFalse { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = value is bool ? (bool)value : false;
            return b ? OnTrue : OnFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
