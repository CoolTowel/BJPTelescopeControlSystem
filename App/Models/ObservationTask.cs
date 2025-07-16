using System;

namespace App.Models
{
    public class ObservationTask
    {
        [Key]
        public required Guid GUID { get; set; }
        public required Guid ApplicantID { get; set; } // 提交用户的ID
        public required string ApplicantName { get; set; } // 提交用户的名字
        public required DateTime SubmitTime { get; set; }
        public required string TargetName { get; set; }
        public required double RA { get; set; } // Format: hh:mm:ss
        public required double Dec { get; set; } // Format: dd:mm:ss
        public required int Telescope { get; set; }
        // Channel parameters
        public required int LNums { get; set; }
        public required int LExpSecs { get; set; }
        public required int RNums { get; set; }
        public required int RExpSecs { get; set; }
        public required int GNums { get; set; }
        public required int GExpSecs { get; set; }
        public required int BNums { get; set; }
        public required int BExpSecs { get; set; }
        public required int HNums { get; set; }
        public required int HExpSecs { get; set; }
        public required int SNums { get; set; }
        public required int SExpSecs { get; set; }
        public required int ONums { get; set; }
        public required int OExpSecs { get; set; }
    }
} 