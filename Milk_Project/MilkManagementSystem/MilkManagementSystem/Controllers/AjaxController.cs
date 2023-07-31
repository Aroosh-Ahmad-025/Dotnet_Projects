using MilkManagementSystem.BL;
using MilkManagementSystem.Filters;
using MilkManagementSystem.HelpingClasses;
using MilkManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static MilkManagementSystem.HelpingClasses.ProjectVariables;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MilkManagementSystem.Controllers
{
    public class AjaxController : Controller
    {
        private readonly SqlConnection de;
        private readonly GeneralPurpose gp;
        private readonly IConfiguration confg;

        public AjaxController(IConfiguration confg, IHttpContextAccessor haccess)
        {
            this.de = new SqlConnection(confg.GetConnectionString("Default"));
            this.gp = new GeneralPurpose(de, haccess);
            this.confg = confg;
            var request = haccess.HttpContext.Request;
           
            baseUrl = $"{request.Scheme}://{request.Host}";
        }

        #region User
       
        [HttpPost]
        public IActionResult GetUserDataTableList(int category = -1, string Name = "", string email = "",int isRegular =-1)
        {

            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            List<User> ulist = new UserBL().GetAllUsersList(de).Where(x=> x.Role != 1).ToList();
            if (category == 3)
            {
                ulist = ulist.Where(x => x.Role == 3).ToList();
            }  
            if (category == 2)
            {
                ulist = ulist.Where(x => x.Role == 2).ToList();
            }

            if(isRegular !=-1)
            {
                ulist = ulist.Where(x => x.IsRegular == isRegular).ToList();
            }


            if (!String.IsNullOrEmpty(Name))
            {
                ulist = ulist.Where(x => x.FullName.ToLower().Contains(Name.ToLower())).ToList();
            }

            if (!String.IsNullOrEmpty(email))
            {
                ulist = ulist.Where(x => x.Email.ToLower().Contains(email.ToLower())).ToList();
            }

            int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            string searchValue = Request.Form["search[value]"].FirstOrDefault();
            string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
            string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        ulist = ulist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        ulist = ulist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = ulist.Count();

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                ulist = ulist.Where(x => x.Email.ToLower().Contains(searchValue.ToLower()) || x.FullName.ToLower().Contains(searchValue.ToLower()) || x.Contact_No.Contains(searchValue.ToLower()) || x.Address.Contains(searchValue.ToLower())
                                    ).ToList();
            }

            int totalrowsafterfilterinig = ulist.Count();


            // pagination
            ulist = ulist.Skip(start).Take(length).ToList();

            

           

            return Json(new { data = ulist, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        }

        public IActionResult GetAllCustomers()
        {
            var getallCustomers = new UserBL().GetActiveUserByRole(2, de);
            return Json(getallCustomers);
        }


        public IActionResult GetAllRiders()
        {
            var getallRiders = new UserBL().GetActiveUserByRole(3, de);
            return Json(getallRiders);
        }


        [HttpPost]
        public IActionResult GetUserById(int id)
        {
            User u = new UserBL().GetActiveUserById(id, de);
            if (u == null)
            {
                return Json(0);
            }

            User obj = new User()
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Contact_No = u.Contact_No,
                Password = StringCipher.Decrypt(u.Password),
                Address = u.Address,
                Salary = u.Salary,
                IsRegular = u.IsRegular,
                IsActive = u.IsActive,
                Role = u.Role,
              

            };

            return Json(obj);
        }

        [HttpPost]
        public IActionResult fileUpload(IFormFile File)
        {
            try
            {
                var img = new UserBL().UploadImage(File);
                return Json(img.Result);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region ValidateUser
        [HttpPost]
        public IActionResult ValidateEmail(string email, int id = -1)
        {
            return Json(gp.ValidateEmail(email, id));
        }

        [HttpPost]
        public IActionResult ValidateUsername(string username, int id = -1)
        {
            return Json(gp.ValidateUsername(username, id));
        }
        #endregion

        #region Packets

        [HttpPost]
        public IActionResult GetActivePacketDataTableList(int category =-1, int quantity=-1)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            List<Packets> ulist = new PacketsBL().GetActivePacketsList(de);
          

            if (category!=-1)
            {
                ulist = ulist.Where(x => x.Category == category ).ToList();
            }

            if(quantity!=-1)
            {
                ulist = ulist.Where(x => x.TotalQuantity == quantity).ToList();
            }

           
            int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            string searchValue = Request.Form["search[value]"].FirstOrDefault();
            string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
            string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

           

            List<Packets> udto = new List<Packets>();

            var soldPackets = new SoldPacketsBL().GetActiveSoldPacketsList(de);
            foreach (Packets u in ulist)
            {
             
                Packets obj = new Packets()
                {
                    Id = u.Id,
                    Category = u.Category,
                    TotalQuantity = u.TotalQuantity,
                    CreatedAt = u.CreatedAt,
                    AddedDate = u.AddedDate,
                    IsActive = (int)u.IsActive
                };

                udto.Add(obj);
            }


            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortColumnName != "0")
                {
                    if (sortDirection == "asc")
                    {
                        udto = udto.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                    else
                    {
                        udto = udto.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                    }
                }
            }

            int totalrows = udto.Count();

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {
                if ("buffalo".Contains(searchValue.ToLower()))
                {
                    udto = udto.Where(x => x.Category == 1).ToList();
                }
                else if ("cow".Contains(searchValue.ToLower()))
                {
                    udto = udto.Where(x => x.Category == 2).ToList();
                }
                else if ("buffalo+cow".Contains(searchValue.ToLower()))
                {
                    udto = udto.Where(x => x.Category == 3).ToList();
                }
            }

            int totalrowsafterfilterinig = udto.Count();


            // pagination
            udto = udto.Skip(start).Take(length).ToList();


            return Json(new { data = udto, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        
        }


        [HttpPost]
        public IActionResult GetPacketById(int id)
        {
            Packets u = new PacketsBL().GetActivePacketsById(id, de);
            if (u == null)
            {
                return Json(0);
            }

            return Json(u);
        }
        #endregion

        #region SoldPackets

        [HttpPost]
        public IActionResult GetSoldPacketsDataTableList(string dropOff_Location ="", string pickUp_Location= "", int Total_Distance =-1, int pricePer=-1, string soldDate="",string userName="",int customerId =-1, Nullable<DateTime> fromDate=null, Nullable<DateTime> toDate=null)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            List<SoldPackets> ulist = new SoldPacketsBL().GetActiveSoldPacketsList(de);


            if(customerId!=-1)
            {
                ulist = ulist.Where(a => a.SoldTo == customerId).ToList();
            }

            int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            string searchValue = Request.Form["search[value]"].FirstOrDefault();
            string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
            string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            int totalrows = ulist.Count();

            List<Sold_Packet_Delivery_DTO> udto = new List<Sold_Packet_Delivery_DTO>();
            string prevdate = "";
            int previd = 0;

            foreach (SoldPackets u in ulist)
            {
               
              
                    Sold_Packet_Delivery_DTO obj = new Sold_Packet_Delivery_DTO()
                    {
                        Id = u.Id,
                        PricePerPacket = u.PricePerPacket,
                        DropOff_Location = u.DropOff_Location,
                        Pickup_Location = u.Pickup_Location,
                        Total_Distance = u.Total_Distance,
                        SoldDate = u.SoldDate,
                        Category = u.Category,
                        FromDate = u.FromDate.Value,
                        ToDate = u.ToDate.Value,
                        SoldDateString = u.SoldDateString,
                        Quantity = u.Quantity,
                        Rider = new UserBL().GetActiveUserById(u.PickedBy, de),
                        Customer = new UserBL().GetActiveUserById(u.SoldTo, de),
                    };

                   
                    udto.Add(obj);
                
                    prevdate = u.SoldDateString;
                    previd = u.Id;

            }

            if (sortColumnName != "" && sortColumnName != null)
            {

                if (sortColumnName != "0")
                {
                   
                    if (sortDirection == "asc")
                    {
                        try
                        {
                            udto = udto.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();

                        }
                        catch
                        {
                            udto = udto.OrderByDescending(x => x.Customer.GetType().GetProperty(sortColumnName).GetValue(x.Customer)).ToList();
                        }
                    }
                    else
                    {
                        try
                        {
                            udto = udto.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                        }
                        catch
                        {
                            
                            udto = udto.OrderBy(x => x.Customer.GetType().GetProperty(sortColumnName).GetValue(x.Customer)).ToList();
                        }
                           
                    }
                }
            }

            //distance filter
            if (Total_Distance != -1)
            {
                udto = udto.Where(x => x.Total_Distance == Total_Distance).ToList();
            }

            //dropoff filter
            if(!string.IsNullOrEmpty(dropOff_Location))
            {
                udto = udto.Where(x => x.DropOff_Location.ToLower().Contains(dropOff_Location.ToLower())).ToList();
            }

            //pickup filter
            if (!string.IsNullOrEmpty(pickUp_Location))
            {
                udto = udto.Where(x => x.Pickup_Location.ToLower().Contains(pickUp_Location.ToLower())).ToList();
            }

            //userName filter
            if (!string.IsNullOrEmpty(userName))
            {
                udto = udto.Where(x => x.Customer.FullName.ToLower().Contains(userName.ToLower())).ToList();
            }

            //soldDate filter
            if (!string.IsNullOrEmpty(soldDate))
            {
                udto = udto.Where(x => x.SoldDate.Value.ToString("dd-MM-yyyy") == Convert.ToDateTime(soldDate).ToString("dd-MM-yyyy")).ToList();
            }


            //fromDate filter
            if (fromDate!=null)
            {
                udto = udto.Where(x => x.FromDate.Value.ToString("dd-MM-yyyy") == Convert.ToDateTime(fromDate.Value).ToString("dd-MM-yyyy")).ToList();
            }

            //fromDate filter
            if (toDate != null)
            {
                udto = udto.Where(x => x.ToDate.Value.ToString("dd-MM-yyyy") == Convert.ToDateTime(toDate.Value).ToString("dd-MM-yyyy")).ToList();
            }

            //filter
            if (!string.IsNullOrEmpty(searchValue))
            {

                udto = udto.Where(x => x.Pickup_Location.ToLower().Contains(searchValue.ToLower()) || x.DropOff_Location.ToLower().Contains(searchValue.ToLower()) || x.PricePerPacket.ToString() == searchValue || x.Total_Distance.ToString() == searchValue || x.Customer.FullName.ToLower().Contains(searchValue.ToLower()) || x.Customer.Address.ToLower().Contains(searchValue.ToLower())).ToList();

            }

            int totalrowsafterfilterinig = udto.Count();

            // pagination
            udto = udto.Skip(start).Take(length).ToList();


            return Json(new { data = udto, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });

        }


        [HttpPost]
        public IActionResult GetSoldPacketById(int id)
        {
            SoldPackets u = new SoldPacketsBL().GetActiveSoldPacketsById(id, de);
            if (u == null)
            {
                return Json(0);
            }

            Sold_Packet_Delivery_DTO obj = new Sold_Packet_Delivery_DTO()
            {
                Id = u.Id,
                PricePerPacket = u.PricePerPacket,
                DropOff_Location = u.DropOff_Location,
                Pickup_Location = u.Pickup_Location,
                Total_Distance = u.Total_Distance,
                Quantity = u.Quantity,
                Category = u.Category,
                FromDate = u.FromDate.Value,
                ToDate = u.ToDate.Value,
                SoldDateString = u.SoldDate.Value.ToString("yyyy-MM-ddThh:mm"),
                Rider = new UserBL().GetActiveUserById(u.PickedBy, de),
                Customer = new UserBL().GetActiveUserById(u.SoldTo, de),
             
            };

            return Json(obj);
        }


        #endregion

        #region Pending Packets

        //[HttpPost]
        //public IActionResult GetActivePendingPacketsList(string dropOff_Location = "", string pickUp_Location = "", int Total_Distance = -1, int pricePer = -1, string soldDate = "", string userName = "")
        //{
        //    var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        //    var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
        //    List<SoldPackets> ulist = new SoldPacketsBL().GetActivePendingPacketsList(de);


        //    int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
        //    int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
        //    string searchValue = Request.Form["search[value]"].FirstOrDefault();
        //    string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
        //    string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();



        //    int totalrows = ulist.Count();

        //    List<Sold_Packet_Delivery_DTO> udto = new List<Sold_Packet_Delivery_DTO>();


        //    foreach (SoldPackets u in ulist)
        //    {

        //        Sold_Packet_Delivery_DTO obj = new Sold_Packet_Delivery_DTO()
        //        {
        //            Id = u.Id,
        //            PricePerPacket = u.PricePerPacket,
        //            DropOff_Location = u.DropOff_Location,
        //            Pickup_Location = u.Pickup_Location,
        //            Total_Distance = u.Total_Distance,
        //            SoldDate = u.SoldDate,
        //            Category = u.Category,
        //            SoldDateString = u.SoldDate.Value.ToString("dd-MM-yyyy"),
        //            Quantity = u.Quantity,
        //            Rider = new UserBL().GetActiveUserById(u.PickedBy, de),
        //            Customer = new UserBL().GetActiveUserById(u.SoldTo, de),
        //        };

        //        udto.Add(obj);
        //    }

        //    if (sortColumnName != "" && sortColumnName != null)
        //    {

        //        if (sortColumnName != "0")
        //        {

        //            if (sortDirection == "asc")
        //            {
        //                try
        //                {
        //                    udto = udto.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();

        //                }
        //                catch
        //                {
        //                    udto = udto.OrderByDescending(x => x.Customer.GetType().GetProperty(sortColumnName).GetValue(x.Customer)).ToList();
        //                }
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    udto = udto.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
        //                }
        //                catch
        //                {

        //                    udto = udto.OrderBy(x => x.Customer.GetType().GetProperty(sortColumnName).GetValue(x.Customer)).ToList();
        //                }

        //            }
        //        }
        //    }

        //    //distance filter
        //    if (Total_Distance != -1)
        //    {
        //        udto = udto.Where(x => x.Total_Distance == Total_Distance).ToList();
        //    }

        //    //dropoff filter
        //    if (!string.IsNullOrEmpty(dropOff_Location))
        //    {
        //        udto = udto.Where(x => x.DropOff_Location.ToLower().Contains(dropOff_Location.ToLower())).ToList();
        //    }

        //    //pickup filter
        //    if (!string.IsNullOrEmpty(pickUp_Location))
        //    {
        //        udto = udto.Where(x => x.Pickup_Location.ToLower().Contains(pickUp_Location.ToLower())).ToList();
        //    }

        //    //userName filter
        //    if (!string.IsNullOrEmpty(userName))
        //    {
        //        udto = udto.Where(x => x.Customer.FullName.ToLower().Contains(userName.ToLower())).ToList();
        //    }

        //    //soldDate filter
        //    if (!string.IsNullOrEmpty(soldDate))
        //    {
        //        udto = udto.Where(x => x.SoldDate.Value.ToString("dd-MM-yyyy") == Convert.ToDateTime(soldDate).ToString("dd-MM-yyyy")).ToList();
        //    }

        //    //filter
        //    if (!string.IsNullOrEmpty(searchValue))
        //    {

        //        udto = udto.Where(x => x.Pickup_Location.ToLower().Contains(searchValue.ToLower()) || x.DropOff_Location.ToLower().Contains(searchValue.ToLower()) || x.PricePerPacket.ToString() == searchValue || x.Total_Distance.ToString() == searchValue || x.Customer.FullName.ToLower().Contains(searchValue.ToLower()) || x.Customer.Address.ToLower().Contains(searchValue.ToLower())).ToList();

        //    }

        //    int totalrowsafterfilterinig = udto.Count();

        //    // pagination
        //    udto = udto.Skip(start).Take(length).ToList();


        //    return Json(new { data = udto, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });

        //}

        #endregion

        #region Dashboard Search
        public IActionResult GetAssetsCount(Nullable<DateTime> fromDate=null, Nullable<DateTime> toDate =null)
        {
            List<double> Assets= new List<double>();

            var customersCount = new UserBL().GetActiveUserByRole(2, de);
            var ridersCount = new UserBL().GetActiveUserByRole(3, de);
            var totalPacketsCount = new PacketsBL().GetActivePacketsList(de);
            var solPacketsCount = new SoldPacketsBL().GetActiveSoldPacketsList(de);
            var regularCustomersCount = new UserBL().GetRegularCustomersList(de);

            if(fromDate!=null)
            {
                customersCount = customersCount.Where(a => a.CreatedAt.Value >= fromDate.Value).ToList();
                ridersCount = ridersCount.Where(a => a.CreatedAt.Value >= fromDate.Value).ToList();
                totalPacketsCount = totalPacketsCount.Where(a => a.CreatedAt.Value >= fromDate.Value).ToList();
                solPacketsCount = solPacketsCount.Where(a => a.CreatedAt.Value >= fromDate.Value).ToList();
                regularCustomersCount = regularCustomersCount.Where(a => a.CreatedAt.Value >= fromDate.Value).ToList();
            }



            if (toDate!= null)
            {
                customersCount = customersCount.Where(a => a.CreatedAt.Value <= toDate.Value).ToList();
                ridersCount = ridersCount.Where(a => a.CreatedAt.Value <= toDate.Value).ToList();
                solPacketsCount = solPacketsCount.Where(a => a.CreatedAt.Value <= toDate.Value).ToList();
                totalPacketsCount = totalPacketsCount.Where(a => a.CreatedAt.Value <= toDate.Value).ToList();
                regularCustomersCount = regularCustomersCount.Where(a => a.CreatedAt.Value <= toDate.Value).ToList();
            }
            
            var nonregular = customersCount.Count() - regularCustomersCount.Count();

            //buffalo total and sold
            var buffaloPackets = totalPacketsCount.Where(a => a.Category == 1).Select(a => a.TotalQuantity).Sum();
            var buffaloPacketsSold = solPacketsCount.Where(a => a.Category == 1).Select(a => a.Quantity).Sum();

            //cow total and sold
            var cowPackets = totalPacketsCount.Where(a => a.Category == 2).Select(a => a.TotalQuantity).Sum();
            var cowPacketsSold = solPacketsCount.Where(a => a.Category == 2).Select(a => a.Quantity).Sum();

            //buffalo Plus cow
            var buffaloPcow = totalPacketsCount.Where(a => a.Category == 3).Select(a => a.TotalQuantity).Sum();
            var buffaloPcowSold = solPacketsCount.Where(a => a.Category == 3).Select(a => a.Quantity).Sum();


            Assets.Add(nonregular);

            Assets.Add(ridersCount.Count());

            Assets.Add(solPacketsCount.Select(a => a.Quantity).Sum());
            Assets.Add(totalPacketsCount.Select(a => a.TotalQuantity).Sum());

            Assets.Add(regularCustomersCount.Count());

            Assets.Add(buffaloPackets);
            Assets.Add(buffaloPacketsSold);

            Assets.Add(cowPackets);
            Assets.Add(cowPacketsSold);

            Assets.Add(buffaloPcow);
            Assets.Add(buffaloPcowSold);

            return Json(Assets);
        }

        #endregion

    }


}

