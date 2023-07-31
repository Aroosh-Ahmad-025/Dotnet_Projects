using Dapper;
using MilkManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MilkManagementSystem.DAL
{
    public class UserDAL
    {
        public List<User> GetAllUsersList(SqlConnection de)
        {
            return de.Query<User>("EXECUTE GetAllRecords Users").ToList();
        }
        public List<User> GetRegularCustomersList(SqlConnection de)
        {
            return de.Query<User>("EXECUTE GetAllRecords Users,IsRegular,1").ToList();
        }

        public List<User> GetActiveUserList(SqlConnection de)
        {
            return de.Query<User>("EXECUTE GetAllRecords Users,Id,-1, ' Where IsActive =1 '").ToList();
        }     

        public User GetUserById(int Id, SqlConnection de)
        {
            return de.Query<User>("EXECUTE GetAllRecords Users,Id,"+Id+"").FirstOrDefault();
        }

        public User GetActiveUserById(int Id, SqlConnection de)
        {
            return de.Query<User>("EXECUTE GetAllRecords Users,Id," + Id + "").FirstOrDefault();
        }




        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        [RequestSizeLimit(209715200)]
        public async Task<string> FileUpload(IFormFile ImagePath=null)
        {
            try
            {
                string DirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot/Images/User");

                if (ImagePath != null)
                {
                    if (!Directory.Exists(DirectoryPath))
                    {
                        Directory.CreateDirectory(DirectoryPath);
                    }

                    var FileExtention = Path.GetExtension(ImagePath.FileName);

                    string FileName = "Invoice" + "_" + DateTime.UtcNow.Ticks.ToString() + "" + FileExtention;
                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwRoot/Images/User/",
                        FileName);

                    var setPath = "Images/User/" + FileName;
                    FileExtention = FileExtention.ToLower();
                    if (FileExtention.Contains("jpg") || FileExtention.Contains("jpeg") || FileExtention.Contains("png") || FileExtention.Contains("svg") || FileExtention.Contains("webp"))
                    {
                        using var image = Image.Load(ImagePath.OpenReadStream());
                        image.Mutate(x => x.Resize(256, 256));
                        image.Save(path, new JpegEncoder { Quality = 100 });
                    }
                    else
                    {
                        using (var stream = new FileStream(path, FileMode.Create))
                        {

                           await ImagePath.CopyToAsync(stream);
                        }
                    }
                    var img = setPath;
                    return img;
                }
                return null;
            }
            catch
            {
                return null;
            }
            
        }

        public List<User> GetActiveUserByRole(int id, SqlConnection de)
        {
            return de.Query<User>("EXECUTE GetAllRecords Users,Role,"+id+"").ToList();
        }

        public async Task<bool> AddUser(User _user, SqlConnection de)
        {
            try
            {
                _user.Password = "";
                var getPropandVal = GetPropandVal(_user);
                var add = await de.QueryAsync("EXECUTE InsertOrUpdate 0,Users," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateUser(User _user, SqlConnection de)
        {
            try
            {
                var getPropandVal = GetUpdatePropandVal(_user);
                //var query = "EXECUTE InsertOrUpdate " + _user.Id + ",Users,'" + getPropandVal + "'";
                var update = await de.QueryAsync("EXECUTE InsertOrUpdate " + _user.Id + ",Users,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }




        public async Task<bool> DeleteUser(int id, SqlConnection de)
        {
            try
            {
                User u = GetActiveUserById(id, de);
             
                u.IsActive = 0;
                u.DeletedAt = DateTime.UtcNow;
                var getPropandVal = GetUpdatePropandVal(u);
                var delete =await de.QueryAsync("EXECUTE InsertOrUpdate " + u.Id + ",Users,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<string> GetPropandVal(object obj)
        {
            try
            {
                var propAndval = new List<string>();
              
                PropertyInfo[] properties = obj.GetType().GetProperties();

                var prop = new List<string>();
                var val = new List<string>();
                
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetValue(obj) != null && !property.GetValue(obj).ToString().Contains("["))
                    {
                        prop.Add(property.Name);
                        val.Add("''"+property.GetValue(obj).ToString()+"''");
                    }
                }
                prop = prop.Skip(1).ToList();
                val = val.Skip(1).ToList();
                

                propAndval.Add("'"+String.Join(",", prop)+"'");
                propAndval.Add("'" +String.Join(",", val) + "'");


                return propAndval;
            }
            catch
            {
                return null;
            }
        }
        public string GetUpdatePropandVal(object obj)
        {
            try
            {
                 
                PropertyInfo[] properties = obj.GetType().GetProperties();

                var prop = new List<string>();
               
                
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetValue(obj)!=null && !property.GetValue(obj).ToString().Contains("[") && !property.GetValue(obj).ToString().Contains("Models"))
                    {
                        prop.Add(property.Name + " = ''" + property.GetValue(obj).ToString()+"''");
                    }
                }
                prop = prop.Skip(1).ToList();



                var getcolandval = String.Join(",", prop);

                return getcolandval;
            }
            catch
            {
                return null;
            }
        }
    }
}
