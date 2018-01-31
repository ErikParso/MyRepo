using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsServiceLibrary
{
    [DataContract]
    public class GameResult
    {
        [DataMember]
        public long GameId { get; set; }

        [DataMember]
        public string Opponent { get; set; }

        [DataMember]
        public DateTime Start { get; set; }

        [DataMember]
        public DateTime End { get; set; }

        [DataMember]
        public bool Result { get; set; }
    }
}
