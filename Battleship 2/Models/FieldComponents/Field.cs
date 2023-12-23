using System;
using System.Collections.Generic;

namespace Battleship_2.Models.FieldComponents
{
    public partial class Field
    {
        public FieldCell[,] Cells { get; set; }
        public Fleet Fleet { get; private set; }

        public Field(int fieldRows, int fieldColumns) : this(fieldRows, fieldColumns, new Fleet()) { }
        public Field(int fieldRows, int fieldColumns, Fleet fleet)
        {
            Cells = new FieldCell[fieldRows, fieldColumns];
            Fleet = fleet;

            for (int i = 0; i < fieldRows; i++)
            {
                for (int j = 0; j < fieldColumns; j++)
                {
                    FieldCell cell = new FieldCell(i, j);
                    Cells[i, j] = cell;
                }
            }
        }

        public FieldCell this[int row, int column]
        {
            get
            {
                return Cells[row, column];
            }
        }

        public void PlaceNewFleet(ref Fleet fleet)
        {
            Fleet = fleet;
            foreach (var cell in Fleet.GetAllShipsCells())
            {
                Cells[cell.I, cell.J] = cell;
            }
        }

        public void AddShip(Ship ship)
        {
            Fleet.AddShip(ref ship);
            for (int i = 0; i < ship.Cells.Count; i++)
            {
                FieldCell cell = ship.Cells[i];
                Cells[cell.I, cell.J] = cell;
            }
        }

        public void OpenCell(Cell cell)
        {
            FieldCell fieldCell = Cells[cell.I, cell.J];
            if (fieldCell.IsOpen) throw new ArgumentException("Cell was already open.");
            fieldCell.IsOpen = true;
        }

        public void OpenRange(Cell[] cells)
        {
            foreach (var cell in cells)
            {
                if (!Cells[cell.I, cell.J].IsOpen)
                {
                    Cells[cell.I, cell.J].IsOpen = true;
                }
            }
        }

        public bool IsShipDestroyed(Guid shipGuid)
        {
            return Fleet.GetShip(shipGuid).IsDestroyed;
        }

        public void OpenCellsAroundShip(Guid shipGuid)
        {
            for (int i = 0; i < FieldRows; i++)
            {
                for (int j = 0; j < FieldColumns; j++)
                {
                    var cell = Cells[i, j];
                    if (cell.ShipsGuids.Contains(shipGuid))
                    {
                        cell.IsOpen = true;
                    }
                }
            }
        }
    }



    public partial class Field
    {
        public static readonly int FieldRows = 10;
        public static readonly int FieldColumns = 10;

        public static readonly Predicate<Cell> FieldBoundsCheck = c =>
        c.I >= 0
        && c.J >= 0
        && c.I < FieldRows
        && c.J < FieldColumns;

        public static List<Cell> GetNearbyCells(Cell cell)
        {
            var functions = new List<Func<int, int, Cell>>
            {
                (i, j) => new Cell(i - 1, j - 1),
                (i, j) => new Cell(i - 1, j),
                (i, j) => new Cell(i - 1, j + 1),

                (i, j) => new Cell(i, j - 1),
                (i, j) => new Cell(i, j + 1),

                (i, j) => new Cell(i + 1, j - 1),
                (i, j) => new Cell(i + 1, j),
                (i, j) => new Cell(i + 1, j + 1),
            };

            var cells = new List<Cell>();
            foreach (var function in functions)
            {
                cells.Add(function.Invoke(cell.I, cell.J));
            }

            cells.RemoveAll(t => !FieldBoundsCheck(t));
            return cells;
        }
    }
}
