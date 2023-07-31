using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginFinal.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //Tables
        public DbSet<User> Users { get; set; }

        public DbSet<Education> Education { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Order> Orders { get; set; }
        
        public DbSet<Logging> Logging { get; set; }
        public DbSet<Skills> Skills { get; set; }

        public DbSet<Tags> Tags { get; set; }

        public DbSet<OrderReview> OrderReviews { get; set; }

        public DbSet<Refferals> Refferals { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Requesthelp> Requesthelp { get; set; }


        public DbSet<Experience> Experience { get; set; }

        public DbSet<UserLanguage> UserLanguage { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }
        
        public DbSet<InternalReview> InternalReviews { get; set; }

        public DbSet<preferservicetime> preferservicetimes { get; set; }
        public DbSet<Requestkills> RequestSkills { get; set; }
        
        public DbSet<RequestTags> RequestTags { get; set; }
        public DbSet<Testorder> Testorders { get; set; } 
        public DbSet<butlerprefertime> butlerprefertime { get; set; }

       
        //Relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Education>()
             .HasOne<User>(e => e.Users)
             .WithMany(d => d.Education)
             .HasForeignKey(e => e.UserId)
             .IsRequired(false);

            modelBuilder.Entity<Tags>()
            .HasOne<User>(e => e.Users)
            .WithMany(d => d.Tags)
            .HasForeignKey(e => e.UserId)
            .IsRequired(false);

            modelBuilder.Entity<Skills>()
            .HasOne<User>(e => e.Users)
            .WithMany(d => d.Skills)
            .HasForeignKey(e => e.UserId)
            .IsRequired(false);

            modelBuilder.Entity<Requestkills>()
            .HasOne<Requesthelp>(e => e.Requestskill)//obj of foreign reference model
            .WithMany(d => d.requestedskills) //obj in foreign reference model
            .HasForeignKey(e => e.Requestid) //foreign key
            .IsRequired(false);

            modelBuilder.Entity<RequestTags>()
            .HasOne<Requesthelp>(e => e.RequestTag)//obj of foreign reference model
            .WithMany(d => d.requestedTags) //obj in foreign reference model
            .HasForeignKey(e => e.Requestid) //foreign key
            .IsRequired(false);

            modelBuilder.Entity<Message>()
            .HasOne<User>(e => e.Users)
            .WithMany(d => d.Messages)
            .HasForeignKey(e => e.SenderId)
            .IsRequired(false);


            modelBuilder.Entity<Message>()
            .HasOne<User>(e => e.RecieverEnd)
            .WithMany(d => d.RecieverEnd)
            .HasForeignKey(e => e.RecieverId)
            .IsRequired(false);


            modelBuilder.Entity<Logging>()
            .HasOne<User>(e => e.Users)
            .WithMany(d => d.Logging)
            .HasForeignKey(e => e.UserId)
            .IsRequired(false);


            modelBuilder.Entity<Refferals>()
           .HasOne<User>(e => e.RefferedUser)
           .WithMany(d => d.Refferals)
           .HasForeignKey(e => e.RefferedId)
           .IsRequired(false);

            modelBuilder.Entity<Order>()
           .HasOne<User>(e => e.Buyer)
           .WithMany(d => d.Buyer)
           .HasForeignKey(e => e.BuyerId)
           .IsRequired(false);

            modelBuilder.Entity<Order>()
            .HasOne<User>(e => e.Seller)
            .WithMany(d => d.Seller)
            .HasForeignKey(e => e.SellerId)
            .IsRequired(false);

            modelBuilder.Entity<OrderReview>()
            .HasOne<User>(e => e.Buyer)
            .WithMany(d => d.BuyerReview)
            .HasForeignKey(e => e.CommentId)
            .IsRequired(false);

            modelBuilder.Entity<OrderReview>()
            .HasOne<User>(e => e.Seller)
            .WithMany(d => d.SellerReview)
            .HasForeignKey(e => e.CommentUser)
            .IsRequired(false);

            modelBuilder.Entity<UserLanguage>()
              .HasOne<User>(e => e.users)
              .WithMany(d => d.userlanguages)
              .HasForeignKey(e => e.UserId)
              .IsRequired(false);

            modelBuilder.Entity<preferservicetime>()
              .HasOne<Requesthelp>(e => e.request)
              .WithMany(d => d.prefferservicetime)
              .HasForeignKey(e => e.requestid)
              .IsRequired(false);
            
            modelBuilder.Entity<butlerprefertime>()
              .HasOne<User>(e => e.user)
              .WithMany(d => d.butlerprefertimes)
              .HasForeignKey(e => e.UserId)
              .IsRequired(false);



            modelBuilder.Entity<Subscription>()
              .HasOne<User>(e => e.Customer)
              .WithMany(d => d.Subscriptions)
              .HasForeignKey(e => e.BuyerId)
              .IsRequired(true);

            //Junction Table Creating
            //  modelBuilder.Entity<Tags>()
            //  .HasMany(c => c.Instructors).WithMany(i => i.Courses)
            //  .Map(t => t.MapLeftKey("CourseID")
            //.MapRightKey("InstructorID")
            //.ToTable("CourseInstructor"));
        }



    }

    //public class YourDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    //{
    //    public AppDbContext CreateDbContext(string[] args)
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
    //        optionsBuilder.UseSqlServer("Server=DESKTOP-ISJQ1C0\\SQLEXPRESS;Database=iT_Buttlerdb;Trusted_Connection=True;MultipleActiveResultSets=true");

    //        return new AppDbContext(optionsBuilder.Options);
    //    }
    //}
}
