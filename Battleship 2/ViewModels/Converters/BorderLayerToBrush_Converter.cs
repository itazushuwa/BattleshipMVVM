using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Battleship_2.ViewModels.Converters
{
    internal class BorderLayerToBrush_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Cell_VM viewModel = (Cell_VM)value;
            if (!viewModel.IsOpen && !viewModel.IsCellActive)
                return Application.Current.Resources["BackgroundBrush"];
            if (!viewModel.IsOpen)
                return Brushes.Transparent;
            if (!viewModel.IsShipDeck)
                return Application.Current.Resources["OpenCellBrush"];
            if (!viewModel.IsShipDestroyed)
                return Application.Current.Resources["DamagedShipCellBrush"];

            return Application.Current.Resources["DestroyedShipBrush"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new object();
        }
    }
}
