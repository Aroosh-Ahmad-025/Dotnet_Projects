using System;

namespace LoginFinal.Models
{
    public class RequestTags
    {
        public int Id { get; set; }
        public string Requested_TagName { get; set; }
        public int IsActive { get; set; }

        public Nullable<DateTime> CreatedAt { get; set; }

        public Nullable<DateTime> UpdatedAt { get; set; }

        public Nullable<DateTime> DeletedAt { get; set; }


        public int Requestid { get; set; }


        public Requesthelp RequestTag { get; set; }


    }
}
