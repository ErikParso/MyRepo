using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsServiceLibrary
{
    public class GameCenter
    {
        private Dictionary<int, Game> games;

        private Random gameIdGenerator;

        public Action<Game> GameEnded { get; set; }

        public GameCenter()
        {
            games = new Dictionary<int, Game>();
            gameIdGenerator = new Random();
        }

        public Game StartGame(PlayerContract player1, bool isPrivate = false)
        {
            int gameId = gameIdGenerator.Next();
            while (games.ContainsKey(gameId) || gameId == 0)
            {
                gameId = gameIdGenerator.Next();
            }
            Game g = new Game(gameId, player1, isPrivate);
            games.Add(gameId, g);
            return g;
        }

        public Game FindGame(PlayerContract player)
        {
            //try find free game 
            int found = games.Where(kp =>
                kp.Value.Player2 == null &&
                kp.Value.Player1.PlayerId != player.PlayerId &&
                !kp.Value.IsPrivate).Select(kp => kp.Key).FirstOrDefault();
            if (found != 0)
            {
                games[found].Player2 = player;
                return games[found];
            }
            else
            {
                return StartGame(player);
            }
        }

        public Game GetGameById(int id)
        {
            return games[id];
        }

        public bool TryPutBoat(int gameId, long playerId, int x, int y, int size, char orientation)
        {
            return games[gameId].TryPutBoat(playerId, x, y, size, orientation);
        }

        public BlockState GetBoardBlock(int gameId, long player, int x, int y)
        {
            if (!games.ContainsKey(gameId))
                return BlockState.EMPTY;
            return games[gameId].GetBlock(player, x, y);
        }

        public void PlayerReady(int gameId, long player)
        {
            Game game = games[gameId];
            if (game.Player1.PlayerId == player)
                game.Player1Ready = true;
            else if (game.Player2.PlayerId == player)
                game.Player2Ready = true;
            else
                throw new Exception("should not happend");
            if (game.Player2Ready && game.Player1Ready)
                game.Turn = game.Player1;
        }

        public string GetGameState(int gameId, long player)
        {
            if (!games.ContainsKey(gameId))
                return "Game not initialised";

            Game game = games[gameId];
            if (game.Opponent(player) == null)
                return game.IsPrivate ? 
                    $"Prepare your battlefield while waiting for friend. Game Id: {game.GameId}" : 
                    "Searching for oponent...";
            if (game.Winner?.PlayerId == player)
                return $"!! VICTORY !!";
            if (game.Winner?.PlayerId == game.Opponent(player).PlayerId)
                return $".. DEFEAT ..";
            if (game.Opponent(player) != null && !game.OpponentReady(player))
                return $"Player {game.Opponent(player).Name} placing his ships...";
            if (game.Turn?.PlayerId == player)
                return $"Your turn...";
            if (game.Turn?.PlayerId == game.Opponent(player).PlayerId)
                return $"Opponents turn...";
            if (game.Opponent(player) != null && game.OpponentReady(player))
                return $"Opponent ready, place your ships...";

            throw new Exception();
        }

        public Game JoinGame(PlayerContract player, int gameId)
        {
            if (!games.ContainsKey(gameId))
                return null;
            else
            {
                games[gameId].Player2 = player;
                return games[gameId];
            }
        }

        public bool GameExists(int gameId)
        {
            return games.ContainsKey(gameId);
        }

        public void LeaveGame(int gameId, long playerId)
        {
            Game g = games[gameId];
            //Player 2 was not connected
            if (g.Player1.PlayerId == playerId && g.Player2 == null)
                games.Remove(gameId);
            //Player 1 disconnected
            if (g.Player1.PlayerId == playerId)
                g.Player1Left = true;
            else if (g.Player2.PlayerId == playerId)
                g.Player2Left = true;
            else throw new Exception();
            if (g.Winner == null)
            {
                g.Winner = g.Opponent(playerId);
                g.Turn = null;
            }
            if (g.Player1Left && g.Player2Left)
            {
                games.Remove(gameId);
                if (GameEnded != null)
                    GameEnded(g);
            }
        }

        public BlockState? MakeTurn(int gameId, long player, int x, int y)
        {
            return games[gameId].MakeTurn(player, x, y);
        }

        public bool GameOver(int gameId)
        {
            if (!games.ContainsKey(gameId))
                return true;
            return games[gameId].Winner != null;
        }

        public PlayerContract Opponent(int gameId, long player)
        {
            return games[gameId].Opponent(player);
        }
    }
}
