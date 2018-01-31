using BattleshipsServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
    /// Interaction logic for ReplayModeUC.xaml
    /// </summary>
    public partial class ReplayModeUC : UserControl
    {
        private PlayerContract player1;
        private PlayerContract player2;
        private List<Move> moves;

        public ReplayModeUC()
        {
            InitializeComponent();
            moves = new List<Move>();
        }

        public void Run(long gameId)
        {
            Player1Board.Clear();
            Player2Board.Clear();
            moves.Clear();
            player1 = null;
            player2 = null;
            using (var channelFactory = new ChannelFactory<IBattleshipsService>("MyNetNamedPipeEndpoint"))
            {
                IBattleshipsService client = channelFactory.CreateChannel();
                GameReplay gr = client.GetGameReplay(gameId);
                if (gr == null)
                    return;
                player1 = gr.Player1;
                player2 = gr.Player2;
                moves = gr.Moves.OrderBy(m => m.Time).ToList();
            }
            updateButtons();
            Player1Button.MainText = $"Player {player1.Name} turn...";
            Player2Button.MainText = $"Player {player2.Name} turn...";
            Visibility = Visibility.Visible;
        }

        private void updateButtons()
        {
            Player1Button.IsEnabled = false;
            Player2Button.IsEnabled = false;
            if (moves.Count == 0)
                return;
            if (moves.First().Player == player1.PlayerId)
                Player1Button.IsEnabled = true;
            else if (moves.First().Player == player2.PlayerId)
                Player2Button.IsEnabled = true;
            else throw new Exception();
        }

        private void Player1Click()
        {
            Move m = moves.First();
            moves.RemoveAt(0);
            Player1Board.SetBlock(m.AtX, m.AtY, m.Result);
            updateButtons();
        }

        private void Player2Click()
        {
            Move m = moves.First();
            moves.RemoveAt(0);
            Player2Board.SetBlock(m.AtX, m.AtY, m.Result);
            updateButtons();
        }

        private void ExitClick()
        {
            Visibility = Visibility.Hidden;
        }
    }
}
