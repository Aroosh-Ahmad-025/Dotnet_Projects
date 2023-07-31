using System.ComponentModel.DataAnnotations.Schema;

namespace MilkManagementSystem.Models
{
    public class SoldPackets
    {
        public int Id { get; set; }
        public int Category { get; set; }
        public int Quantity { get; set; } //total packets taken
        public int PricePerPacket { get; set; }
        public string? Pickup_Location { get; set; }
        public string? DropOff_Location { get; set; }
        public double? Total_Distance { get; set; }

        [ForeignKey("Rider")]
        public int PickedBy { get; set; } //rider Id

        [ForeignKey("Customer")]
        public int SoldTo { get; set; } //User Id
        public User Rider { get; set; }
        public User Customer { get; set; }
        public int IsActive { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
        public Nullable<DateTime> SoldDate { get; set; }
        public string SoldDateString { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
    }
}
