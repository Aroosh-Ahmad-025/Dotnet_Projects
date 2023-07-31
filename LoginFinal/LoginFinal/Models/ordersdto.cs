using System;

namespace LoginFinal.Models
{
    public class ordersdto
    {

        public int Id { get; set; }
        public string OrderTitle { get; set; }
        public string OrderDescription { get; set; }

        public string color { get; set; }
        public string url { get; set; }

        public string NewEndDate { get; set; }

        public string PopoverString { get; set; }

        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }


        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<int> BuyerId { get; set; }
        public Nullable<int> SellerId { get; set; }

    }
}
