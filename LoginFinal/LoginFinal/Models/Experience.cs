using System;


namespace LoginFinal.Models
{
    public class Experience
    {
        public int Id { get; set; }

        public int IsActive { get; set; }

        public Nullable<DateTime> CreatedAt { get; set; }

        public Nullable<DateTime> updatedat { get; set; }

        public Nullable<DateTime> Deletedat { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string Organization { get; set; }


        public string Designation { get; set; }

        public string Organization_Reference { get; set; }

        public string ReferalContact { get; set; }

        public int userid { get; set; }


        




    }
}
