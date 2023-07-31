using CarSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSystem.DAL
{
    public class UserDAL
    {
        public List<User> GetActiveUsersList(DatabaseEntities de)
        {
            return de.Users.Where(x => x.IsActive == 1).ToList();
        }

        public User GetActiveUserById(int id, DatabaseEntities de)
        {
            try
            {
                return de.Users.Where(x => x.Id == id).FirstOrDefault(x => x.IsActive == 1);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public bool AddUser(User user, DatabaseEntities de)
        {
            try
            {
                de.Users.Add(user);
                de.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateUser(User user, DatabaseEntities de)
        {
            try
            {
                de.Entry(user).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        //public bool DeleteUser(int id, DatabaseEntities de)
        //{
        //    try
        //    {
        //        de.Users.Remove(de.Users.Where(x => x.Id == id).FirstOrDefault());
        //        de.SaveChanges();

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
    }
}