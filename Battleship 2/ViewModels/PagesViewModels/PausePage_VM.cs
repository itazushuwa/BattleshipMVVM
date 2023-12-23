using Battleship_2.Command;
using Battleship_2.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Battleship_2.ViewModels.PagesViewModels
{
    internal class PausePage_VM
    {
        private readonly AutoEventCommandBase _resumeCommand;
        private readonly AutoEventCommandBase _toMainMenuCommand;
        private readonly AutoEventCommandBase _exitCommand;

        public PausePage_VM()
        {
            _resumeCommand = new AutoEventCommandBase(o => Resume(o), _ => true);
            _toMainMenuCommand = new AutoEventCommandBase(o => ToMainMenu(o), _ => true);
            _exitCommand = new AutoEventCommandBase(_ => Exit(), _ => true);
        }

        public AutoEventCommandBase ResumeCommand => _resumeCommand;
        public AutoEventCommandBase ToMainMenuCommand => _toMainMenuCommand;
        public AutoEventCommandBase ExitCommand => _exitCommand;

        private void Resume(object o)
        {
            try
            {
                var page = (Page)o;
                page.NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ToMainMenu(object o)
        {
            try
            {
                var page = (Page)o;
                page.NavigationService.Navigate(new MainPage());
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
    }
}
