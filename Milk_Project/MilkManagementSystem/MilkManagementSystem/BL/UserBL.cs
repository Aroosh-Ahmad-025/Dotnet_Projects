using MilkManagementSystem.DAL;
using MilkManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilkManagementSystem.BL
{
    public class UserBL
    {
        public List<User> GetAllUsersList(SqlConnection de)
        {
            return new UserDAL().GetAllUsersList(de);
        }

        public List<User> GetRegularCustomersList(SqlConnection de)
        {
            return new UserDAL().GetRegularCustomersList(de);
        }


        public List<User> GetActiveUserList(SqlConnection de)
        {
            return new UserDAL().GetActiveUserList(de);
        }


        public User GetUserById(int id, SqlConnection de)
        {
            return new UserDAL().GetUserById(id, de);
        }

        public User GetActiveUserById(int id, SqlConnection de)
        {
            return new UserDAL().GetActiveUserById(id, de);
        }
        public async Task<bool> AddUser(User _user, SqlConnection de)
        {
            if (String.IsNullOrEmpty(_user.Contact_No) || String.IsNullOrEmpty(_user.Role.ToString()))
                return false;

            return await new UserDAL().AddUser(_user, de);
        }    

        public async Task<bool> UpdateUser(User _user, SqlConnection de)
        {
            return await new UserDAL().UpdateUser(_user, de);
        }

        public async Task<bool> DeleteUser(int id, SqlConnection de)
        {
            return await new UserDAL().DeleteUser(id, de);
        }
        public Task<string> UploadImage(IFormFile ImagePath)
        {
            return new UserDAL().FileUpload(ImagePath);
        }

        public List<User> GetActiveUserByRole(int id, SqlConnection de)
        {
            return new UserDAL().GetActiveUserByRole(id, de);
        }
       
    }
}
