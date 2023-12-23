using Battleship_2.Accessories;
using Battleship_2.Command;
using Battleship_2.Models;
using Battleship_2.Models.Ai;
using Battleship_2.Models.FieldComponents;
using Battleship_2.Models.FieldComponents.Enumerations;
using Battleship_2.ViewModels.Abstractions;
using Battleship_2.Views;
using Battleship_2.ViewModels.PagesViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System;

namespace Battleship_2.ViewModels
{
    internal class PvE_GameManager_VM : INotifyPropertyChanged, IGameManager_VM
    {
        private static readonly int DelayBeforeWin = 2000;

        private PvE_GameManager _gameManager;
        private List<Cell_VM> _leftField;
        private List<Cell_VM> _rightField;
        private readonly AutoEventCommandBase _shootCommand;
        public event PropertyChangedEventHandler? PropertyChanged;

        public PvE_GameManager_VM(Page pageForNavigationService)
        {
            var autoPlacer = new ShipsAutoPlacer();
            var aiFieldManager = new SimpleAiFieldManager(autoPlacer.GenerateField());
            var playerFieldManager = new PlayerFieldManager(autoPlacer.GenerateField());
            _gameManager = new PvE_GameManager(aiFieldManager, playerFieldManager);

            ShipsImagesManager imagesManager = new ShipsImagesManager();
            LeftFieldShips = new ShipsGrid_VM(imagesManager.GetFirstShipsSet(), _gameManager.PlayerFleet, DirectionsEnum.Left);
            RightFieldShips = new ShipsGrid_VM(imagesManager.GetSecondShipsSet(), _gameManager.AiFleet, DirectionsEnum.Right);

            _leftField = new List<Cell_VM>();
            _rightField = new List<Cell_VM>();
            RefreshLeftField();
            RefreshRightField();

            _gameManager.PlayerWin += NavigateToPlayerWinPage;
            _gameManager.AiWin += NavigateToAiWinPage;
            _gameManager.PlayerFieldChanged += RefreshLeftField;
            _gameManager.AiFieldChanged += RefreshRightField;

            _shootCommand = new AutoEventCommandBase(o => Shoot(o), o => CanShoot(o));
            PageForNavigationService = pageForNavigationService;
        }




        public List<Cell_VM> LeftField
        {
            get => _leftField;
            private set => SetProperty(ref _leftField, value, nameof(LeftField));
        }
        public List<Cell_VM> RightField
        {
            get => _rightField;
            private set => SetProperty(ref _rightField, value, nameof(RightField));
        }

        public IShipsGrid_VM RightFieldShips { get; }
        public IShipsGrid_VM LeftFieldShips { get; }

        public AutoEventCommandBase ShootCommand => _shootCommand;
        public Page PageForNavigationService { get; set; }



        private async void Shoot(object parameter)
        {
            try
            {
                var cell = (Cell_VM)parameter;
                int indexOfCell = RightField.IndexOf(cell);
                int rows = indexOfCell / Field.FieldRows;
                int columns = indexOfCell % Field.FieldColumns;
                await Task.Run(() => _gameManager.PlayerShoot(new Cell(rows, columns)));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private bool CanShoot(object parameter)
        {
            try
            {
                var cell = (Cell_VM)parameter;
                return !cell.IsOpen && cell.IsCellActive;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }

        private void NavigateToAiWinPage()
        {
            Thread.Sleep(DelayBeforeWin);
            PageForNavigationService.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    try
                    {
                        var viewModel = new WinPage_VM(
                        Application.Current.Resources["PlayerLoseMessage"] as string ?? "Error",
                        Application.Current.Resources["LoseTextBrush"] as Brush ?? Brushes.Gray);
                        var winPage = new WinPage(viewModel);
                        PageForNavigationService.NavigationService.Navigate(winPage);
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex);
                    }
                });
        }

        private void NavigateToPlayerWinPage()
        {
            Thread.Sleep(DelayBeforeWin);
            PageForNavigationService.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    try
                    {
                        var viewModel = new WinPage_VM(
                        Application.Current.Resources["PlayerWinMessage"] as string ?? "Error",
                        Application.Current.Resources["WinTextBrush"] as Brush ?? Brushes.Gray);
                        var winPage = new WinPage(viewModel);
                        PageForNavigationService.NavigationService.Navigate(winPage);
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex);
                    }
                });
        }

        private void RefreshLeftField()
        {
            try
            {
                int totalNumberOfCells = Field.FieldRows * Field.FieldColumns;
                var playerCellsList = new List<Cell_VM>(totalNumberOfCells);

                for (int i = 0; i < Field.FieldRows; i++)
                {
                    for (int j = 0; j < Field.FieldColumns; j++)
                    {
                        var playerCell = new Cell_VM(_gameManager.PlayerField[i, j].CellType == CellTypesEnum.ShipDeck)
                        {
                            IsOpen = _gameManager.PlayerField[i, j].IsOpen
                        };
                        if (playerCell.IsShipDeck)
                        {
                            playerCell.IsShipDestroyed = _gameManager.PlayerFleet.GetShip(_gameManager.PlayerField[i, j].ShipsGuids.First()).IsDestroyed;
                        }
                        playerCellsList.Add(playerCell);
                    }
                }
                LeftField = playerCellsList;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void RefreshRightField()
        {
            try
            {
                int totalNumberOfCells = Field.FieldRows * Field.FieldColumns;
                var aiCellsList = new List<Cell_VM>(totalNumberOfCells);

                for (int i = 0; i < Field.FieldRows; i++)
                {
                    for (int j = 0; j < Field.FieldColumns; j++)
                    {

                        var aiCell = new Cell_VM(_gameManager.AiField[i, j].CellType == CellTypesEnum.ShipDeck)
                        {
                            IsOpen = _gameManager.AiField[i, j].IsOpen,
                            IsCellActive = _gameManager.IsPlayerTurn
                        };
                        if (aiCell.IsShipDeck)
                        {
                            aiCell.IsShipDestroyed = _gameManager.AiFleet.GetShip(_gameManager.AiField[i, j].ShipsGuids.First()).IsDestroyed;
                        }
                        aiCellsList.Add(aiCell);
                    }
                }
                RightField = aiCellsList;

                RightFieldShips.RefreshState(_gameManager.AiFleet);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void SetProperty<T>(ref T oldValue, T newValue, string propertyName)
        {
            if (!oldValue?.Equals(newValue) ?? newValue != null)
            {
                oldValue = newValue;

                NotifyPropertyChanged(propertyName);
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void HandleException(Exception ex)
        {
            MessageBox.Show(ex.Message);
            Application.Current.Shutdown();
        }
    }
}
