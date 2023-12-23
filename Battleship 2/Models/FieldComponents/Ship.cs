using System;
using System.Collections.Generic;
using System.Linq;
using Battleship_2.Models.FieldComponents.Enumerations;

namespace Battleship_2.Models.FieldComponents
{
    public class Ship
    {
        public int NumberOfDecks => Cells.Count;
        public List<FieldCell> Cells { get; set; }
        public Guid ShipGuid { get; }
        public bool IsDestroyed => !Cells.Any(c => !c.IsOpen);
        public OrientationsEnum Orientation { get; }

        public Ship(OrientationsEnum orientation) : this(new List<FieldCell>(), orientation) { }
        public Ship(List<FieldCell> cells, OrientationsEnum orientation)
        {
            ShipGuid = Guid.NewGuid();
            Cells = cells;
            Orientation = orientation;
        }

        public void AddCell(ref FieldCell cell)
        {
            Cells.Add(cell);
        }
    }
}
