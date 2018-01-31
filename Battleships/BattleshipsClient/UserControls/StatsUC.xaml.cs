using BattleshipsClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BattleshipsClient.UserControls
{
    /// <summary>
    /// Interaction logic for StatsUC.xaml
    /// </summary>
    public partial class StatsUC : UserControl
    {
        public StatsUC()
        {
            InitializeComponent();
        }

        private void MainMenuClick()
        {
            this.Visibility = Visibility.Hidden;
        }

        public void Display(long player)
        {
            this.DataContext = new StatsModel(player);
            Visibility = Visibility.Visible;
        }

        private void ReplayClick(object sender, RoutedEventArgs e)
        {
            ReplayModeUc.Run(((GameModel)((Button)sender).DataContext).GameId);
        }
    }
}
