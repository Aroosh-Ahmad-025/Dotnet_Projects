using MilkManagementSystem.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static MilkManagementSystem.HelpingClasses.ProjectVariables;
using MilkManagementSystem.BL;
using System.Linq;
using System;
using System.Security.Claims;
using MilkManagementSystem.Models;
using System.Threading.Tasks;
using MilkManagementSystem.HelpingClasses;
using Newtonsoft.Json;

namespace MilkManagementSystem.Controllers
{
    [Authorize]
    [ExceptionFilter]
    public class AdminController : Controller
    {
        private readonly SqlConnection de;

        public AdminController(IConfiguration confg, IHttpContextAccessor haccess)
        {
            this.de = new SqlConnection(confg.GetConnectionString("Default"));

            var request = haccess.HttpContext.Request;
            baseUrl = $"{request.Scheme}://{request.Host}";
        }

        #region Landing Page Admin
        public IActionResult Index()
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        #region Manage User

        public IActionResult AddUser(string msg = "", string color = "black", int cat = 0)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                ViewBag.Message = msg;
                ViewBag.Color = color;
                ViewBag.cat = cat;
                ViewBag.Role = CurrentUserRecord.Role;
                ViewBag.AdminId = CurrentUserRecord.Id;
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> PostAddUser(User _user )
        {
            _user.IsActive = 1;
            _user.CreatedAt = GeneralPurpose.DateTimeNow();

            if(string.IsNullOrEmpty(_user.Email))
            {
                _user.Email = "";
            }
         
            if (!await new UserBL().AddUser(_user, de))
            {
                return RedirectToAction("AddUser", "Admin", new { cat = _user.Role, msg = "Somethings' Wrong", color = "red" });
            }

            return RedirectToAction("AddUser", "Admin", new { msg = "Record Inserted Successfully", color = "green", cat = _user.Role });
        }

        // [ValidationFilter(Roles = new int[] { 1 })]
        public IActionResult ViewUser(int category = -1, int isRegular =-1, string msg = "", string color = "black")
        {

            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                if (CurrentUserRecord.Role == 1)
                {

                    ViewBag.category = category;
                    ViewBag.isRegular = isRegular;
                    ViewBag.Message = msg;
                    ViewBag.Color = color;
                    return View();
                   
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
                
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> PostUpdateUser(User _user, string way = "")
        {
            User user = new UserBL().GetActiveUserById(_user.Id, de);

            if (user == null)
            {
                return RedirectToAction("ViewUser", "Admin", new { category = user.Role, msg = "Record not found", color = "red", way = way });
            }
            if (_user.Email != null)
            {
                user.Email = _user.Email;
            }


            if (_user.Password != null)
            {
                user.Password = StringCipher.Encrypt(_user.Password);
            }

            if (!string.IsNullOrEmpty(_user.FullName))
            {
                user.FullName = _user.FullName;
            }


            if (!string.IsNullOrEmpty(_user.Contact_No))
            {
                user.Contact_No = _user.Contact_No;
            }


            if (!string.IsNullOrEmpty(_user.Address))
            {
                user.Address = _user.Address;
            }


        
            user.IsRegular = _user.IsRegular;
            


            bool chkUser = await new UserBL().UpdateUser(user, de);
            if (chkUser)
            {
                if (user.Role != 1)
                {
                    return RedirectToAction("ViewUser", "Admin", new { isRegular = user.IsRegular, category = user.Role, msg = "User updated successfully", color = "green", way = way });
                }
                else
                {
                    return RedirectToAction("UpdateProfile", "Auth", new { isRegular = user.IsRegular, category = user.Role, msg = "Profile updated successfully", color = "green" });
                }
            }
            return RedirectToAction("ViewUser", "Admin", new { isRegular = user.IsRegular, category = user.Role, msg = "Somethings' wrong", color = "red", way = way });
        }


        public async Task<IActionResult> DeleteUser(int id)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                if (CurrentUserRecord.Role == 1)
                {
                    var r = new UserBL().GetActiveUserById(id, de);
                    if (!await new UserBL().DeleteUser(id, de))
                    {
                        return RedirectToAction("ViewUser", "Admin", new { category = r, msg = "Somethings' wrong", color = "red" });

                    }

                    return RedirectToAction("ViewUser", "Admin", new { msg = "Record deleted successfully!", color = "green", category = r });
                }
                else
                {
                    return RedirectToAction("NotFoundPage", "Error");
                }
            }
            catch
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
        }



        #endregion

        #region Packets

