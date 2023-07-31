using System;

namespace LoginFinal.Models
{
    public class Notification
    {
        public int Id { get; set; }

       
        public string Title { get; set; }

        
        public string Description { get; set; }

       

        public Nullable<int> IsRead { get; set; }
        public Nullable<int> IsActive { get; set; }

        
        public string CreatedAt { get; set; }

        
        public string DeletedAt { get; set; }

        public Nullable<int> UserId { get; set; }

        public Nullable<int> Senderid { get; set; }

        public Nullable<int> Requestid { get; set; }
        //public User User { get; set; }

        


    }
}
