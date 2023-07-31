using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginFinal.Models
{
    public class CheckOutDTO
    {
        public int Id { set; get; }

        public DateTime FromDateTime { set; get; }
        public DateTime ToDateTime { set; get; }

        public string title { set; get; }
        public string desc { set; get; }
        public string duration { set; get; }

        public Nullable<int> orderId { set; get; }
        public int subscriptionId { set; get; }
        public int sessionCount { set; get; }
        public int customerId { set; get; }
        public int butlerId { set; get; }
        //public int buyerId { set; get; }
        //public int RecId { set; get; }

        public string hourlyRate { set; get; }
        public double subscribtionCharges { set; get; }
        public double stripeFee { set; get; }
        public double platformFee { set; get; }
        public double butlerServiceCharges { set; get; }

        public string wHours { set; get; }
        public  double workCharges { set; get; }
    }
}