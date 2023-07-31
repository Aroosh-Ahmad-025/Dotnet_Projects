using System;

namespace LoginFinal.Models
{
    public class Requestkills
    {
        public int Id { get; set; }

        public string Skillcategory { get; set; }
        public string Requested_SkillName { get; set; }

        public string subcategoryissue { get; set; }

        public int IsActive { get; set; }

        public Nullable<DateTime> CreatedAt { get; set; }

        public Nullable<DateTime> UpdatedAt { get; set; }

        public Nullable<DateTime> DeletedAt { get; set; }

        
        public Nullable<int>  Requestid { get; set; }


        public Requesthelp Requestskill { get; set; }


        


    }
}
