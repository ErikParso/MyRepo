using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsServiceLibrary
{
    [DataContract]
    public class Move
    {
        [DataMember]
        public long Player { get; private set; }

        [DataMember]
        public int AtX { get; private set; }

        [DataMember]
        public int AtY { get; private set; }

        [DataMember]
        public BlockState Result { get; private set; }

        [DataMember]
        public DateTime Time { get; private set; }

        public Move(long player, int atX, int atY, BlockState result)
        {
            Player = player;
            AtX = atX;
            AtY = atY;
            Result = result;
            Time = DateTime.Now;
        }
    }
}
