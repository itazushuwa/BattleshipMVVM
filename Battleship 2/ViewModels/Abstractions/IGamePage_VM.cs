using Battleship_2.Command;
using Battleship_2.ViewModels.Abstractions;

namespace Battleship_2.ViewModels
{
    public interface IGamePage_VM
    {
        IGameManager_VM GameManager { get; }
        AutoEventCommandBase PauseCommand { get; }
    }
}
