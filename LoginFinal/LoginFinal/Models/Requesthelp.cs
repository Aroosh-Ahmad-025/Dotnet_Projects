using System;
using System.Collections.Generic;

namespace LoginFinal.Models
{
    public class Requesthelp
    {
        public int Id { get; set; }
        public int Isactive { get; set; }
        //public string CreatedAt { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public string DeletedAt { get; set; }
       // public string UpdatedAt { get; set; }
       
        public Nullable<DateTime> UpdatedAt { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string budget { get; set; }  // currently we using it as category of skills 
        public Nullable<DateTime> FromDateTime { get; set; }
        public Nullable<DateTime> ToDateTime { get; set; }
        public string skills { get; set; }
        public string Tags { get; set; }
        public string Zipcodes { get; set; }

        public string servicetime { get; set; }
        public string City { get; set; }

        public string Language { get; set; }

        public int userid { get; set; }

        public User Requesteduser { get; set; }

        public string EmailToButlers { get; set; }
        public int EmailToCustomer { get; set; }

        public string RequestType { get; set; }
        public List<preferservicetime> prefferservicetime { get; set; }

        public List<Requestkills> requestedskills { get; set; }

        public List<RequestTags> requestedTags { get; set; }


    }
}
