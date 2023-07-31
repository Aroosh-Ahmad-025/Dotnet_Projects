using System;
namespace LoginFinal.Models
{
    public class preferservicetime
    {
        public int Id { get; set; }
        public string CreatedAt { get; set; }

        public int IsActive { get; set; }

        public string UpdatedAt { get; set; }

        public string DeletedAt { get; set; }

        public  string servicetime{ get; set; }

        public int requestid { get; set; }

        public Requesthelp request { get; set; }



    }
}
