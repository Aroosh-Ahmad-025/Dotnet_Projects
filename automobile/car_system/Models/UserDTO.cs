using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSystem.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string EncId { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}