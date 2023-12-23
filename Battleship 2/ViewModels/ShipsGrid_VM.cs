using Battleship_2.Exceptions;
using Battleship_2.Models.FieldComponents;
using Battleship_2.Models.FieldComponents.Enumerations;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

namespace Battleship_2.ViewModels
{
    class ShipsGrid_VM : IShipsGrid_VM, INotifyPropertyChanged
    {
        private readonly IShip_VM[] _shipViewModels;
        private readonly DirectionsEnum _location;

        public event PropertyChangedEventHandler? PropertyChanged;

        public IShip_VM Decks_4 => _shipViewModels[0];

        public IShip_VM Decks_3_Num_1 => _shipViewModels[1];

        public IShip_VM Decks_3_Num_2 => _shipViewModels[2];

        public IShip_VM Decks_2_Num_1 => _shipViewModels[3];

        public IShip_VM Decks_2_Num_2 => _shipViewModels[4];

        public IShip_VM Decks_2_Num_3 => _shipViewModels[5];

        public IShip_VM Decks_1_Num_1 => _shipViewModels[6];

        public IShip_VM Decks_1_Num_2 => _shipViewModels[7];

        public IShip_VM Decks_1_Num_3 => _shipViewModels[8];

        public IShip_VM Decks_1_Num_4 => _shipViewModels[9];

        public ShipsGrid_VM(string[] shipImagesUri, Fleet fleet, DirectionsEnum gridLocation)
        {
            _location = gridLocation;
            int[] decks = Fleet.ShipsDecks;
            fleet.Ships.Sort((s1, s2) => s2.Cells.Count.CompareTo(s1.Cells.Count));
            _shipViewModels = new Ship_VM[decks.Length];

            if (shipImagesUri.Length != 4) throw new ArgumentException("Wrong number of images");
            if (gridLocation == DirectionsEnum.Up || gridLocation == DirectionsEnum.Down) throw new ArgumentException("Grid location can be only left or right");
            if (decks.Length != 10) throw new ShipsGridViewModelException("Class [ShipsGridViewModel] can only be used when [LogicAccessories.NumberOfShipsDecks.Length] equals 10");

            for (int i = 0; i < _shipViewModels.Length; i++)
            {
                var shipVm = new Ship_VM();
                var fleetShip = fleet.Ships[i];
                shipVm.ShipImageUri = shipImagesUri[decks[i] - 1];

                if (gridLocation == DirectionsEnum.Left) shipVm.IsVisible = true;
                if (gridLocation == DirectionsEnum.Right) shipVm.IsVisible = false;

                if (fleetShip.Orientation == OrientationsEnum.Horizontal)
                {
                    if (gridLocation == DirectionsEnum.Left) shipVm.Rotation = new RotateTransform(90);
                    if (gridLocation == DirectionsEnum.Right) shipVm.Rotation = new RotateTransform(270);

                    shipVm.ColumnSpan = fleetShip.Cells.Count;
                    fleetShip.Cells.Sort((s1, s2) => s1.J.CompareTo(s2.J));
                }
                else
                {
                    shipVm.RowSpan = fleetShip.Cells.Count;
                    fleetShip.Cells.Sort((s1, s2) => s1.I.CompareTo(s2.I));
                }

                shipVm.Row = fleetShip.Cells.First().I;
                shipVm.Column = fleetShip.Cells.First().J;

                _shipViewModels[i] = shipVm;
            }
        }

        public void RefreshState(Fleet fleet)
        {
            if (_location == DirectionsEnum.Right)
            {
                var ships = fleet.Ships;
                for (int i = 0; i < ships.Count; i++)
                {
                    if (ships[i].IsDestroyed != _shipViewModels[i].IsVisible)
                    {
                        _shipViewModels[i].IsVisible = ships[i].IsDestroyed;
                    }
                }
            }
            NotifyAllPropertiesChanged();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void NotifyAllPropertiesChanged()
        {
            NotifyPropertyChanged(nameof(Decks_1_Num_1));
            NotifyPropertyChanged(nameof(Decks_1_Num_2));
            NotifyPropertyChanged(nameof(Decks_1_Num_3));
            NotifyPropertyChanged(nameof(Decks_1_Num_4));
            NotifyPropertyChanged(nameof(Decks_2_Num_1));
            NotifyPropertyChanged(nameof(Decks_2_Num_2));
            NotifyPropertyChanged(nameof(Decks_2_Num_3));
            NotifyPropertyChanged(nameof(Decks_3_Num_1));
            NotifyPropertyChanged(nameof(Decks_3_Num_2));
            NotifyPropertyChanged(nameof(Decks_4));
        }
    }
}
