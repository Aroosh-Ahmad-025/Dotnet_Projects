using Microsoft.Data.SqlClient;
using MilkManagementSystem.DAL;
using MilkManagementSystem.Models;

namespace MilkManagementSystem.BL
{
    public class SoldPacketsBL
    {
    
        public List<SoldPackets> GetActiveSoldPacketsList(SqlConnection de)
        {
            return new SoldPacketsDAL().GetActiveSoldPacketsList(de);
        }  
        public SoldPackets GetActiveSoldPacketsByUserId(int id,SqlConnection de)
        {
            return new SoldPacketsDAL().GetActiveSoldPacketsByUserId(id,de);
        }

        //public List<SoldPackets> GetActivePendingPacketsList(SqlConnection de)
        //{
        //    return new SoldPacketsDAL().GetActivePendingPacketsList(de);
        //}
   
        public SoldPackets GetActiveSoldPacketsById(int id, SqlConnection de)
        {
            return new SoldPacketsDAL().GetActiveSoldPacketsById(id, de);
        }
        public async Task<bool> AddSoldPackets(SoldPackets _SoldPackets, SqlConnection de)
        {
            if (String.IsNullOrEmpty(_SoldPackets.Category.ToString()) || String.IsNullOrEmpty(_SoldPackets.Quantity.ToString()))
                return false;

            return await new SoldPacketsDAL().AddSoldPackets(_SoldPackets, de);
        }

        public async Task<bool> UpdateSoldPackets(SoldPackets _SoldPackets, SqlConnection de)
        {
            return await new SoldPacketsDAL().UpdateSoldPackets(_SoldPackets, de);
        }

        public async Task<bool> DeleteSoldPackets(int id, SqlConnection de)
        {
            return await new SoldPacketsDAL().DeleteSoldPackets(id, de);
        }
    
    }
}
