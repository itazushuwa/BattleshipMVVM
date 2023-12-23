using Battleship_2.Command;
using Battleship_2.ViewModels.Abstractions;
using Battleship_2.Views;
using System.Windows;
using System;
using System.Windows.Controls;

namespace Battleship_2.ViewModels.PagesViewModels
{
    internal class PvE_GamePage_VM : IGamePage_VM
    {
        private readonly IGameManager_VM _gameManager;
        private readonly AutoEventCommandBase _pauseCommand;

        public PvE_GamePage_VM(Page pageForNavigationService)
        {
            _gameManager = new PvE_GameManager_VM(pageForNavigationService);
            _pauseCommand = new AutoEventCommandBase(o => Pause(o), _ => true);
        }

        public IGameManager_VM GameManager => _gameManager;
        public AutoEventCommandBase PauseCommand => _pauseCommand;

        private void Pause(object o)
        {
            try
            {
                var page = (Page)o;
                page.NavigationService.Navigate(new PausePage());
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void HandleException(Exception ex)
        {
            MessageBox.Show(ex.Message);
            Application.Current.Shutdown();
        }
    }
}
