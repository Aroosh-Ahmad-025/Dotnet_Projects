using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public int Role { get; set; }//admin 1
        public int CreatedBy { get; set; }  //admin Id
        public int IsActive { get; set; }
        public double? Salary { get; set; } //for rider
        public string Contact_No { get; set; }
        public string? Address { get; set; }
        public int IsRegular { get; set; } //0 not regular // 1 regular
        public List<SoldPackets> Packets_Sold { get; set; }
        public List<SoldPackets> Packets_Bought { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
    }
}