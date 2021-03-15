using System;
using PropertyChanged;

namespace LeaveTracker.Model
{
    [AddINotifyPropertyChangedInterface]
    public class LeaveBalanceModel
    {
        public string Duration { get; set; }
        public string NextLabel { get; set; }
        public string Summary { get; set; }
        public string AprovedDate { get; set; }
    }
}
