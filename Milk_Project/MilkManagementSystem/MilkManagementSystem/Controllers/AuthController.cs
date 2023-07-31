using MilkManagementSystem.BL;
using MilkManagementSystem.HelpingClasses;
using MilkManagementSystem.Models;
using MilkManagementSystem.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

using static MilkManagementSystem.HelpingClasses.ProjectVariables;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MilkManagementSystem.Controllers
{
    [ExceptionFilter]
    public class AuthController : Controller
    {
        private readonly SqlConnection de;
        private readonly GeneralPurpose gp;
        private readonly IConfiguration confg;
        private readonly IHttpContextAccessor haccess;
        //private readonly IHubContext<ChatHub> HubContext;


        public AuthController(IConfiguration confg, IHttpContextAccessor haccess)
        {
            this.de = new SqlConnection(confg.GetConnectionString("Default"));
            this.gp = new GeneralPurpose(de, haccess);
            this.confg = confg;
            this.haccess = haccess;
            var request = haccess.HttpContext.Request;

            baseUrl = $"{request.Scheme}://{request.Host}";
        }

        #region Login

        public IActionResult Login(string msg = "", string color = "black")
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;


            var GetUser = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);

            User loggedinUser = GetUser;

            if (loggedinUser != null)
            {
                if (loggedinUser.Role == 1)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostLogin(string Email = "", string Password = "")
        {
            User user = new UserBL().GetAllUsersList(de).Where(x => x.IsActive != 0 && (x.Email.Trim().ToLower() == Email.Trim().ToLower() && StringCipher.Decrypt(x.Password).Equals(Password))).FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction("Login", new { msg = "Incorrect Email/Password!", color = "red" });
            }
            else if (user.IsActive == 2)
            {
                return RedirectToAction("Login", new { msg = "Please verify your email to Login!", color = "red" });
            }

            //Generating a list of claims to store them into Cookies
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim("Role", user.Role.ToString()),
            };


            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
            var principal = new ClaimsPrincipal(identity);
            //SignInAsync is a Extension method for Sign in a principal for the specified scheme.
            //in order to work the following code we need to register it into Startup.cs class
            //so paste the following code ini that
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x => x.LoginPath = "/Auth/Login");

            //also add the following functions in startup.cs class to store claims in cookies (else wise system will not store cookies)
            //app.UseAuthentication();
            //app.UseAuthorization();

         
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = true// for remember me check box while logging in

            });



         

          

            return RedirectToAction("Index", "Admin");

        }

        #endregion

        #region Forgot Password

        public IActionResult ForgotPassword(string msg = "", string color = "black")
        {
            ViewBag.Color = color;
            ViewBag.Message = msg;

            return View();
        }

        public IActionResult PostForgotPassword(string Email = "")
        {
            bool checkEmail = gp.ValidateEmail(Email);

            if (checkEmail == false)
            {
                int id = new UserBL().GetAllUsersList(de).Where(x => x.Email.ToLower() == Email.ToLower()).Select(x => x.Id).FirstOrDefault();

                string BaseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}"+"/";

                bool checkMail = MailSender.SendForgotPasswordEmail(Email, id, BaseUrl);

                if (checkMail == true)
                {
                    return RedirectToAction("ForgotPassword", "Auth", new { msg = "Please check your mail's inbox/spam", color = "green" });
                }
                else
                {
                    return RedirectToAction("ForgotPassword", "Auth", new { msg = "Mail sending fail!", color = "red" });
                }
            }
            else
            {
                return RedirectToAction("ForgotPassword", "Auth", new { msg = "Email does not belong to our record!!", color = "red" });
            }
        }


        public IActionResult ResetPassword(string encId = "", string t = "", string msg = "", string color = "black")
        {
            DateTime PassDate = new DateTime(Convert.ToInt64(t)).Date;
            DateTime CurrentDate = GeneralPurpose.DateTimeNow().Date;

            if (CurrentDate != PassDate)
            {
                return RedirectToAction("Login", "Auth", new { msg = "Link expired, Please try again!", color = "red" });
            }


            ViewBag.Time = t;
            ViewBag.EncId = encId;
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> PostResetPassword(string encId = "", string t = "", string NewPassword = "", string ConfirmPassword = "")
        {
            if (NewPassword != ConfirmPassword)
            {
                return RedirectToAction("ResetPassword", "Auth", new { encId = encId, t = t, msg = "Password and confirm password did not match", color = "red" });
            }
            var enc = StringCipher.DecryptId(encId);
            var user = new UserBL().GetActiveUserById(Convert.ToInt32(StringCipher.DecryptId(encId)), de);
            user.Password = StringCipher.Encrypt(NewPassword);

            bool check = await new UserBL().UpdateUser(user, de);

            if (check == true)
            {
                return RedirectToAction("Login", "Auth", new { msg = "Password reset successful, Try login", color = "green" });
            }
            else
            {
                return RedirectToAction("ResetPassword", "Auth", new { encId = encId, t = t, msg = "Somethings' wrong!", color = "red" });
            }
        }

        #endregion

        #region Manage Profile

        public IActionResult UpdateProfile(string msg = "", string color = "black")
        {
            try
            {
                var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;


                var GetUser = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
                User u = GetUser;


                ViewBag.User = u;
                ViewBag.Message = msg;
                ViewBag.Color = color;

                return View();

            }
            catch
            {
                return RedirectToAction("NotFoundPage", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostUpdateProfile(string EncId, User _user)
        {
            int UserId = Convert.ToInt32(StringCipher.Decrypt(EncId));

            User u = new UserBL().GetActiveUserById(UserId, de);

            bool checkEmail = true;

            if (_user.Email != u.Email)
            {
                checkEmail = gp.ValidateEmail(_user.Email, UserId);
            }


            if (checkEmail == false)
            {
                if (u.Role == 1)
                {
                    return RedirectToAction("UpdateProfile", "Auth", new { msg = "Email used by someone else, Please try another", color = "red" });
                }
              
            }

            u.Contact_No = _user.Contact_No;
            //u.FirstName = u.FirstName;
            //u.LastName = u.LastName;

            u.Email = _user.Email.Trim();

            //u.Description = _user.Description;


            u.FullName = _user.FullName;


            u.UpdatedAt = _user.UpdatedAt;





            bool chk = await new UserBL().UpdateUser(u, de);

            if (chk)
            {

                return RedirectToAction("UpdateProfile", "Auth", new { msg = "Profile updated successfully!", color = "green" });
            }
            else
            {
              
                return RedirectToAction("UpdateProfile", "Auth", new { msg = "Somthing is Wrong!", color = "red" });
            }
        }

        public IActionResult UpdatePassword(string msg = "", string color = "black")
        {
            try
            {
                var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;


                var GetUser = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
                User u = GetUser;

                ViewBag.Message = msg;
                ViewBag.Color = color;

                return View();

            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }




        public async Task<IActionResult> PostUpdatePassword(string oldPassword = "", string newPassword = "", string confirmPassword = "")
        {

            if (newPassword != confirmPassword)
            {
                return RedirectToAction("UpdatePassword", "Auth", new { msg = "New password and Confirm password did not match!", color = "red" });
            }
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;


            var GetUser = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            User u = GetUser;

            if (StringCipher.Decrypt(u.Password) != oldPassword)
            {
                return RedirectToAction("UpdatePassword", "Auth", new { msg = "Old password did not match!", color = "red" });
            }

            u.Password = StringCipher.Encrypt(newPassword);

            bool chk = await new UserBL().UpdateUser(u, de);

            if (chk)
            {
                return RedirectToAction("UpdatePassword", "Auth", new { msg = "Password updated successfully!", color = "green" });
            }
            else
            {
                return RedirectToAction("UpdatePassword", "Auth", new { msg = "Something's wrong!", color = "red" });
            }
        }

        #endregion

        #region Logout



        public async Task<IActionResult> LogOut()
        {

            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            var GetUser = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            return RedirectToAction("Login", "Auth");
        }

        #endregion

    }
}
