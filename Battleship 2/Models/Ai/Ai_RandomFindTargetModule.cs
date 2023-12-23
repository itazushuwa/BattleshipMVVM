using Battleship_2.Models.Ai.Abstractions;
using Battleship_2.Models.FieldComponents;
using Battleship_2.Models.FieldComponents.Enumerations;
using System.Linq;

namespace Battleship_2.Models.Ai
{
    internal class Ai_RandomFindTargetModule : IAi_FindTargetModule
    {
        private Field _field;
        public Field Field { get => _field; }

        public Ai_RandomFindTargetModule(Field field)
        {
            _field = field;
        }

        public Ai_TurnInfo FindTarget()
        {
            Ai_TurnInfo info = new Ai_TurnInfo();
            while (true)
            {
                var randomCell = Cell.RandomCell;
                FieldCell fieldCell = Field.Cells[randomCell.I, randomCell.J];
                if (!fieldCell.IsOpen)
                {
                    fieldCell.IsOpen = true;
                    info.LastOpenedCell = fieldCell.Base;

                    if (fieldCell.CellType == CellTypesEnum.ShipDeck
                        && !Field.Fleet.GetShip(fieldCell.ShipsGuids.First()).IsDestroyed)
                    {
                        info.WasShotSuccessfull = true;
                        info.WasShipDestroyed = false;
                    }
                    else if (fieldCell.CellType == CellTypesEnum.ShipDeck)
                    {
                        _field.OpenCellsAroundShip(fieldCell.ShipsGuids.First());
                        info.WasShotSuccessfull = true;
                        info.WasShipDestroyed = true;
                    }
                    else
                    {
                        info.WasShotSuccessfull = false;
                        info.WasShipDestroyed = false;
                    }

                    return info;
                }
            }
        }
    }
}
