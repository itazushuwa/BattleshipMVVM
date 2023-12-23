using Battleship_2.Models.FieldComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using Battleship_2.Exceptions;
using Battleship_2.Models.Ai.Abstractions;
using Battleship_2.Models.FieldComponents.Enumerations;

namespace Battleship_2.Models.Ai
{
    internal class Ai_SimpleDestroyShipModule : IAi_DestroyShipModule
    {
        private Random _random;
        private Field _field;
        private List<Cell> _openedShipCells;
        private Cell _lastOpenedCell;
        private OrientationsEnum _shipOrientation;
        private (DirectionsEnum direction, bool isChecked)[] _checkedDirections;

        public Field Field { get => _field; }

        public Ai_SimpleDestroyShipModule(Field field)
        {
            _random = new Random();
            _field = field;
            _openedShipCells = new List<Cell>(Fleet.ShipsDecks.Max());
            _lastOpenedCell = Cell.NotValid;
            _shipOrientation = OrientationsEnum.Unknown;
            _checkedDirections = new[]
            {
                (DirectionsEnum.Left, false),
                (DirectionsEnum.Right, false),
                (DirectionsEnum.Up, false),
                (DirectionsEnum.Down, false)
            };
        }

        public Ai_TurnInfo DestroyShip(Cell lastOpenedCell)
        {
            if (_shipOrientation == OrientationsEnum.Unknown)
            {
                if (_openedShipCells.Count == 0)
                {
                    _openedShipCells.Add(lastOpenedCell);
                }

                return FindSecondCell();
            }
            else
            {
                return FindLeftCells();
            }
        }

        private Ai_TurnInfo FindSecondCell()
        {
            while (true)
            {
                var availableDirections = _checkedDirections.Where(d => !d.isChecked);
                var count = availableDirections.Count();

                if (count == 0) throw new AiException("AI checked every direction");

                var direction = availableDirections.ElementAt(_random.Next(count)).direction;
                var nextCell = GetNextCell(direction, _openedShipCells.First());

                if (Field.FieldBoundsCheck(nextCell) && !Field[nextCell.I, nextCell.J].IsOpen)
                {
                    var info = OpenCellAndMakeReport(nextCell);
                    _lastOpenedCell = nextCell;

                    if (!info.WasShotSuccessfull)
                    {
                        MarkCheckedDirection(direction);
                    }
                    else if (info.WasShipDestroyed)
                    {
                        ResetMemory();
                    }
                    else
                    {
                        _openedShipCells.Add(nextCell);
                        _shipOrientation = direction.DirectionToOrientation();
                    }

                    return info;
                }
                else
                {
                    MarkCheckedDirection(direction);
                }
            }
        }

        private Ai_TurnInfo FindLeftCells()
        {
            if (_shipOrientation == OrientationsEnum.Unknown) throw new AiException("Orientation of the ship was not found out");

            Cell firstPossibleCell;
            Cell secondPossibleCell;

            if (_shipOrientation == OrientationsEnum.Horizontal)
            {
                _openedShipCells.Sort((c1, c2) => c1.J.CompareTo(c2.J));
                firstPossibleCell = GetNextCell(DirectionsEnum.Left, _openedShipCells.First());
                secondPossibleCell = GetNextCell(DirectionsEnum.Right, _openedShipCells.Last());
            }
            else
            {
                _openedShipCells.Sort((c1, c2) => c1.I.CompareTo(c2.I));
                firstPossibleCell = GetNextCell(DirectionsEnum.Up, _openedShipCells.First());
                secondPossibleCell = GetNextCell(DirectionsEnum.Down, _openedShipCells.Last());
            }

            var isFirstCellAlowed = Field.FieldBoundsCheck(firstPossibleCell)
                && !Field[firstPossibleCell.I, firstPossibleCell.J].IsOpen;
            var isSecondCellAlowed = Field.FieldBoundsCheck(secondPossibleCell)
                && !Field[secondPossibleCell.I, secondPossibleCell.J].IsOpen;

            var info = new Ai_TurnInfo();

            if (isFirstCellAlowed && isSecondCellAlowed)
            {
                _lastOpenedCell = Convert.ToBoolean(_random.Next(2)) ? firstPossibleCell : secondPossibleCell;
            }
            else if (isFirstCellAlowed)
            {
                _lastOpenedCell = firstPossibleCell;
            }
            else if (isSecondCellAlowed)
            {
                _lastOpenedCell = secondPossibleCell;
            }
            else
            {
                throw new AiException("AI checked every direction");
            }

            info = OpenCellAndMakeReport(_lastOpenedCell);

            if (info.WasShipDestroyed)
            {
                ResetMemory();
            }
            else if (info.WasShotSuccessfull)
            {
                _openedShipCells.Add(info.LastOpenedCell);
            }

            return info;
        }

        private Ai_TurnInfo OpenCellAndMakeReport(Cell cell)
        {
            Field.OpenCell(cell);
            var fieldCell = Field[cell.I, cell.J];
            var info = new Ai_TurnInfo();

            if (fieldCell.CellType != CellTypesEnum.ShipDeck)
            {
                info.WasShotSuccessfull = false;
                info.WasShipDestroyed = false;
                info.LastOpenedCell = cell;
            }
            else if (Field.IsShipDestroyed(fieldCell.ShipsGuids.First()))
            {
                info.WasShotSuccessfull = true;
                info.WasShipDestroyed = true;
                info.LastOpenedCell = cell;

                _field.OpenCellsAroundShip(fieldCell.ShipsGuids.First());
            }
            else
            {
                info.WasShotSuccessfull = true;
                info.WasShipDestroyed = false;
                info.LastOpenedCell = cell;
            }

            return info;
        }

        private void MarkCheckedDirection(DirectionsEnum direction)
        {
            for (int i = 0; i < _checkedDirections.Length; i++)
            {
                if (_checkedDirections[i].direction == direction)
                {
                    _checkedDirections[i].isChecked = true;
                }
            }
        }

        private Cell GetNextCell(DirectionsEnum direction, Cell cell) => direction switch
        {
            DirectionsEnum.Left => new Cell(cell.I, cell.J - 1),
            DirectionsEnum.Right => new Cell(cell.I, cell.J + 1),
            DirectionsEnum.Up => new Cell(cell.I - 1, cell.J),
            DirectionsEnum.Down => new Cell(cell.I + 1, cell.J),
            _ => cell
        };

        public void ResetMemory()
        {
            _shipOrientation = OrientationsEnum.Unknown;
            _openedShipCells.Clear();
            _lastOpenedCell = Cell.NotValid;
            for (int i = 0; i < _checkedDirections.Length; i++)
            {
                _checkedDirections[i].isChecked = false;
            }
        }
    }
}
