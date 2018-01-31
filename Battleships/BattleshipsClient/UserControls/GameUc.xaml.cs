using BattleshipsServiceLibrary;
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

namespace BattleshipsClient.UserControls
{
    /// <summary>
    /// Interaction logic for GameUc.xaml
    /// </summary>
    public partial class GameUc : UserControl
    {
        private int gameId;

        private long playerId;

        private Rectangle toPlace;

        public bool AllBoatsPlaced
        {
            get
            {
                return AvailableBoats.Children.Cast<Rectangle>()
                    .Where(r => r.Visibility == Visibility.Visible)
                    .Count() == 0;
            }
        }

        private IEnumerable<Rectangle> boats {
            get
            {
                return AvailableBoats.Children.Cast<Rectangle>();
            }
        }

        public Action GameLeft { get; set; }

        public GameUc()
        {
            InitializeComponent();
            myBoard.Click += myBoardClick;
            opponentsBoard.Click += OpponentBoardClick;
            StartActualisation();
        }

        public void StartGame(int gameId, long playerId)
        {
            opponentsBoard.Clear();
            myBoard.Clear();
            opponentsBoard.Visibility = Visibility.Hidden;
            AvailableBoats.Visibility = Visibility.Visible;
            foreach (Rectangle r in boats)
            {
                r.Visibility = Visibility.Visible;
                r.StrokeThickness = 0;
            }
            toPlace = null;
            this.gameId = gameId;
            this.playerId = playerId;
            this.Visibility = Visibility.Visible;
        }

        private void myBoardClick(int x, int y)
        {
            //place ships
            if (toPlace != null && !AllBoatsPlaced)
            {
                int size;
                char orientation;
                GetBoatParameters(toPlace, out size, out orientation);
                using (var channelFactory = new ChannelFactory<IBattleshipsService>("MyNetNamedPipeEndpoint"))
                {
                    IBattleshipsService client = channelFactory.CreateChannel();
                    if (client.TryPutBoat(gameId, playerId, x, y, size, orientation))
                    {
                        Rectangle friend = AvailableBoats.Children.Cast<Rectangle>()
                            .Where(r => r.Width == toPlace.Height && r.Height == toPlace.Width && r.Visibility == Visibility.Visible)
                            .First();
                        toPlace.Visibility = Visibility.Hidden;
                        friend.Visibility = Visibility.Hidden;
                        toPlace = null;
                    }
                    //all ships placed
                    if (AllBoatsPlaced)
                    {
                        client.PlayerReady(gameId, playerId);
                        opponentsBoard.Visibility = Visibility.Visible;
                        AvailableBoats.Visibility = Visibility.Hidden;
                    }
                }
                updateMyBoard();
            }
        }

        internal void LeaveGame()
        {
            using (var channelFactory = new ChannelFactory<IBattleshipsService>("MyNetNamedPipeEndpoint"))
            {
                IBattleshipsService client = channelFactory.CreateChannel();
                client.LeaveGame(gameId, playerId);
            }
            this.Visibility = Visibility.Hidden;
            GameLeft();
        }

        private void OpponentBoardClick(int x, int y)
        {
            using (var channelFactory = new ChannelFactory<IBattleshipsService>("MyNetNamedPipeEndpoint"))
            {
                IBattleshipsService client = channelFactory.CreateChannel();
                BlockState? turnResult = client.MakeTurn(gameId, playerId, x, y);
                if (turnResult != null)
                {
                    opponentsBoard.SetBlock(x, y, (BlockState)turnResult);
                }
            }
        }

        private void StartActualisation()
        {
            Task.Run(() =>
            {
                using (var channelFactory = new ChannelFactory<IBattleshipsService>("MyNetNamedPipeEndpoint"))
                {
                    IBattleshipsService client = channelFactory.CreateChannel();
                    while (true)
                    {
                        try
                        {
                            GameInfo.Dispatcher.Invoke(() =>
                            {
                                GameInfo.Content = client.GetGameState(gameId, playerId);
                            });
                            updateMyBoard();
                        }
                        catch (EndpointNotFoundException)
                        {

                        }
                        Thread.Sleep(100);
                    }
                }
            });
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Rectangle)
            {
                if (toPlace != null)
                {
                    toPlace.Stroke = null;
                    toPlace.StrokeThickness = 0;
                }
                toPlace = e.OriginalSource as Rectangle;
                toPlace.Stroke = Brushes.Black;
                toPlace.StrokeThickness = 2;
            }
        }

        private void updateMyBoard()
        {
            using (var channelFactory = new ChannelFactory<IBattleshipsService>("MyNetNamedPipeEndpoint"))
            {
                IBattleshipsService client = channelFactory.CreateChannel();
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        //my board update
                        myBoard.SetBlock(x, y, client.GetMyBoardBlock(gameId, playerId, x, y));
                    }
                }
            }
        }

        private void GetBoatParameters(Rectangle boat, out int size, out char orientation)
        {
            if (boat.Width == 26)
            {
                orientation = 'V';
                size = (int)(boat.Height + 4) / 30;
            }
            else if (boat.Height == 26)
            {
                orientation = 'H';
                size = (int)(boat.Width + 4) / 30;
            }
            else
                throw new Exception();
        }
    }
}
