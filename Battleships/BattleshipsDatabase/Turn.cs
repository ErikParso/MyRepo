//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BattleshipsDatabase
{
    using System;
    using System.Collections.Generic;
    
    public partial class Turn
    {
        public long TurnId { get; set; }
        public long Game { get; set; }
        public System.DateTime Time { get; set; }
        public long Player { get; set; }
        public bool Hit { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    
        public virtual Game GameRef { get; set; }
        public virtual Player PlayerRef { get; set; }
    }
}
