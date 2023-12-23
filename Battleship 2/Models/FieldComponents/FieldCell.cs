using System;
using System.Collections.Generic;
using Battleship_2.Models.FieldComponents.Enumerations;

namespace Battleship_2.Models.FieldComponents
{
    public class FieldCell : Cell
    {
        public bool IsOpen { get; set; }
        public CellTypesEnum CellType { get; set; }
        public HashSet<Guid> ShipsGuids { get; }

        public FieldCell() : this(0, 0, CellTypesEnum.Empty) { }
        public FieldCell(int i, int j) : this(i, j, CellTypesEnum.Empty) { }
        public FieldCell(int i, int j, CellTypesEnum cellType) : base(i, j)
        {
            CellType = cellType;
            IsOpen = false;
            ShipsGuids = new HashSet<Guid>();
        }

        public virtual void AddShipGuid(Guid guid)
        {
            if (CellType == CellTypesEnum.Empty && ShipsGuids.Count >= 0) return;
            if (CellType == CellTypesEnum.ShipDeck && ShipsGuids.Count >= 1) return;
            if (CellType == CellTypesEnum.NearTheShip && ShipsGuids.Count >= 4) return;
            if (!ShipsGuids.Add(guid))
            {
                throw new ArgumentException("Hashset already contains identical guid");
            }
        }

        public Cell Base => new Cell(I, J);
    }
}
