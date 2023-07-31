using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SolomonCookBook.Models{
    public class Recepie_likes{
        [Key]
        public int R_like_ID {get;set;}
        public int UserId{get;set;}
        public Recepies? Recepie{get;set;}
    }
}