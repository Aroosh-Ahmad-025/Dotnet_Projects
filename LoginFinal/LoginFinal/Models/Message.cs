using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginFinal.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Message_Description { get; set; }
        public int IsActive { get; set; }
        public int IsRead { get; set; }
        public string FilePath { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public int SenderId { get; set; }
        public int RecieverId { get; set; }

        [ForeignKey("SenderId")]
        public virtual User Users { get; set; }

        [ForeignKey("RecieverId")]
        public virtual User RecieverEnd { get; set; }

        [NotMapped]
        public IFormFile FileUpload { get; set; }

        public int OrderId { get; set; }

    }
}
