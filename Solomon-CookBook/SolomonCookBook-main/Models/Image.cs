using System.ComponentModel.DataAnnotations;
namespace SolomonCookBook.Models{
    public class Image{
        [Key]
        public int ImageId{get;set;}
        
        public IFormFile Img{get;set;}
    }
}