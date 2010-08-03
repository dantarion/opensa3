using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Controls;
using System.Globalization;
using OpenSALib3;
namespace Tabuu
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
                    return App.Current.Resources["DatSectionContextMenu"];
                }
                var df = value as DatWrapper;
                if (df != null)
                {
                    return App.Current.Resources["DatFileContextMenu"];
                }
                return null;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
    }

}
