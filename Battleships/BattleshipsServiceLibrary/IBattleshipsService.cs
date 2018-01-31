using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsServiceLibrary
{
    [ServiceContract]
    public interface IBattleshipsService
    {
        [OperationContract]
        PlayerContract Register(string name, string password);

        [OperationContract]
        PlayerContract Login(string name, string password);

        [OperationContract]
        Game StartGame(PlayerContract playerr);

        [OperationContract]
        Game FindGame(PlayerContract player);

        [OperationContract]
        Game JoinGame(PlayerContract player, int gameId);

        [OperationContract]
        Game GetGame(int id);

        [OperationContract]
        bool TryPutBoat(int gameId, long playerId, int x, int y, int size, char orientation);

        [OperationContract]
        BlockState GetMyBoardBlock(int gameId, long player, int x, int y);

        [OperationContract]
        BlockState GetOpponentsBoardBlock(int gameId, long player, int x, int y);

        [OperationContract]
        void PlayerReady(int gameId, long player);

        [OperationContract]
        string GetGameState(int gameId, long player);

        [OperationContract]
        BlockState? MakeTurn(int gameId, long player, int x, int y);

        [OperationContract]
        bool GameOver(int gameId);

        [OperationContract]
        bool GameExists(int gameId);

        [OperationContract]
        void LeaveGame(int gameId, long playerId);

        [OperationContract]
        List<StatsContract> GetStats();

        [OperationContract]
        List<GameResult> GetMyGames(long myId);

        [OperationContract]
        GameReplay GetGameReplay(long gameId);
    }
}
