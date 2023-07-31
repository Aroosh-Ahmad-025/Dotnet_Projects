using LoginFinal.HelpingClasses;
using LoginFinal.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginFinal.DbSeed
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
                        FirstName = "Usman",
                        LastName = "Ali",
                        Username = "usman",
                        Contact = "0000-0000000",
                        Email = "usman78056@gmail.com",
                        Password = StringCipher.Encrypt("123"),
                        Role = 1,
                        IsActive = 1,
                        CreatedAt = GeneralPurpose.DateTimeNow(),
                        Refferal_Code = "usman1",
                        Country = "Canada",
                        City = "Alberta ",
                        ZipCode="14207"
                    };

                    User obj2 = new User()
                    {
                        FirstName = "Michael",
                        LastName = "Michael",
                        Username = "Michael",
                        Contact = "0000-0000000",
                        Email = "micheal@gmail.com",
                        Country = "Canada",
                        Password = StringCipher.Encrypt("123"),
                        Role = 4,
                        IsActive = 1,
                        Refferal_Code = "michael2",
                        CreatedAt = GeneralPurpose.DateTimeNow(),
                        City = "Alberta",
                        ZipCode = "14207"
                    };
                    User obj3 = new User()
                    {
                        FirstName = "ian",
                        LastName = "ian",
                        Username = "ian",
                        Contact = "0000-0000000",
                        Email = "ian@gmail.com",
                        Password = StringCipher.Encrypt("123"),
                        Role = 3,
                        IsActive = 1,
                        Refferal_Code = "ian3",
                        CreatedAt = GeneralPurpose.DateTimeNow(),
                        Country = "Canada",
                        City = "Alberta",
                        ZipCode = "14207",
                        StartingFrom = "25"
                    };
                    User obj4 = new User()
                    {
                        FirstName = "Kevin",
                        LastName = "Hart",
                        Username = "Kevin",
                        Contact = "0000-0000000",
                        Email = "kevin@gmail.com",
                        Password = StringCipher.Encrypt("123"),
                        Role = 3,
                        IsActive = 1,
                        Refferal_Code = "kevin4",
                        CreatedAt = GeneralPurpose.DateTimeNow(),
                        Country = "Canada",
                        City = "Alberta",
                        ZipCode = "14207",
                        StartingFrom = "25"
                    };
                    User obj5 = new User()
                    {
                        FirstName = "Alex",
                        LastName = "Jones",
                        Username = "Alex",
                        Contact = "0000-0000000",
                        Email = "alex@gmail.com",
                        Password = StringCipher.Encrypt("123"),
                        Role = 3,
                        IsActive = 1,
                        Refferal_Code = "alex5",
                        CreatedAt = GeneralPurpose.DateTimeNow(),
                        Country = "Canada",
                        City = "Alberta",
                        ZipCode = "14207",
                        StartingFrom = "25"
                    };
                    context.Users.Add(obj);
                    context.Users.Add(obj2);
                    context.Users.Add(obj3);
                    context.Users.Add(obj4);
                    context.Users.Add(obj5);

                    context.SaveChanges();
                }

            }

        }

    }
}