        public IActionResult AddPackets(string msg="",string color="black")
        {
            try
            {
                var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
                var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
                ViewBag.Message = msg;
                ViewBag.Color = color;
                ViewBag.AdminId = CurrentUserRecord.Id;
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAddPackets(Packets packets)
        {
            packets.IsActive = 1;
            packets.CreatedAt = GeneralPurpose.DateTimeNow();


            if (!await new PacketsBL().AddPackets(packets, de))
            {
                return RedirectToAction("AddPackets", "Admin", new { msg = "Somethings' Wrong", color = "red" });
            }

            return RedirectToAction("AddPackets", "Admin", new { msg = "Record Inserted Successfully", color = "green" });
        

        }

        public IActionResult ViewPackets(int category = -1, string msg = "", string color = "black")
        {

            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                if (CurrentUserRecord.Role == 1)
                {

                    ViewBag.category = category;
                    ViewBag.Message = msg;
                    ViewBag.Color = color;
                    return View();

                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }

            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> PostUpdatePackets(Packets packets, string way = "")
        {
            Packets packet = new PacketsBL().GetActivePacketsById(packets.Id, de);

            if (packet == null)
            {
                return RedirectToAction("ViewPackets", "Admin", new {msg = "Record not found", color = "red", way = way });
            }

            packet.TotalQuantity = packets.TotalQuantity;
            packet.AddedDate = packets.AddedDate;
            packet.Category = packets.Category;

            bool result = await new PacketsBL().UpdatePackets(packet, de);
            if (result)
            {
               
                return RedirectToAction("ViewPackets", "Admin", new { msg = "Packets updated successfully", color = "green", way = way });
              
            }
            return RedirectToAction("ViewPackets", "Admin", new { msg = "Somethings' wrong", color = "red", way = way });
        }

        public async Task<IActionResult> DeletePackets(int id)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                if (CurrentUserRecord.Role == 1)
                {
                    
                    if (!await new PacketsBL().DeletePackets(id, de))
                    {
                        return RedirectToAction("ViewPackets", "Admin", new { msg = "Somethings' wrong", color = "red" });

                    }

                    return RedirectToAction("ViewPackets", "Admin", new { msg = "Record deleted successfully!", color = "green" });
                }
                else
                {
                    return RedirectToAction("NotFoundPage", "Error");
                }
            }
            catch
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
        }

        #endregion

        #region Sold Packets
        public IActionResult AddSoldPackets(string msg = "", string color = "black")
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                ViewBag.Message = msg;
                ViewBag.Color = color;
                ViewBag.AdminId = CurrentUserRecord.Id;
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostSoldPackets(SoldPackets packets)
        {
            packets.IsActive = 1;
            packets.CreatedAt = GeneralPurpose.DateTimeNow();


            var getTotal = new PacketsBL().GetActivePacketsList(de).Select(a => a.TotalQuantity).Sum();

            if(getTotal< packets.Quantity)
            {
                return RedirectToAction("AddSoldPackets", "Admin", new { msg = "Insufficient Packets Available !", color = "red" });
            }

            var IsSold = await new SoldPacketsBL().AddSoldPackets(packets,de);

           
            if (!IsSold)
            {
                return RedirectToAction("AddSoldPackets", "Admin", new { msg = "Somethings' Wrong", color = "red" });
            }

            return RedirectToAction("AddSoldPackets", "Admin", new { msg = "Record Inserted Successfully", color = "green" });


        }

        public IActionResult ViewSoldPackets(int category = -1, string msg = "", string color = "black")
        {

            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                if (CurrentUserRecord.Role == 1)
                {

                    ViewBag.category = category;
                    ViewBag.Message = msg;
                    ViewBag.Color = color;
                    return View();

                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }

            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> PostUpdateSoldPackets(SoldPackets packets, string way = "", string way2="")
        {
            SoldPackets packet = new SoldPacketsBL().GetActiveSoldPacketsById(packets.Id, de);

            if (packet == null)
            {
                return RedirectToAction("ViewSoldPackets", "Admin", new { msg = "Record not found", color = "red", way = way });
            }

            packet.PricePerPacket = packet.PricePerPacket;
            packet.Pickup_Location = packets.Pickup_Location;
            packet.DropOff_Location = packets.DropOff_Location;
            packet.SoldDate = packets.SoldDate;
            packet.Category = packets.Category;
            packet.Total_Distance = packets.Total_Distance;
            packet.Quantity = packets.Quantity;

            bool result = await new SoldPacketsBL().UpdateSoldPackets(packet, de);
            if (result)
            {
                
                return RedirectToAction("ViewSoldPackets", "Admin", new { msg = "Packets updated successfully", color = "green", way = way });

            }
            return RedirectToAction("ViewSoldPackets", "Admin", new { msg = "Somethings' wrong", color = "red", way = way });
        }

        public async Task<IActionResult> DeleteSoldPackets(int id)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                if (CurrentUserRecord.Role == 1)
                {

                    if (!await new SoldPacketsBL().DeleteSoldPackets(id, de))
                    {
                        return RedirectToAction("ViewSoldPackets", "Admin", new { msg = "Somethings' wrong", color = "red" });

                    }

                    return RedirectToAction("ViewSoldPackets", "Admin", new { msg = "Record deleted successfully!", color = "green" });
                }
                else
                {
                    return RedirectToAction("NotFoundPage", "Error");
                }
            }
            catch
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
        }

