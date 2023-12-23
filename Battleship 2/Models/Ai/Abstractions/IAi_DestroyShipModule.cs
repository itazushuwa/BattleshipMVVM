using Battleship_2.Models.FieldComponents;

namespace Battleship_2.Models.Ai.Abstractions
{
    internal interface IAi_DestroyShipModule
    {
        Ai_TurnInfo DestroyShip(Cell lastOpenedCell);
        void ResetMemory();
    }
}
