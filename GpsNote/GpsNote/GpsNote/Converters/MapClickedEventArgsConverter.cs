using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Converters
{
    public class MapClickedEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MapClickedEventArgs mapClickedEventArgs = value as MapClickedEventArgs;
            if (mapClickedEventArgs == null)
                throw new ArgumentException("Expected value to be of type MapClickedEventArgs", nameof(value));
            return mapClickedEventArgs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}