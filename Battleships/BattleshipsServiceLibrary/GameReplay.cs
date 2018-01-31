using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsServiceLibrary
{
    [DataContract]
    public class GameReplay
    {
        [DataMember]
        public PlayerContract Player1 { get; set; }

        [DataMember]
        public PlayerContract Player2 { get; set; }

        [DataMember]
        public List<Move> Moves { get; set; }

        public GameReplay()
        {
            Moves = new List<Move>();
        }
    }
}
