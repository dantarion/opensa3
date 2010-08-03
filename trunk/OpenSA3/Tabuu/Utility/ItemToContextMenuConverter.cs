using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Globalization;
using OpenSALib3;
using OpenSALib3.DatHandler;

namespace Tabuu.Utility
{
    namespace Utility
    {
        public class ItemToContextMenuConverter : IValueConverter
        {
            public static ContextMenu OpenInHexContextMenu;
            public static ContextMenu DirContextMenu;

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var ds = value as DatSection;
                if (ds != null)
                {
                    return Application.Current.Resources["DatSectionContextMenu"];
                }
                var df = value as DatWrapper;
                return df != null ? Application.Current.Resources["DatFileContextMenu"] : null;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
    }

}
