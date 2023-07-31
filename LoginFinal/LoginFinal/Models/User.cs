using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginFinal.Models
{
    public class User
    {

       
        [Key]
        public int Id { get; set; }

        [NotMapped]
        public string EncId { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        [Required]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        [Required]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        [Required]
        public string Username { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Contact { get; set; }

        [Column(TypeName = "nvarchar(355)")]
        [Required]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [Required]
        public string Password { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        [Required]
        public string Country { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string ImagePath { get; set; }

        public Nullable<DateTime> DOB { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Gender { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string City { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Organization { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Website { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Language { get; set; }
        public Nullable<DateTime> Experience_From { get; set; }
        public Nullable<DateTime> Experience_To { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Company { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Position { get; set; }

        [Column(TypeName = "nvarchar(8)")]
        public string ZipCode { get; set; }
        public int? Status { get; set; }
        public int Availability { get; set; }
        public Nullable<int> Role { get; set; }
        public Nullable<int> IsActive { get; set; } //DeactivateAcc=100;
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Refferal_Code { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Reffered_By { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string GoogleLink { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string GitHubLink { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string StackOverFlowLink { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string DribbleLink { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string VimeoLink { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string FacebookLink { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string TwitterLink { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Acc_Deactive_Reason { get; set; }
        public int? Stars { get; set; }
        public string StartingFrom { get; set; }
        public string Comment { get; set; }
        public string ConnectionId { get; set; }

        public string StripeId { get; set; }

        public string Account_Verification_Link { get; set; }

        public int Is_Verified { get; set; }
        public virtual List<Education> Education { get; set; }

        public virtual List<Tags> Tags { get; set; }

        public virtual List<Skills> Skills { get; set; }

        //public virtual List<Orders> Orders { get; set; }
        public virtual List<Message> Messages { get; set; }

        public virtual List<Message> RecieverEnd { get; set; }

        public virtual List<Logging> Logging { get; set; }

        public virtual List<Order> Buyer { get; set; }

        public virtual List<Order> Seller { get; set; }

        public virtual List<OrderReview> BuyerReview { get; set; }

        public virtual List<OrderReview> SellerReview { get; set; }

        public virtual List<Refferals> Refferals { get; set; }

        public List<UserLanguage> userlanguages { get; set; }
        public List<Subscription> Subscriptions { get; set; }

        public List<butlerprefertime> butlerprefertimes { get; set; }


    }

}
