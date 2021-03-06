﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using OpenSALib3.DatHandler;

namespace Tabuu.Utility {
    public class ItemToContextMenuConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is DatFile)
                return Application.Current.Resources["DatFileContextMenu"];
            if (value is DatSection)
                return Application.Current.Resources["DatSectionContextMenu"];
            if (value is DatElement)
                return Application.Current.Resources["DatElementContextMenu"];
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}