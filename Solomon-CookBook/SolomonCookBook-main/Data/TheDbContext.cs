using Microsoft.EntityFrameworkCore;
using SolomonCookBook.Models;

namespace SolomonCookBook.Data{
    public class TheDbContext : DbContext{
        public TheDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Recepies> Recepies {get;set;}
        // public DbSet<Recepie_Comments> Recepie_Comments {get;set;}
        public DbSet<User> Users {get;set;}
        public DbSet<Recepie_Comments> Recepie_Comments{get;set;}

        public DbSet<Recepie_likes> Recepie_Likes{get;set;}


    }
}