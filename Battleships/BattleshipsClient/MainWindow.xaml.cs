using BattleshipsClient.Model;
using BattleshipsClient.Windows;
using BattleshipsServiceLibrary;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
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

namespace BattleshipsClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PlayerContract me;

        public MainWindow()
        {
            InitializeComponent();
            me = LoginWindow.Run();
            Title = $"Battleships {me.Name}";
            MainMenuUc.FindGame = FindGame;
            MainMenuUc.StartGame = StartGame;
            MainMenuUc.JoinGame = JoinGame;
            MainMenuUc.Stats = DisplayStats;
            MainMenuUc.Logout = Relog;
            GameUc.GameLeft = DisplayMenu;
        }

        private void JoinGame()
        {
            int gameId;
            if (!int.TryParse(Interaction.InputBox("Join Game", "Set game id number value"), out gameId))
            {
                MessageBox.Show("Game id must be a number...");
                return;
            }
            using (var channelFactory = new ChannelFactory<IBattleshipsService>("MyNetNamedPipeEndpoint"))
            {
                IBattleshipsService client = channelFactory.CreateChannel();
                Game foundGame = client.JoinGame(me, gameId);
                if (foundGame == null)
                {
                    MessageBox.Show($"Game with id {gameId} not found");
                    return;
                }
                MainMenuUc.Visibility = Visibility.Hidden;
                GameUc.StartGame(foundGame.GameId, me.PlayerId);
            }
        }

        private void StartGame()
        {
            using (var channelFactory = new ChannelFactory<IBattleshipsService>("MyNetNamedPipeEndpoint"))
            {
                IBattleshipsService client = channelFactory.CreateChannel();
                Game foundGame = client.StartGame(me);
                MainMenuUc.Visibility = Visibility.Hidden;
                GameUc.StartGame(foundGame.GameId, me.PlayerId);
            }
        }

        private void FindGame()
        {
            using (var channelFactory = new ChannelFactory<IBattleshipsService>("MyNetNamedPipeEndpoint"))
            {
                IBattleshipsService client = channelFactory.CreateChannel();
                Game foundGame = client.FindGame(me);
                MainMenuUc.Visibility = Visibility.Hidden;
                GameUc.StartGame(foundGame.GameId, me.PlayerId);
            }
        }

        private void DisplayMenu()
        {
            MainMenuUc.Visibility = Visibility.Visible;
        }

        private void DisplayStats()
        {
            StatsUc.Display(me.PlayerId);
        }

        private void Relog()
        {
            me = LoginWindow.Run();
            Title = $"Battleships {me.Name}";
            MainMenuUc.Visibility = Visibility.Visible;
        }
    }
}
