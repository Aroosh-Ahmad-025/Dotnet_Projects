using Microsoft.EntityFrameworkCore;

namespace MilkManagementSystem.Models
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Packets> Packets { get; set; }
        public DbSet<SoldPackets> SoldPackets { get; set; }



        //Relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<SoldPackets>()
           .HasOne<User>(e => e.Rider)
           .WithMany(d => d.Packets_Sold)
           .HasForeignKey(e => e.PickedBy)
           .IsRequired(false);

           modelBuilder.Entity<SoldPackets>()
             .HasOne<User>(e => e.Customer)
             .WithMany(d => d.Packets_Bought)
             .HasForeignKey(e => e.SoldTo)
             .IsRequired(false);

            //Junction Table Creating
            //  modelBuilder.Entity<Tags>()
            //  .HasMany(c => c.Instructors).WithMany(i => i.Courses)
            //  .Map(t => t.MapLeftKey("CourseID")
            //.MapRightKey("InstructorID")
            //.ToTable("CourseInstructor"));
        }
    }
}
