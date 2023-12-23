using Battleship_2.Command;
using Battleship_2.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Battleship_2.ViewModels.PagesViewModels
{
    public class WinPage_VM
    {
        private readonly AutoEventCommandBase _toMainMenuCommand;
        private readonly AutoEventCommandBase _exitCommand;

        public WinPage_VM(string message, Brush messageBrush)
        {
            Message = message;
            MessageBrush = messageBrush;

            _toMainMenuCommand = new AutoEventCommandBase(o => ToMainMenu(o), _ => true);
            _exitCommand = new AutoEventCommandBase(_ => Exit(), _ => true);
        }

        public string Message { get; }
        public Brush MessageBrush { get; }

        public AutoEventCommandBase ToMainMenuCommand => _toMainMenuCommand;
        public AutoEventCommandBase ExitCommand => _exitCommand;

        private void ToMainMenu(object o)
        {
            try
            {
                var page = (Page)o;
                var mainMenu = new MainPage();
                page.NavigationService.Navigate(mainMenu);
            }
            catch(Exception ex)
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
    }
}
