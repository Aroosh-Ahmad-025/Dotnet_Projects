namespace LoginFinal.Models
{
    public class UserLanguage
    {
        public int Id { get; set; }
        public string CreatedAt { get; set; }

        public int IsActive { get; set; }

        public int UpdatedAt { get; set; }

        public string language { get; set; }

        public int UserId { get; set; }

        public User users { get; set; }

    }
}
