using Battleship_2.Command;
using Battleship_2.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Battleship_2.ViewModels.PagesViewModels
{
    public class MainPage_VM
    {
        private readonly AutoEventCommandBase _exitCommand;
        private readonly AutoEventCommandBase _startGameCommand;

        public MainPage_VM()
        {
            _startGameCommand = new AutoEventCommandBase(o => StartGame(o), _ => true);
            _exitCommand = new AutoEventCommandBase(_ => Exit(), _ => true);
        }

        public AutoEventCommandBase ExitCommand => _exitCommand;
        public AutoEventCommandBase StartGameCommand => _startGameCommand;

        private void StartGame(object parameter)
        {
            try
            {
                var page = (Page)parameter;
                ClearNavigationHistory(page);

                var gamePage = new GamePage();
                var viewModel = new PvE_GamePage_VM(gamePage);
                gamePage.DataContext = viewModel;
                page.NavigationService.Navigate(gamePage);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        private void HandleException(Exception ex)
        {
            MessageBox.Show(ex.Message);
            Application.Current.Shutdown();
        }

        private void ClearNavigationHistory(Page page)
        {
            while (page.NavigationService.CanGoBack)
            {
                try { page.NavigationService.RemoveBackEntry(); }
                catch (Exception) { break; }
            }
        }
    }
}
