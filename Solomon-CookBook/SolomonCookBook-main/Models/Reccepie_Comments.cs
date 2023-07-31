using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SolomonCookBook.Models{
    public class Recepie_Comments{
        [Key]
        public int R_Comment_ID{get;set;}

        public int? User_ID {get;set;} 
        public int? Recepie_ID{get;set;}
         
        public DateTime? Date{get;set;} = DateTime.Now;

        public string? Comment{get;set;}

    }
}