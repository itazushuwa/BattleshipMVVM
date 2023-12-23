using System;
using System.Windows;

namespace Battleship_2.Accessories
{
    public class ShipsImagesManager : IShipsImagesManager
    {
        public String[] GetFirstShipsSet()
        {
            return new[]
            {
                Application.Current.Resources["Fleet_1_Ship_1_Uri"] as string ?? "Error",
                Application.Current.Resources["Fleet_1_Ship_2_Uri"] as string ?? "Error",
                Application.Current.Resources["Fleet_1_Ship_3_Uri"] as string ?? "Error",
                Application.Current.Resources["Fleet_1_Ship_4_Uri"] as string ?? "Error"
            };
        }

        public String[] GetSecondShipsSet()
        {
            return new[]
            {
                Application.Current.Resources["Fleet_2_Ship_1_Uri"] as string ?? "Error",
                Application.Current.Resources["Fleet_2_Ship_2_Uri"] as string ?? "Error",
                Application.Current.Resources["Fleet_2_Ship_3_Uri"] as string ?? "Error",
                Application.Current.Resources["Fleet_2_Ship_4_Uri"] as string ?? "Error"
            };
        }
    }
}
