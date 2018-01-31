using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsServiceLibrary
{
    [DataContract]
    public class Game
    {
        [DataMember]
        public int GameId { get; private set; }
        [DataMember]
        public PlayerContract Player1 { get; private set; }
        [DataMember]
        public bool Player1Ready { get; set; }
        [DataMember]
        public PlayerContract Player2 { get; set; }
        [DataMember]
        public bool Player2Ready { get; set; }
        [DataMember]
        public bool IsPrivate { get; private set; }
        [DataMember]
        public PlayerContract Turn { get; set; }
        [DataMember]
        public PlayerContract Winner { get; set; }
        [DataMember]
        public int Players1Lifes { get; private set; }
        [DataMember]
        public int Players2Lifes { get; private set; }
        [DataMember]
        public DateTime Start { get; private set; }

        public List<Move> Moves { get; private set; }
        
        public bool Player1Left { get; set; }

        public bool Player2Left { get; set; }

        private BlockState[,] player1Box;
        private BlockState[,] player2Box;

        public Game(int id, PlayerContract player1, bool isPrivate)
        {
            GameId = id;
            Player1 = player1;
            IsPrivate = isPrivate;
            player1Box = new BlockState[10, 10];
            player2Box = new BlockState[10, 10];
            Start = DateTime.Now;
            Moves = new List<Move>();
        }

        public bool TryPutBoat(long playerId, int x, int y, int size, char orientation)
        {
            BlockState[,] box = GetPlayersBlocks(playerId);
            if (orientation == 'H')
            {
                //check if if ship can be there 
                if (x + size > 10)
                    return false;
                for (int i = 0; i < size; i++)
                    if (!CanPlaceBoatBlockThere(box, x + i, y))
                        return false;
                //yes it can place it there
                for (int i = 0; i < size; i++)
                    box[y, x + i] = BlockState.BOAT;
            }
            else if (orientation == 'V')
            {
                //check if if ship can be there 
                if (y + size > 10)
                    return false;
                for (int i = 0; i < size; i++)
                    if (!CanPlaceBoatBlockThere(box, x, y + i))
                        return false;
                //yes it can place it there
                for (int i = 0; i < size; i++)
                    box[y + i, x] = BlockState.BOAT;
            }
            else
                throw new Exception("Should not happend");
            //add boat length ti players lifes
            if (Player1.PlayerId == playerId)
                Players1Lifes += size;
            else if (Player2.PlayerId == playerId)
                Players2Lifes += size;
            else
                throw new Exception("Should not happend");
            return true;
        }

        private bool CanPlaceBoatBlockThere(BlockState[,] box, int x, int y)
        {
            for (int px = -1; px < 2; px++)
                for (int py = -1; py < 2; py++)
                    try { if (box[y + py, x + px] != BlockState.EMPTY) return false; } catch (IndexOutOfRangeException e) { }
            return true;
        }

        internal BlockState GetBlock(long player, int x, int y)
        {
            return GetPlayersBlocks(player)[y, x];
        }

        private BlockState[,] GetPlayersBlocks(long playerId)
        {
            if (Player1.PlayerId == playerId)
                return player1Box;
            else if (Player2.PlayerId == playerId)
                return player2Box;
            else
                throw new Exception("Should not happend");

        }

        internal BlockState? MakeTurn(long player, int x, int y)
        {
            if (Turn == null)
                return null;
            if (Turn.PlayerId != player)
                return null;
            BlockState[,] box = GetPlayersBlocks(Opponent(player).PlayerId);
            if (box[y, x] == BlockState.BOAT)
            {
                box[y, x] = BlockState.HIT;
                if (Player1.PlayerId == player)
                    Players1Lifes --;
                else if (Player2.PlayerId == player)
                    Players2Lifes --;
                else
                    throw new Exception("Should not happend");
                if (Players1Lifes == 0 || Players2Lifes == 0)
                { 
                    Winner = Turn;
                    Turn = null;
                }
                Moves.Add(new Move(player, x, y, BlockState.HIT));
                return BlockState.HIT;
            }
            if (box[y, x] == BlockState.EMPTY)
            {
                box[y, x] = BlockState.MISS;
                Turn = Opponent(Turn.PlayerId);
                Moves.Add(new Move(player, x, y, BlockState.MISS));
                return BlockState.MISS;
            }
            return null;
        }

        public PlayerContract Opponent(long player)
        {
            if (Player1.PlayerId == player)
                return Player2;
            else if (Player2.PlayerId == player)
                return Player1;
            else
                throw new Exception("Should not happend");
        }

        public bool OpponentReady(long player)
        {
            if (Player1.PlayerId == player)
                return Player2Ready;
            else if (Player2.PlayerId == player)
                return Player1Ready;
            else
                throw new Exception("Should not happend");
        }
    }
}
