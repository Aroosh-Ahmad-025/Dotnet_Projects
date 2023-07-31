using Dapper;
using Microsoft.Data.SqlClient;
using MilkManagementSystem.Models;

namespace MilkManagementSystem.DAL
{
    public class PacketsDAL
    {
        public List<Packets> GetActivePacketsList(SqlConnection de)
        {
            return de.Query<Packets>("EXECUTE GetAllRecords Packets").ToList();
        }

        public Packets GetPacketsById(int Id, SqlConnection de)
        {
            return de.Query<Packets>("EXECUTE GetAllRecords Packets,Id," + Id + "").FirstOrDefault();
        }

        public Packets GetActivePacketsById(int Id, SqlConnection de)
        {
            return de.Query<Packets>("EXECUTE GetAllRecords Packets,Id," + Id + "").FirstOrDefault();
        }

        public async Task<bool> AddPackets(Packets _packets, SqlConnection de)
        {
            try
            {
                var getPropandVal = new UserDAL().GetPropandVal(_packets);
                var add = await de.QueryAsync("EXECUTE InsertOrUpdate 0,Packets," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdatePackets(Packets _packets, SqlConnection de)
        {
            try
            {
                _packets.UpdatedAt = DateTime.Now;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(_packets);
                var update = await de.QueryAsync("EXECUTE InsertOrUpdate " + _packets.Id + ",Packets,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> DeletePackets(int id, SqlConnection de)
        {
            try
            {
                Packets u = GetActivePacketsById(id, de);

                u.IsActive = 0;
                u.DeletedAt = DateTime.UtcNow;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(u);
                var delete = await de.QueryAsync("EXECUTE InsertOrUpdate " + u.Id + ",Packets,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
