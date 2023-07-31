namespace LoginFinal.Models
{
    public class butlerprefertime
    {
        public int Id { get; set; }
        public string CreatedAt { get; set; }

        public int IsActive { get; set; }

        public string UpdatedAt { get; set; }

        public string DeletedAt { get; set; }

        public string PreferTime { get; set; }

        public int UserId { get; set; }

        public User user { get; set; }


    }
}
