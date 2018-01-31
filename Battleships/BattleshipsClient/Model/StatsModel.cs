using BattleshipsServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsClient.Model
{
    public class StatsModel
    {
        public PlayerModel Me { get; private set; }

        public List<PlayerModel> Players { get; private set; }

        public List<GameModel> Games { get; private set; }

        public StatsModel(long myId)
        {
            Players = new List<PlayerModel>();
            using (var channelFactory = new ChannelFactory<IBattleshipsService>("MyNetNamedPipeEndpoint"))
            {
                IBattleshipsService client = channelFactory.CreateChannel();
                foreach (StatsContract pc in client.GetStats())
                    Players.Add(new PlayerModel()
                    {
                        Id = pc.Player.PlayerId,
                        Name = pc.Player.Name,
                        Wins = pc.Wins,
                        Loses = pc.Loses
                    });
            }
            Players = Players.OrderByDescending(p => p.Loses == 0 ? p.Wins : (double)p.Wins / p.Loses).ToList();
            foreach (PlayerModel pm in Players)
                pm.Rank = Players.IndexOf(pm) + 1;
            Me = Players.Find(p => p.Id == myId);

            //init games 
            Games = new List<GameModel>();
            using (var channelFactory = new ChannelFactory<IBattleshipsService>("MyNetNamedPipeEndpoint"))
            {
                IBattleshipsService client = channelFactory.CreateChannel();
                foreach (GameResult gc in client.GetMyGames(myId))
                    Games.Add(new GameModel()
                    {
                        GameId = gc.GameId,
                        End = gc.End,
                        Start = gc.Start,
                        Opponent = gc.Opponent,
                        Result = gc.Result
                    });
            }
        }
    }
}
