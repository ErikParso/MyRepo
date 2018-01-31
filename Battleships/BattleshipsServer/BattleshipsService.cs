using BattleshipsDatabase;
using BattleshipsServiceLibrary;
using System;
using System.Linq;
using System.ServiceModel;
using System.Collections.Generic;

namespace BattleshipsServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class BattleshipsService : IBattleshipsService
    {
        public GameCenter GameCenter { get; private set; }

        public BattleshipsService()
        {
            GameCenter = new GameCenter();
            GameCenter.GameEnded = SaveGame;
        }

        public PlayerContract Login(string name, string password)
        {
            using (BattleshipsDbEntities bdc = new BattleshipsDbEntities())
            {
                return bdc.Players.AsEnumerable()
                    .Where(p => p.Name == name && p.Password == password)
                    .Select(p => new PlayerContract(p.PlayerId, p.Name))
                    .FirstOrDefault();
            }
        }

        private void SaveGame(BattleshipsServiceLibrary.Game game)
        {
            using (BattleshipsDbEntities bdc = new BattleshipsDbEntities())
            {
                BattleshipsDatabase.Game g = new BattleshipsDatabase.Game();
                g.End = DateTime.Now;
                g.Winner = game.Winner.PlayerId;
                g.Loser = game.Opponent(g.Winner).PlayerId;
                g.Start = game.Start;
                foreach (Move m in game.Moves)
                {
                    bdc.Turns.Add(new Turn()
                    {
                        Game = g.GameId,
                        Player = m.Player,
                        Time = m.Time,
                        Hit = m.Result == BlockState.HIT,
                        X = m.AtX,
                        Y = m.AtY
                    });
                }
                bdc.Games.Add(g);
                bdc.SaveChanges();
            }
        }

        public PlayerContract Register(string name, string password)
        {
            using (BattleshipsDbEntities bdc = new BattleshipsDbEntities())
            {
                if (bdc.Players.AsEnumerable().Where(p => p.Name == name).Count() == 0)
                {
                    Player player = new Player { Name = name, Password = password };
                    bdc.Players.Add(player);
                    bdc.SaveChanges();
                    return new PlayerContract(player.PlayerId, player.Name);
                }
            }
            return null;
        }

        public BattleshipsServiceLibrary.Game StartGame(PlayerContract playerr)
        {
            return GameCenter.StartGame(playerr, true);
        }

        public BattleshipsServiceLibrary.Game FindGame(PlayerContract player)
        {
            return GameCenter.FindGame(player);
        }

        public BattleshipsServiceLibrary.Game GetGame(int id)
        {
            return GameCenter.GetGameById(id);
        }

        public bool TryPutBoat(int gameId, long playerId, int x, int y, int size, char orientation)
        {
            return GameCenter.TryPutBoat(gameId, playerId, x, y, size, orientation);
        }

        public BlockState GetMyBoardBlock(int gameId, long player, int x, int y)
        {
            return GameCenter.GetBoardBlock(gameId, player, x, y);
        }

        public BlockState GetOpponentsBoardBlock(int gameId, long player, int x, int y)
        {
            BlockState ret = BlockState.EMPTY;
            PlayerContract opponent = GameCenter.Opponent(gameId, player);
            if (opponent == null)
                return ret;
            ret = GameCenter.GetBoardBlock(gameId, opponent.PlayerId, x, y);
            if (ret == BlockState.BOAT)
                ret = BlockState.EMPTY;
            return ret;
        }

        public void PlayerReady(int gameId, long player)
        {
            GameCenter.PlayerReady(gameId, player);
        }

        public string GetGameState(int gameId, long player)
        {
            return GameCenter.GetGameState(gameId, player);
        }

        public BlockState? MakeTurn(int gameId, long player, int x, int y)
        {
            return GameCenter.MakeTurn(gameId, player, x, y);
        }

        public bool GameOver(int gameId)
        {
            return GameCenter.GameOver(gameId);
        }

        public bool GameExists(int gameId)
        {
            return GameCenter.GameExists(gameId);
        }

        public void LeaveGame(int gameId, long playerId)
        {
            GameCenter.LeaveGame(gameId, playerId);
        }

        public BattleshipsServiceLibrary.Game JoinGame(PlayerContract player, int gameId)
        {
            return GameCenter.JoinGame(player, gameId);
        }

        public List<StatsContract> GetStats()
        {
            List<StatsContract> stats = new List<StatsContract>();
            using (BattleshipsDbEntities bdc = new BattleshipsDbEntities())
            {
                foreach (Player p in bdc.Players.Include("WonGames").Include("LostGames"))
                {
                    StatsContract sc = new StatsContract();
                    sc.Player = new PlayerContract(p.PlayerId, p.Name);
                    sc.Wins = p.WonGames.Count;
                    sc.Loses = p.LostGames.Count;
                    stats.Add(sc);
                }
            }
            return stats;
        }

        public List<GameResult> GetMyGames(long myId)
        {
            List<GameResult> games = new List<GameResult>();
            using (BattleshipsDbEntities bdc = new BattleshipsDbEntities())
            {
                foreach (BattleshipsDatabase.Game g in bdc.Games.Include("WinnerRef").Include("LoserRef").Where(g => g.Loser == myId || g.Winner == myId))
                {
                    GameResult gr = new GameResult()
                    {
                        End = g.End,
                        Start = g.Start,
                        GameId = g.GameId,
                        Opponent = g.Loser == myId ? g.WinnerRef.Name : g.LoserRef.Name,
                        Result = g.Winner == myId
                    };
                    games.Add(gr);
                }
            }
            return games;
        }

        public GameReplay GetGameReplay(long gameId)
        {
            GameReplay ret = new GameReplay();
            using (BattleshipsDbEntities bdc = new BattleshipsDbEntities())
            {
                BattleshipsDatabase.Game game = bdc.Games.Include("WinnerRef").Include("LoserRef").Include("Turns").FirstOrDefault(g => g.GameId == gameId);
                if (game == null)
                    return null;
                ret.Player1 = new PlayerContract(game.WinnerRef.PlayerId, game.WinnerRef.Name);
                ret.Player2 = new PlayerContract(game.LoserRef.PlayerId, game.LoserRef.Name);
                foreach (Turn t in game.Turns)
                {
                    ret.Moves.Add(new Move(t.Player, t.X,t.Y, t.Hit == true ? BlockState.HIT : BlockState.MISS));
                }
            }
            return ret;
        }
    }
}
