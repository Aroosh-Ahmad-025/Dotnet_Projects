using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginFinal.Models
{
    public class Tags
    {
        public int Id { get; set; } 

        public string TagName { get; set; }

        public int IsActive { get; set; }

        public Nullable<DateTime> CreatedAt { get; set; }

        public Nullable<DateTime> UpdatedAt { get; set; }

        public Nullable<DateTime> DeletedAt { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User Users { get; set; }

    }
}
