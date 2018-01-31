using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsServiceLibrary
{
    [DataContract]
    public enum BlockState
    {
        [EnumMember]
        EMPTY,
        [EnumMember]
        BOAT,
        [EnumMember]
        MISS,
        [EnumMember]
        HIT
    }
}
