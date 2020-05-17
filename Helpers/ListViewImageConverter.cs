// EVECSC - Michael
// ListViewImageConverter.cs
// Last Cleanup: 17/05/2020 17:39
// Created: 17/05/2020 16:10

#region Directives
using System;
using System.Globalization;
using System.Windows.Data;
#endregion

namespace EVECSC.Helpers
{
    public class ListViewImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var statusValue = value?.ToString().ToUpper();

            if (!string.IsNullOrEmpty(statusValue))
            {
                string result;

                switch (statusValue)
                {
                    case "MALE":
                        result = "male.png";
                        break;
                    case "FEMALE":
                        result = "female.png";
                        break;
                    default:
                        result = "silhouette.png";
                        break;
                }

                var uri = new Uri("pack://application:,,,/Content/Icons/" + result);

                return uri;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}