        #endregion
        
        #region Customer Sales

        public async Task<IActionResult> ViewCustomerSales(int id=-1,string msg="", string color="black")
        {
            ViewBag.msg = msg;
            ViewBag.color = color;
            ViewBag.UserId = id;

            var getCustomer = new UserBL().GetActiveUserById(id, de);
            var getCustomerSales = new SoldPacketsBL().GetActiveSoldPacketsList(de).Where(a=>a.SoldTo== id).ToList();
            ViewBag.CustomerName = getCustomer.FullName;
            ViewBag.Contact = getCustomer.Contact_No;
            ViewBag.TotalSoldToday = getCustomerSales.Where(a=>a.SoldDate.Value.ToString("dd-MM-yyyy")== DateTime.Now.ToString("dd-MM-yyyy")).Select(a=>a.Quantity).Sum();
            ViewBag.TotalSales = getCustomerSales.Select(a => a.Quantity).Sum();
            ViewBag.MonthSales = getCustomerSales.Where(a=> a.SoldDate.Value.Month == DateTime.Now.Month).Select(a => a.Quantity).Sum();

            if(getCustomer.IsRegular==1)
            {
                ViewBag.IsRegular = "Regular";
            }
            else
            {
                ViewBag.IsRegular = "Non-Regular";
            }


          

            return View();
        }


        #endregion

        #region Commented Pending Sales Code 

          //public async Task <IActionResult> SellPacket(int id)
        //{
        //    SoldPackets packet = new SoldPacketsBL().GetActiveSoldPacketsById(id, de);

        //    if (packet == null)
        //    {
        //        return RedirectToAction("ViewSoldPackets", "Admin", new { msg = "Record not found", color = "red" });
        //    }

        //    packet.IsActive = 0;

            
        //    await new SoldPacketsBL().UpdateSoldPackets(packet, de);

        //    var result = await SoldPending(packet);

        //    if (result)
        //    {

        //        return RedirectToAction("ViewSoldPackets", "Admin", new { msg = "Packets Sold successfully ! ", color = "green"});

        //    }

        //    return RedirectToAction("ViewPendingSales", "Admin", new { msg = "Insufficient Packets Available!", color = "red" });

         
        //}

        //public async Task<bool> SoldPending(SoldPackets packets)
        //{
        //    packets.IsActive = 1;
        //    packets.CreatedAt = GeneralPurpose.DateTimeNow();

        //    packets.SoldDate = DateTime.Now.Date;

        //    packets.IsSold = 1;

        //    var getAvaliablePackets = new PacketsBL().GetActivePacketsList(de);

        //    var getSoldPackets = new SoldPacketsBL().GetActiveSoldPacketsList(de);

        //    var IsSold = false;

        //    var total = getAvaliablePackets.Where(a => a.Category == packets.Category).Select(a=>a.TotalQuantity).Sum();

        //    var soldTotal = getSoldPackets.Where(a=>a.Category == packets.Category).Select(a => a.Quantity).Sum();

        //    var remaining = total - soldTotal;

        //    if (remaining <= 0 || packets.Quantity > remaining)
        //    {
                
        //            return false;
                
        //    }

        //    var getcustomerQuantity = packets.Quantity;

        //    var leftQuantity = getcustomerQuantity;

        //    int i = 0;

        //    try
        //    {
        //        while (getcustomerQuantity != 0)
        //        {
        //            var solds = getSoldPackets.Where(a => a.PacketId == getAvaliablePackets[i].Id && getAvaliablePackets[i].Category == packets.Category).Select(a => a.Quantity).Sum();
        //            var leftquantityForCurrentPacket = getAvaliablePackets[i].TotalQuantity - solds;

        //            if (leftquantityForCurrentPacket > 0)
        //            {
        //                if (leftquantityForCurrentPacket >= getcustomerQuantity)
        //                {

        //                    packets.Quantity = getcustomerQuantity;
        //                    getcustomerQuantity = 0; //breaks loop


        //                }
        //                else
        //                {

        //                    getcustomerQuantity = Convert.ToInt32(getcustomerQuantity - getAvaliablePackets[i].TotalQuantity);
        //                    packets.Quantity = Convert.ToInt32(leftQuantity - getcustomerQuantity);

        //                }
        //                packets.PacketId = getAvaliablePackets[i].Id;

        //                IsSold = await new SoldPacketsBL().AddSoldPackets(packets, de);
        //            }
        //            i++;
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }

        //    if(!IsSold)
        //    { 
        //        return false; 
            
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
        #endregion

    }
}
