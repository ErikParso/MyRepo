using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsClient.Model
{
    public class PlayerModel : ObservableObject
    {
        private string _name;
        public string Name { get { return _name; } set { _name = value; RaisePropertyChanged("Name"); } }

        private long _id;
        public long Id { get { return _id; } set { _id = value; RaisePropertyChanged("Id"); } }

        private int _wins;
        public int Wins { get { return _wins; } set { _wins = value; RaisePropertyChanged("Wins"); } }

        private int _loses;
        public int Loses { get { return _loses; } set { _loses = value; RaisePropertyChanged("Loses"); } }

        private long _rank;
        public long Rank { get { return _rank; } set { _rank = value; RaisePropertyChanged("Rank"); } }

    }
}
