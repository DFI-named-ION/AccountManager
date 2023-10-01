using System;
using System.Globalization;
using System.Windows.Data;

namespace AccountManager.Services
{
    public class CryptConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string data = string.Empty;
            if (value != null)
            {
                data = value.ToString();
            }
            return DataCryptography.AESDecrypt(data);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DataCryptography.AESEncrypt(value.ToString());
        }
    }
}
