using System;
using System.Windows.Media;

namespace Battleship_2.ViewModels
{
    public interface IShip_VM
    {
        string ShipImageUri { get; set; }
        Transform Rotation { get; set; }
        Int32 Row { get; set; }
        Int32 Column { get; set; }
        Int32 RowSpan { get; set; }
        Int32 ColumnSpan { get; set; }
        bool IsVisible { get; set; }
    }
}
