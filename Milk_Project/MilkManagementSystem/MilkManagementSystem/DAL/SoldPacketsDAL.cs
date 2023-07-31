using Dapper;
using Microsoft.Data.SqlClient;
using MilkManagementSystem.Models;

namespace MilkManagementSystem.DAL
{
    public class SoldPacketsDAL
    {
        public List<SoldPackets> GetActiveSoldPacketsList(SqlConnection de)
        {
            return de.Query<SoldPackets>("EXECUTE GetAllRecords SoldPackets").ToList();
        }

        //public List<SoldPackets> GetActivePendingPacketsList(SqlConnection de)
        //{
        //    return de.Query<SoldPackets>("EXECUTE GetAllRecords SoldPackets,IsSold,0").ToList();
        //}

        public SoldPackets GetActiveSoldPacketsByUserId(int id, SqlConnection de)
        {
            
            return de.Query<SoldPackets>("EXECUTE GetAllRecords SoldPackets,Id,-1,' Where SoldDateString=''" + DateTime.Now.Date.ToString("dd-MM-yyyy") + "'' AND SoldTo=" + id + " '").FirstOrDefault();
        }

        public SoldPackets GetActiveSoldPacketsById(int Id, SqlConnection de)
        {
            return de.Query<SoldPackets>("EXECUTE GetAllRecords SoldPackets,Id," + Id + "").FirstOrDefault();
        }

        public async Task<bool> AddSoldPackets(SoldPackets _soldPackets, SqlConnection de)
        {
            try
            {
                _soldPackets.SoldDate = _soldPackets.SoldDate.Value.Date;
                _soldPackets.SoldDateString = _soldPackets.SoldDate.Value.Date.ToString("dd-MM-yyyy");
                if(_soldPackets.PickedBy == 0)
                {
                    _soldPackets.PickedBy = new UserDAL().GetActiveUserByRole(1,de).FirstOrDefault().Id;
                }
                var getPropandVal = new UserDAL().GetPropandVal(_soldPackets);
                
                var add = await de.QueryAsync("EXECUTE InsertOrUpdate 0,SoldPackets," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateSoldPackets(SoldPackets _soldPackets, SqlConnection de)
        {
            try
            {
                var getPropandVal = new UserDAL().GetUpdatePropandVal(_soldPackets);
                var update = await de.QueryAsync("EXECUTE InsertOrUpdate " + _soldPackets.Id + ",SoldPackets,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> DeleteSoldPackets(int id, SqlConnection de)
        {
            try
            {
                SoldPackets u = GetActiveSoldPacketsById(id, de);

                u.IsActive = 0;
                u.DeletedAt = DateTime.UtcNow;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(u);
                var delete = await de.QueryAsync("EXECUTE InsertOrUpdate " + u.Id + ",SoldPackets,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
