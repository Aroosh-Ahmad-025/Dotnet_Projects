using Microsoft.Data.SqlClient;
using MilkManagementSystem.DAL;
using MilkManagementSystem.Models;

namespace MilkManagementSystem.BL
{
    public class PacketsBL
    {
        public List<Packets> GetActivePacketsList(SqlConnection de)
        {
            return new PacketsDAL().GetActivePacketsList(de);
        }

        public Packets GetActivePacketsById(int id, SqlConnection de)
        {
            return new PacketsDAL().GetActivePacketsById(id, de);
        }
        public async Task<bool> AddPackets(Packets _packets, SqlConnection de)
        {
            if (String.IsNullOrEmpty(_packets.TotalQuantity.ToString()) || String.IsNullOrEmpty(_packets.Category.ToString()))
                return false;

            return await new PacketsDAL().AddPackets(_packets, de);
        }

        public async Task<bool> UpdatePackets(Packets _packets, SqlConnection de)
        {
            return await new PacketsDAL().UpdatePackets(_packets, de);
        }

        public async Task<bool> DeletePackets(int id, SqlConnection de)
        {
            return await new PacketsDAL().DeletePackets(id, de);
        }
    }
}
