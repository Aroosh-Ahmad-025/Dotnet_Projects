using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginFinal.Models
{
    public class MessageDTO
    {
        public int Id { set; get; }
        
        public int SenderId { set; get; }
        public string SenderName { set; get; }

        public string Message { set; get; }
    }
}