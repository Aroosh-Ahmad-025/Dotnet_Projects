using CarSystem.BL;
using CarSystem.Helping_Classes;
using CarSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CarSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly DatabaseEntities de = new DatabaseEntities();
        private readonly GeneralPurpose gp = new GeneralPurpose();


        // GET: Auth

        #region Login

        public ActionResult Login(string msg = "", string color = "")
        {
            if (gp.ValidateLoggedinUser() != null)
            {
                if (gp.ValidateLoggedinUser().Role == 1)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            int userCount = new UserBL().GetActiveUsersList(de).Count();
            if (userCount == 0)
            {
                User u = new User()
                {
                    Name = "Uzair Aslam",
                    Contact = "0000-0000000",
                    Email = "uzair.aslam02@gmail.com",
                    Gender = 1,
                    Password = StringCipher.Encrypt("123"),
                    Role = 1,
                    IsActive = 1,
                    CreatedAt = gp.DateTimeNow()
                };
                bool checkUser = new UserBL().AddUser(u, de);
            }

            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        [HttpPost]
        public ActionResult PostLogin(string Email = "", string Password = "")
        {
            User user = new UserBL().GetActiveUsersList(de).Where(x => x.Email.Trim().ToLower() == Email.Trim().ToLower() && StringCipher.Decrypt(x.Password).Equals(Password)).FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction("Login", new { msg = "Incorrect Email/Password!", color = "red" });
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Sid,user.Id.ToString()),
                new Claim("UserName", user.Name),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim("Role", user.Role.ToString()),

            }, "ApplicationCookie");

            var claimsPrincipal = new ClaimsPrincipal(identity);
            Thread.CurrentPrincipal = claimsPrincipal;
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignIn(identity);

            if (user.Role == 1)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion Login


        #region Register
        public ActionResult Register(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        [HttpPost]
        public ActionResult PostRegister(User _user, string _confirmPassword = "")
        {

            if (_user.Password != _confirmPassword)
            {
                return RedirectToAction("Register", "Auth", new { msg = "Password and confirm password didn't match", color = "red" });
            }

            bool checkEmail = gp.ValidateEmail(_user.Email);
            if (checkEmail == false)
            {
                return RedirectToAction("Register", "Auth", new { msg = "Email already exists. Try Sign-In with your personal email!", color = "red" });
            }

            User u = new User()
            {
                Name = _user.Name.Trim(),
                Contact = _user.Contact,
                Email = _user.Email.Trim(),
                Password = StringCipher.Encrypt(_user.Password),
                //Gender = _user.Gender,                              //   2/18/2022
                Gender = 1,                                           //   2/18/2022
                Role = 2,    
                IsActive = 1,
                CreatedAt = gp.DateTimeNow()
            };

            bool chkUser = new UserBL().AddUser(u, de);

            if (chkUser)
            {
                return RedirectToAction("Login", "Auth", new { msg = "Account created successfully, Please login", color = "green" });
            }
            else
            {
                return RedirectToAction("Register", "Auth", new { msg = "Something's wrong.Try again!", color = "red" });
            }
        }

        #endregion Register


        #region Update Password

        public ActionResult UpdatePassword(string msg = "", string color = "black")
        {
            if (gp.ValidateLoggedinUser() == null)
            {
                return RedirectToAction("Login", "Auth", new { msg = "Session expired", color = "red" });
            }

            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        public ActionResult PostUpdatePassword(string oldPassword = "", string newPassword = "", string confirmPassword = "")
        {
            if (newPassword != confirmPassword)
            {
                return RedirectToAction("UpdatePassword", "Auth", new { msg = "New password and Confirm password did not match!", color = "red" });
            }
            DatabaseEntities de = new DatabaseEntities();

            User u = new UserBL().GetActiveUserById(gp.ValidateLoggedinUser().Id, de);
            if (!StringCipher.Decrypt(u.Password).Equals(oldPassword))
            {
                return RedirectToAction("UpdatePassword", "Auth", new { msg = "Old password did not match!", color = "red" });
            }

            u.Password = StringCipher.Encrypt(newPassword);

            bool chk = new UserBL().UpdateUser(u, de);

            if (chk == true)
            {
                return RedirectToAction("UpdatePassword", "Auth", new { msg = "Password updated successfully!", color = "green" });
            }
            else
            {
                return RedirectToAction("UpdatePassword", "Auth", new { msg = "Something's wrong!", color = "red" });
            }
        }

        #endregion Update Password


        #region Update Profile

        public ActionResult UpdateProfile(string msg = "", string color = "black")
        {
            if (gp.ValidateLoggedinUser() == null)
            {

                return RedirectToAction("Login", "Auth", new { msg = "Session Expired", color = "red" });

            }

            User u = new UserBL().GetActiveUserById(gp.ValidateLoggedinUser().Id, de);

            ViewBag.User = u;
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }
        [HttpPost]
        public ActionResult PostUpdateProfile(User _user)
        {
            bool checkMail = gp.ValidateEmail(_user.Email, _user.Id);
            if (checkMail == false)
            {
                return RedirectToAction("UpdateProfile", "Auth", new { msg = "Email used by someone else, Please try another", color = "red" });
            }

            User u = new UserBL().GetActiveUserById(_user.Id, de);
            u.Name = _user.Name.Trim();
            u.Contact = _user.Contact.Trim();
            u.Email = _user.Email.Trim();

            bool chk = new UserBL().UpdateUser(u, de);

            if (chk == true)
            {
                return RedirectToAction("UpdateProfile", "Auth", new { msg = "Profile updated successfully!", color = "green" });
            }
            else
            {
                return RedirectToAction("UpdateProfile", "Auth", new { msg = "Something's Wrong!", color = "red" });
            }

        }

        #endregion Update Profile


        #region Forgot Password

        public ActionResult ForgotPassword(string msg = "", string color = "black")
        {
            ViewBag.Color = color;
            ViewBag.Message = msg;
            return View();
        }
        public ActionResult PostForgotPassword(string Email = "")
        {
            bool checkEmail = gp.ValidateEmail(Email);
            if (checkEmail == false)
            {
                int id = new UserBL().GetActiveUsersList(de).Where(x => x.Email.ToLower() == Email.ToLower()).Select(x => x.Id).FirstOrDefault();
                string BaseUrl = string.Format("{0}://{1}{2}", HttpContext.Request.Url.Scheme, HttpContext.Request.Url.Authority, "/");
                bool checkMail = MailSender.SendForgotPasswordEmail(Email, StringCipher.Base64Encode(id.ToString()), BaseUrl);
                if (checkMail == true)
                {
                    return RedirectToAction("ForgotPassword", "Auth", new { msg = "Please check your mails' inbox/spam", color = "green" });
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

        public ActionResult ResetPassword(string encId = "", string t = "", string msg = "", string color = "black")
        {
            DateTime PassDate = new DateTime(Convert.ToInt64(t)).Date;
            DateTime CurrentDate = gp.DateTimeNow().Date;
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
        public ActionResult PostResetPassword(string encId = "", string t = "", string NewPassword = "", string ConfirmPassword = "")
        {
            if (NewPassword != ConfirmPassword)
            {
                return RedirectToAction("ResetPassword", "Auth", new { encId = encId, t = t, msg = "Password and confirm password did not match", color = "red" });
            }
            User user = new UserBL().GetActiveUserById(Convert.ToInt32(StringCipher.Base64Decode(encId)), de);
            user.Password = StringCipher.Encrypt(NewPassword);
            bool check = new UserBL().UpdateUser(user, de);
            if (check == true)
            {
                return RedirectToAction("Login", "Auth", new { msg = "Password reset successful, Try login", color = "green" });
            }
            else
            {
                return RedirectToAction("ResetPassword", "Auth", new { encId = encId, t = t, msg = "Something's wrong!", color = "red" });
            }
        }

        #endregion



        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Login");
        }
    }
}