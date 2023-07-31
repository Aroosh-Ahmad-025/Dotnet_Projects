using Dapper;
using LoginFinal.BL;
using LoginFinal.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoginFinal.DAL
{
    public class ChatDAL
    {
        public readonly SqlConnection db;

        public ChatDAL(IConfiguration confg)
        {
            this.db = new SqlConnection(confg.GetConnectionString("Default"));
        }
        public List<Message> GetAllChats()
        {
            List<Message> clist = db.Query<Message>("EXECUTE GetAllRecords Messages").ToList();
            foreach (Message c in clist)
            {
                var getSender = new UserBL().GetActiveUserById(c.SenderId, db);
                var getReciever = new UserBL().GetActiveUserById(c.RecieverId, db);
                c.Users = getSender;
                c.RecieverEnd = getReciever;
            }
            return clist;
        }

        public Message GetChatById(int _id)
        {
            Message chat = new Message();

            chat = db.Query<Message>("EXECUTE GetAllRecords Messages,Id," + _id + "").FirstOrDefault();
            chat.RecieverEnd = new UserBL().GetActiveUserById(chat.RecieverId, db);
            chat.Users = new UserBL().GetActiveUserById(chat.SenderId, db);

            return chat;
        }

        public int AddChat(Message _chat)
        {
            try
            {
                _chat.Message_Description = _chat.Message_Description.Replace("'", "''''");
                
                var getPropandVal = new UserDAL().GetPropandVal(_chat);
                var qr = "EXECUTE InsertOrUpdate 0,Messages," + getPropandVal[0] + "," + getPropandVal[1] + "";
                var add = db.Query("EXECUTE InsertOrUpdate 0,Messages," + getPropandVal[0] + "," + getPropandVal[1] + "").FirstOrDefault();

                return Convert.ToInt32(add.Id) ;
            }
            catch
            {
                return -1;
            }
        }


        public bool UpdateChat(int id, string modified_msg = "")
        {
            try
            {
                var find_msg = GetChatById(id);
                find_msg.Message_Description = modified_msg;
                find_msg.UpdatedAt = DateTime.UtcNow;
                var getPropandVal = new UserDAL().GetUpdatePropandVal(find_msg);
                var update = db.Query("EXECUTE InsertOrUpdate " + find_msg.Id + ",Messages,'" + getPropandVal + "'");
                return true;

            }
            catch
            {
                return false;
            }
        }

        //public bool ClearUnreadChart(int _id)
        //{
        //    try
        //    {
        //        using (db)
        //        {
        //            db.Messages.Where(x => x.SenderId == _id && x.IsRead == 0).ToList().ForEach(x => x.IsRead = 1);
        //            db.SaveChanges();
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public bool DeleteChat(int _id)
        //{
        //    try
        //    {
        //        db.Messages.RemoveRange(db.Messages.Where(X => X.Id == _id));
        //        db.SaveChanges();

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
    }
}