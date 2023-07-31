using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginFinal.Models
{
    public class Order
    {   
        public int Id { get; set; }
        public string OrderTitle { get; set; }   
        public string OrderDescription { get; set; }
        public string Cancellation_Reason { get; set; }
        public string Extending_Reason { get; set; }
        public string Revision_Reason { get; set; }
        public long OrderPrice { get; set; }
        public int IsAccepted { get; set; }
        public int requestedSessions { get; set; }
        public string Charge { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public int paidBy { get; set; }
        public int IsActive { get; set; }
        public int IsDelivered { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public User Buyer { get; set; }
        public User Seller { get; set; }
        public string BuyerCommentsCount { get; set; }
        public string SellerCommentsCount { get; set; }

        //public OrderReview OrderReview { get; set; }

        //public virtual List<Message> Messages { get;set; }

    }
}
