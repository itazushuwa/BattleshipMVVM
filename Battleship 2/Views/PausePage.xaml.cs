using Battleship_2.ViewModels.PagesViewModels;
using System.Windows.Controls;

namespace Battleship_2.Views
{
    public partial class PausePage : Page
    {
        public PausePage()
        {
            InitializeComponent();

            DataContext = new PausePage_VM();
        }
    }
}
