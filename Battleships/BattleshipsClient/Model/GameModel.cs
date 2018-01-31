using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsClient.Model
{
    public class GameModel
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool Result { get; set; }
        public string Opponent { get; set; }
        public long GameId { get; set; }
    }
}
