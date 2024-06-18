using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using A4DN.Core.BOS.Base;

namespace WPF.OrderManagement.Shared
{
    public class ZeroCheckToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int _int && _int != 0) return Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NullCheckToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string _str && !string.IsNullOrWhiteSpace(_str)) return Visibility.Visible;

            if (value is int) return Visibility.Visible;

            if (value is decimal) return Visibility.Visible;

            if (value is DateTime) return Visibility.Visible;

            if (value is AB_BusinessObjectEntityBase _ent && _ent != null) return Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
