using System;

namespace LoginFinal.Models
{
    public class Refferals
    {
        public int Id { get; set; }

        public string RefferalCode { get; set; }
      
        public int? IsActive { get; set; }

        public int  RefferalType { get; set; }

        public int ReferredUserType { get; set; }

        public Nullable<DateTime> CreatedAt { get; set; }

        public Nullable<DateTime> UpdatedAt { get; set; }

        public Nullable<DateTime> DeletedAt { get; set; }
        
        public Nullable<DateTime> ReferralEndTime { get; set; }

        public int RefferedId { get; set; }

        public virtual User RefferedUser { get; set; }    }
}
