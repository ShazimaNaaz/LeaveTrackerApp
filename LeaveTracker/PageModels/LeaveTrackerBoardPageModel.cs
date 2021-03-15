using System;
using System.Collections.ObjectModel;
using FreshMvvm;
using LeaveTracker.Model;
using PropertyChanged;

namespace LeaveTracker.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class LeaveTrackerBoardPageModel :FreshBasePageModel
    {
        public ObservableCollection<LeaveBalanceModel> LeaveBalanceList { get; set; }
        public LeaveTrackerBoardPageModel()
        {
            LeaveBalanceList = new ObservableCollection<LeaveBalanceModel>();

            var obj1 = new LeaveBalanceModel();
            obj1.Duration = "AUG 15th ~~~ AUG 18th";
            obj1.NextLabel = "4 days next week";
            obj1.Summary = "Hey I need to take care of my personal pending work so I would like to take a break from work next week 4 days.";
            obj1.AprovedDate = "Approved \non Aug 11th";

            var obj2 = new LeaveBalanceModel();
            obj2.Duration = "Jul 8th ~~~ Jul 12th";
            obj2.NextLabel = "5 days next week";
            obj2.Summary = "Hey I am out for a vacation with family out of India so request you to approve the leaves.";
            obj2.AprovedDate = "Approved \non Jul 1st";

            var obj3 = new LeaveBalanceModel();
            obj3.Duration = "May 02nd ~~~ JUL 08th";
            obj3.NextLabel = "7 days next week";
            obj3.Summary = "Relocation leaves as I have joined here 3 month back I need to shift by family from banglore to hyderabad and set the house.";
            obj3.AprovedDate = "Approved \non May 01st";

            LeaveBalanceList.Add(obj1);
            LeaveBalanceList.Add(obj2);
            LeaveBalanceList.Add(obj3);
        }
    }
}
