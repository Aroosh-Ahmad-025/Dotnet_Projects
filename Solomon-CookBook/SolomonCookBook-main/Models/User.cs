using System.ComponentModel.DataAnnotations;
namespace SolomonCookBook.Models{
    public class User{
        [Key]
        public int User_ID{get;set;}
        [Display(Name = "Enter your First Name"), Required]
        public string? First_name{get;set;}

        [Display(Name = "Enter your Email"), Required]
        public string? Email{get;set;}
        [Display(Name = "Enter your Phone Number"), Required]
        public string? Phone_number{get;set;}
        [Display(Name = "Enter your Password"), Required]
        public string? Password{get;set;}    

    }
}