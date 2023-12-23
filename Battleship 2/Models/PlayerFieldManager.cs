using Battleship_2.Models.FieldComponents;
using Battleship_2.Models.FieldComponents.Enumerations;
using System.Linq;

namespace Battleship_2.Models
{
    internal class PlayerFieldManager : IFieldManager
    {
        private Field _field;
        public Field Field => _field;

        public PlayerFieldManager(Field field)
        {
            _field = field;
        }

        public bool Shoot(Cell selectedCell)
        {
            FieldCell fieldCell = _field.Cells[selectedCell.I, selectedCell.J];
            _field.OpenCell(fieldCell);

            if (fieldCell.CellType == CellTypesEnum.ShipDeck)
            {
                if (_field.Fleet.GetShip(fieldCell.ShipsGuids.First()).IsDestroyed)
                {
                    _field.OpenCellsAroundShip(fieldCell.ShipsGuids.First());
                }
                return true;
            }
            return false;
        }
    }
}
