using System;

namespace App.Models
{
    public class ObservationTask
    {
        public int Id { get; set; }
        public string TargetName { get; set; }
        public string RA { get; set; } // Format: hh:mm:ss
        public string Dec { get; set; } // Format: dd:mm:ss
        public string Telescope { get; set; }
        // Channel parameters
        public int LNums { get; set; }
        public int LExpSecs { get; set; }
        public int RNums { get; set; }
        public int RExpSecs { get; set; }
        public int GNums { get; set; }
        public int GExpSecs { get; set; }
        public int BNums { get; set; }
        public int BExpSecs { get; set; }
        public int HNums { get; set; }
        public int HExpSecs { get; set; }
        public int SNums { get; set; }
        public int SExpSecs { get; set; }
        public int ONums { get; set; }
        public int OExpSecs { get; set; }
    }
} 