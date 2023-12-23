using Battleship_2.Models.FieldComponents;

namespace Battleship_2.ViewModels
{
    public interface IShipsGrid_VM
    {
        IShip_VM Decks_4 { get; }
        IShip_VM Decks_3_Num_1 { get; }
        IShip_VM Decks_3_Num_2 { get; }
        IShip_VM Decks_2_Num_1 { get; }
        IShip_VM Decks_2_Num_2 { get; }
        IShip_VM Decks_2_Num_3 { get; }
        IShip_VM Decks_1_Num_1 { get; }
        IShip_VM Decks_1_Num_2 { get; }
        IShip_VM Decks_1_Num_3 { get; }
        IShip_VM Decks_1_Num_4 { get; }

        void RefreshState(Fleet fleet);
    }
}
