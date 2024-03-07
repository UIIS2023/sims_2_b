using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace InitialProject.Converters
{
    public class ComboBoxItemToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            // extract the Content property from the ComboBoxItem
            if (value is ComboBoxItem comboBoxItem)
            {
                return int.TryParse(comboBoxItem.Content.ToString(), out int result) ? result : null;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();

                switch (intValue)
                {
                    case 1:
                        comboBoxItem.Content = "1";
                        break;
                    case 2:
                        comboBoxItem.Content = "2";
                        break;
                    case 3:
                        comboBoxItem.Content = "3";
                        break;
                    case 4:
                        comboBoxItem.Content = "4";
                        break;
                    case 5:
                        comboBoxItem.Content = "5";
                        break;
                    default:
                        comboBoxItem = null;
                        break;
                }

                return comboBoxItem;
            }

            return null;
        }
    }

}
