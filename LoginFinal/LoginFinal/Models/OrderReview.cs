using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginFinal.Models
{
    public class OrderReview
    {
        public int Id { get; set; }
        public string BuyerComment { get; set; }
        public string SellerComment { get; set; }
        public int Stars { get; set; }
        public string FilePath { get; set; }
        public int IsActive { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
       
        public Nullable<DateTime> DeletedAt { get; set; }
    
        public int OrdId { get; set; }
        
        public int CommentId { get; set; }
        
        public int CommentUser { get; set; }

        [ForeignKey("CommentId")]
        public User Buyer { get; set; }

        [ForeignKey("CommentUser")]
        public User Seller { get; set; }

        [NotMapped]
        public IFormFile FileUpload { get; set; }

    }
}
