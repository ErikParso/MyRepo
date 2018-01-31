using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsServiceLibrary
{
    [DataContract]
    public class StatsContract
    {
        [DataMember]
        public PlayerContract Player { get; set; }
        [DataMember]
        public int Wins { get; set; }
        [DataMember]
        public int Loses { get; set; }
        [DataMember]
        public int Rank { get; set; }
    }
}
