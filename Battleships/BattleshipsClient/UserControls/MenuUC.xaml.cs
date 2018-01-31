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
    /// Interaction logic for MenuUC.xaml
    /// </summary>
    public partial class MenuUC : UserControl
    {
        public Action FindGame { get; set; }
        public Action StartGame { get; set; }
        public Action JoinGame { get; set; }
        public Action Logout { get; set; }
        public Action Stats { get; set; }

        public MenuUC()
        {
            InitializeComponent();
        }

        private void FindGameClick()
        {
            this.Visibility = Visibility.Hidden;
            FindGame();
        }

        private void StartGameClick()
        {
            this.Visibility = Visibility.Hidden;
            StartGame();
        }

        private void JoinGameClick()
        {
            this.Visibility = Visibility.Hidden;
            JoinGame();
        }

        private void LogoutClick()
        {
            Logout();
        }

        private void StatsClick()
        {
            Stats();
        }
    }
}
