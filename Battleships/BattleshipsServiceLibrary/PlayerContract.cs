using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsServiceLibrary
{
    [DataContract]
    public class PlayerContract
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public long PlayerId { get; set; }

        public PlayerContract(long id, string name)
        {
            this.Name = name;
            this.PlayerId = id;
        }
    }
}
