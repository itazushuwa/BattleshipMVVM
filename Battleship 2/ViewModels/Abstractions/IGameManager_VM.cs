using Battleship_2.Command;
using System.Collections.Generic;

namespace Battleship_2.ViewModels.Abstractions
{
    public interface IGameManager_VM
    {
        public IShipsGrid_VM LeftFieldShips { get; }
        public IShipsGrid_VM RightFieldShips { get; }
        public List<Cell_VM> LeftField { get; }
        public List<Cell_VM> RightField { get; }
        public AutoEventCommandBase ShootCommand { get; }
    }
}
