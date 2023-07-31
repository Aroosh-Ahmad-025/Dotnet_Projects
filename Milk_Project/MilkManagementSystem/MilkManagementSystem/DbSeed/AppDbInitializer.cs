using MilkManagementSystem.HelpingClasses;
using MilkManagementSystem.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilkManagementSystem.DbSeed
{
    public class AppDbInitializer
    {
        public static void DbSeed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.Users.Any())
                {
                    User obj = new User()
                    {
                        FullName = "Uzair Aslam",
                        Email = "uzair.aslam02@gmail.com",
                        Password = StringCipher.Encrypt("123"),
                        Role = 1,
                        IsActive = 1,
                        CreatedAt = GeneralPurpose.DateTimeNow(),
                        Address = "",
                        Contact_No="0000-0000000",
                        Salary= 0
                    };

                    User obj2 = new User()
                    {
                        FullName = "Kamran",
                        Email = "kamran@nodlays.com",
                        Password = StringCipher.Encrypt("123"),
                        Role = 1,
                        IsActive = 1,
                        CreatedAt = GeneralPurpose.DateTimeNow(),
                        Address = "",
                        Contact_No = "0000-0000000",
                        Salary = 0
                    };


                    context.Users.Add(obj);

                    context.SaveChanges();
                }

                

            }

        }

    }
}
