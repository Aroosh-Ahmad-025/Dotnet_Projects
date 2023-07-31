using System;

namespace LoginFinal.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Contact_No { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public int IsActive { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
    }
}
