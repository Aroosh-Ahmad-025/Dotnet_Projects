using MilkManagementSystem.BL;
using MilkManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MilkManagementSystem.Controllers;
using static MilkManagementSystem.HelpingClasses.ProjectVariables;
using System.Net.NetworkInformation;
using Microsoft.Data.SqlClient;

namespace MilkManagementSystem.HelpingClasses
{
    //In order to use this class as service so that we can access it from front end we
    //need to register it as transient service, copy the following code in Startup.cs class
    //services.AddTransient<GeneralPurpose>();
    public class GeneralPurpose
    {
        private SqlConnection de;
        //need to register HttpContextAccessor in startup.cs class
        //copy the following code in startup.cs
        //services.AddHttpContextAccessor();
        private HttpContext hcontext;

        private IHttpContextAccessor haccess;
        public GeneralPurpose(SqlConnection de, IHttpContextAccessor haccess)
        {
            this.de = de;
            this.haccess = haccess;
            hcontext = haccess.HttpContext;
        }


        public static DateTime DateTimeNow()
        {
            return DateTime.UtcNow;
        }

        public bool ValidateEmail(string email = "", int id = -1)
        {
            int emailCount = 0;

            if (id != -1)
            {
                emailCount = new UserBL().GetAllUsersList(de).Where(x => x.IsActive != 0 && x.Email.ToLower() == email.ToLower() && x.Id != id).Count();
            }
            else
            {
                emailCount = new UserBL().GetAllUsersList(de).Where(x => x.IsActive != 0 && x.Email.ToLower() == email.ToLower()).Count();
            }

            if (emailCount > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckInternet()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ValidateUsername(string username = "", int id = -1)
        {
            int userCount = 0;

            if (id != -1)
            {
                userCount = new UserBL().GetAllUsersList(de).Where(x => x.IsActive != 0 && x.Id != id).Count();
            }
            else
            {
                userCount = new UserBL().GetAllUsersList(de).Where(x => x.IsActive != 0).Count();
            }

            if (userCount > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
