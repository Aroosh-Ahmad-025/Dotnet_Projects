using Microsoft.VisualBasic;
using System;

namespace LoginFinal.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public int IsSubscribed { get; set; }
        public int SessionCount { get; set; }
        public int subtractedSessions { get; set; }
        public DateTime SubscriptionStart { get; set; }
        public DateTime SubscriptionEnd { get; set; }
        public long SubscriptionPrice { get; set; }
        public string SubscriptionCharge { get; set; }
        public Nullable<int> subscriptionType { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public User Customer { get; set; }
       
    }
}
