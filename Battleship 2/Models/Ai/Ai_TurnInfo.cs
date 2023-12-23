using Battleship_2.Models.FieldComponents;

namespace Battleship_2.Models.Ai
{
    internal class Ai_TurnInfo
    {
        public Cell LastOpenedCell { get; set; } = Cell.NotValid;
        public bool WasShipDestroyed { get; set; }
        public bool WasShotSuccessfull { get; set; }
    }
}
