using Battleship_2.ViewModels.PagesViewModels;
using System.Windows.Controls;

namespace Battleship_2.Views
{
    public partial class WinPage : Page
    {
        public WinPage(WinPage_VM winPageViewModel)
        {
            InitializeComponent();

            DataContext = winPageViewModel;
        }
    }
}
