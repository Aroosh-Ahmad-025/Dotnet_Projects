using System.ComponentModel.DataAnnotations.Schema;

namespace MilkManagementSystem.Models
{
    public class Packets
    {
        public int Id { get; set; }
        public double TotalQuantity { get; set; }
        public int Category { get; set; }

        public int CreatedBy { get; set; }

        public int IsActive { get; set; }

        [NotMapped]
        public string RemainingPackets { get; set; }
        public Nullable<DateTime> AddedDate { get; set; }
        public List<SoldPackets> SoldPackets { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

    }
}
