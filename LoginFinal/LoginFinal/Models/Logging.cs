using System;

namespace LoginFinal.Models
{
    public class Logging
    {
        public int Id { get; set; }
        public int? IsActive { get; set; }
        public string SearchKeyword { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public int? ProfileId { get; set; }

        //public string GeoLocation { get;set; }
        public int UserId { get;set; }
        public User Users { get; set; }
    }
}
