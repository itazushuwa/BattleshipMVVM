using System.Windows.Media;

namespace Battleship_2.ViewModels
{
    public class Ship_VM : IShip_VM
    {
        public string ShipImageUri { get; set; }
        public Transform Rotation { get; set; }
        public int Row { get; set; } = 0;
        public int Column { get; set; } = 0;
        public int RowSpan { get; set; } = 1;
        public int ColumnSpan { get; set; } = 1;
        public bool IsVisible { get; set; } = false;

        public Ship_VM() : this("", new RotateTransform()) { }
        public Ship_VM(string shipImageUri) : this(shipImageUri, new RotateTransform()) { }
        public Ship_VM(string shipImageUri, Transform rotation)
        {
            ShipImageUri = shipImageUri;
            Rotation = rotation;        
        }
    }
}
