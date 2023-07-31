using System;

namespace LoginFinal.Models
{
    public class InternalReview
    {
        public int Id { get; set; }
        public string Sellerreview { get; set; }
        public Nullable<int>  Stars { get; set; }
        public string customerid { get; set; }
        public Nullable<int> IsActive { get; set; }

        public Nullable<DateTime> CreatedAt { get; set; }
        public string Sellerid { get; set; }

        public Nullable<int> Orderid { get; set; }


    }
}
