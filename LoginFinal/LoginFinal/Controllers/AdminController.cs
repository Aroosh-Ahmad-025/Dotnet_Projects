using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginFinal.Models;
using LoginFinal.BL;
using LoginFinal.HelpingClasses;
using Microsoft.AspNetCore.Http;
using static LoginFinal.HelpingClasses.ProjectVariables;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LoginFinal.Filters;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LoginFinal.Controllers
{
    [Authorize]
    [ExceptionFilter]
    public class AdminController : Controller
    {
        private readonly SqlConnection de;
        
        public AdminController(SqlConnection de, IConfiguration confg, IHttpContextAccessor haccess)
        {
            //this.de = new SqlConnection(confg.GetConnectionString("Default"));
            this.de = de;

            var request = haccess.HttpContext.Request;
            baseUrl = $"{request.Scheme}://{request.Host}";
        }

        #region Landing Page Admin
        
       
        public IActionResult Index()
        {
            var Userid =User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                if (CurrentUserRecord.Role == 1)
                {
                    ViewBag.Buyer = new UserBL().GetAllUsersList(de).Where(x => x.IsActive != 0 && x.Role == 4).Count();
                    ViewBag.Seller = new UserBL().GetAllUsersList(de).Where(x => x.IsActive != 0 && x.Role == 3).Count();

                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion


        //Start massam 06-01-23
        public IActionResult ViewUserUnderReview(int category = -1, string msg = "", string color = "black")
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
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //End massam 06-01-23

        #region Manage User

        public IActionResult AddUser(string msg = "", string color = "black", int cat=0)
        {
            var Userid =User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                if (CurrentUserRecord.Role == 1)
                {
                    ViewBag.Message = msg;
                    ViewBag.Color = color;
                    ViewBag.cat = cat;
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

       
        [HttpPost]
        public async Task<IActionResult> PostAddUser(User _user)
        {
            _user.Password = StringCipher.Encrypt(_user.Password);
            _user.IsActive = 3;
            _user.CreatedAt = GeneralPurpose.DateTimeNow();

            if (!await new UserBL().AddUser(_user, de))
            {
                return RedirectToAction("AddUser", "Admin", new { msg = "Somethings' Wrong", color = "red" });
            }
            MailSender.ActivationEmail(_user.Email);
            return RedirectToAction("AddUser", "Admin", new { msg = "Record Inserted Successfully", color = "green", cat=_user.Role });
        }

       // [ValidationFilter(Roles = new int[] { 1 })]
        public IActionResult ViewUser(int category = -1, string msg = "", string color = "black")
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
                    return RedirectToAction("Index", "Home");
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
        
                user.Contact = _user.Contact;
            if (_user.Password != null)
            {
                user.Password = StringCipher.Encrypt(_user.Password);
            }
            
                

            if (_user.Country != null)
            {
                user.Country = _user.Country;
            }
            if (_user.Gender != null)
            {
                user.Gender = _user.Gender;
            }
           
            if (_user.ZipCode != null)
            {
                user.ZipCode = _user.ZipCode;
            }
            if (_user.Username != null)
            {
                user.Username = _user.Username;
            }
            if (_user.FirstName != null)
            {
                user.FirstName = _user.FirstName;
            }
            if (_user.LastName != null)
            {
                user.LastName = _user.LastName;
            }

            user.Description = _user.Description;

            user.Website = _user.Website;

            user.Organization = _user.Organization;

            user.City = _user.City;

            user.ImagePath = user.ImagePath;

            bool chkUser = await new UserBL().UpdateUser(user, de);
            if (chkUser)
            {
                if (user.Role != 1)
                {
                    return RedirectToAction("ViewUser", "Admin", new { category = user.Role, msg = "User updated successfully", color = "green", way = way });
                }
                else
                {
                    return RedirectToAction("UpdateProfile","Auth", new { msg= "Profile updated successfully" , color="green" });
                }
            }
            return RedirectToAction("ViewUser", "Admin", new { category=user.Role, msg = "Somethings' wrong", color = "red", way = way });
        }

      
        public async Task<IActionResult> DeleteUser(int id)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                if (CurrentUserRecord.Role == 1)
                {
                    var r = new UserBL().GetActiveUserById(id, de).Role;
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

        public async Task<IActionResult> ApprovedUser(int id)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                if (CurrentUserRecord.Role == 1)
                {
                    var getUser = new UserBL().GetActiveUserById(id, de);
                    if (getUser != null)
                    {
                        getUser.IsActive = 3;
                        MailSender.ActivationEmail(getUser.Email); // new changes
                        if (await new UserBL().UpdateUser(getUser, de))
                        {
                            MailSender.SendApprovalEmail(getUser.Email);
                            return RedirectToAction("ViewUser", new { category = getUser.Role, msg = "Record is approved successfully!", color = "green" });
                        }
                    }
                    return RedirectToAction("ViewUser", new { category = getUser.Role, msg = "Something is wrong", color = "red" });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> enableUser(int id)
        {
            //var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            //var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            try
            {
                
                    var getUser = new UserBL().GetActiveUserById(id, de);
                    if (getUser != null)
                    {
                        getUser.IsActive = 1;
                        //MailSender.ActivationEmail(getUser.Email); // new changes
                        if (await new UserBL().UpdateUser(getUser, de))
                        {
                            //MailSender.SendApprovalEmail(getUser.Email);
                            return RedirectToAction("ViewUserUnderReview", new { category = getUser.Role, msg = "User enabled successfully!", color = "green" });
                        }
                    }
                    return RedirectToAction("ViewUserUnderReview", new { category = getUser.Role, msg = "No record found now please refresh your page!", color = "red" });
                
                
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        #region Feedbacks
        public IActionResult ViewFeedbacks()
        {
            return View();
        }
        #endregion

        //public string TakeSS(string Email="", string Pass="")
        //{

        //    ChromeOptions options = new ChromeOptions();
        //    //options.AddArgument("headless");//Comment if we want to see the window. 
        //    var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //    var driver = new ChromeDriver((path), options) ;
        //    driver.Navigate().GoToUrl(ProjectVariables.baseUrl+"/Auth/PostLogin?Email="+ Email+ "&?Password="+Pass);
        //    var screenshot = (driver as ITakesScreenshot).GetScreenshot();
        //    string FileName = "Invoice" + "_" + DateTime.UtcNow.Ticks.ToString() + "" + ".jpeg";
        //    screenshot.SaveAsFile(FileName);
        //    driver.Close();
        //    driver.Quit();
        //    return FileName;
        //}

    }
}
