using LoginFinal.BL;
using LoginFinal.HelpingClasses;
using LoginFinal.Models;
using LoginFinal.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using static LoginFinal.HelpingClasses.ProjectVariables;
using LoginFinal.DataHub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LoginFinal.Controllers
{
    [ExceptionFilter]
    public class AuthController : Controller
    {   
        private readonly SqlConnection de;
        private readonly GeneralPurpose gp;
        private readonly IConfiguration confg;
        //private readonly AppDbContext db;
        private readonly IHttpContextAccessor haccess;
        private readonly IHubContext<ChatHub> HubContext;


        public AuthController(SqlConnection de, IConfiguration confg, IHttpContextAccessor haccess, IHubContext<ChatHub> HubContext)
        {
            //this.de = new SqlConnection(confg.GetConnectionString("Default"));
            this.de = de;
            this.gp = new GeneralPurpose(de, haccess);
            this.confg = confg;
            this.haccess = haccess;
            var request = haccess.HttpContext.Request;
            this.HubContext = HubContext;
            baseUrl = $"{request.Scheme}://{request.Host}";
           
        }

        #region Login
        public IActionResult Login(string msg = "", string color = "black")
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            var GetUser = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);

            User loggedinUser = GetUser;

            if(loggedinUser != null)
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
            User user = new UserBL().GetAllUsersList(de).Where(x => x.IsActive != 0 && (x.Email.Trim().ToLower() == Email.Trim().ToLower() || x.Username.Trim().ToLower() == Email.Trim().ToLower()) && StringCipher.Decrypt(x.Password).Equals(Password)).FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction("Login", new { msg = "Incorrect Email/Password!", color = "red" });
            }


            MailSender.AccountOnHold(user.Email);


            //Start massam 07-01-23

            if (user.IsActive == 4)
            {
                return RedirectToAction("Login", new { msg = "Your account is on hold. Kindly contact to support for further assistance", color = "red" });
            }
            //End massam 07-01-23


            //Generating a list of cliams to store them into Cookiest
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim("UserName", user.Username),
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

            var statusupd = new AjaxController(de, confg, haccess, HubContext).StatusUpdate(user.Id, 1);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = true// for remember me check box while logging in
    
            });

            

            if(user.Role == 1)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                user.Status = 1;
                var update = new UserBL().UpdateUser(user, de);
               
                return RedirectToAction("Account", "Seller");
               
            }
        }


        public async Task<bool> PostLogins(string Email = "", string Password = "")
        {
            var pa = StringCipher.Decrypt(Password);
            User user = new UserBL().GetAllUsersList(de).Where(x => x.IsActive != 0 && (x.Email.Trim().ToLower() == Email.Trim().ToLower() || x.Username.Trim().ToLower() == Email.Trim().ToLower()) && StringCipher.Decrypt(x.Password).Equals(pa)).FirstOrDefault();

            if (user == null)
            {
                //return RedirectToAction("Login", new { msg = "Incorrect Email/Password!", color = "red" });
                return false;
            }

            //Generating a list of cliams to store them into Cookiest
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim("UserName", user.Username),
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
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = true// for remember me check box while logging in
            });
            //also add the following functions in startup.cs class to store claims in cookies (else wise system will not store cookies)
            //app.UseAuthentication();
            //app.UseAuthorization();


            if (user.Role == 1)
            {
                //return RedirectToAction("Index", "Admin");
                return true;
            }
            else
            {
                user.Status = 1;
                var update = new UserBL().UpdateUser(user, de);
                if (user.Role == 3)
                {
                   
                    //return RedirectToAction("Account", "Home");
                    return true;
                }

                //return RedirectToAction("Index", "Home");
                return true;
            }
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

        #region Signup

        [HttpPost]
        public async Task<IActionResult> PostRegister(User _user, string _confirmPassword = "", string val="")
        {

            if (_user.Password != _confirmPassword)
            {
                return RedirectToAction("Register", "Home", new { msg = "Password and confirm password didn't match", color = "red" });
            }

            bool checkEmail = gp.ValidateEmail(_user.Email);
            if (checkEmail == false)
            {
                return RedirectToAction("Register", "Home", new { msg = "Email already exists. Try sign in!", color = "red" });
            }
            // 1 For Buyer
            // 2 For Seller
            if(val=="Customer")
            {
                _user.Role = 4;
                _user.IsActive = 3; // masam change
            }
            if (val == "I.T. Butler" || val == "Butler")
            {
                _user.Role = 3;
                _user.IsActive = 2; // masam change
            }
            User u = new User()
            {
                Username = _user.Username.Trim(),
                FirstName = _user.FirstName,
                LastName = _user.LastName,
                Country = _user.Country,
                Contact = _user.Contact,
                Email = _user.Email.Trim(),
                Password = StringCipher.Encrypt(_user.Password),
                Role = _user.Role,
                City= _user.City,
                ZipCode= _user.ZipCode,
                //IsActive = 3,   older
                IsActive = _user.IsActive,

                //IsActive = 2,  old

                Refferal_Code = _user.Username.Trim()+_user.Id,
                CreatedAt = GeneralPurpose.DateTimeNow()
            };
            if (_user.Role == 4)
            {
                u.IsActive = 3;
            }
            if (_user.Role == 3)
            {
               u.StartingFrom="25";
            }

            //massam(26-12-22)
            var referredUserReccord = new UserBL().GetUserReccordByReferralCode(_user.Reffered_By, de);
            if (referredUserReccord != null && (referredUserReccord.Role == 3 && _user.Role == 3))
            {
                var butlersReferralCount = new UserBL().GetButlersReferralReccordByReferralCode(referredUserReccord.Refferal_Code, de).Count();
                if (butlersReferralCount > 10)
                {
                    return RedirectToAction("Register", "Home", new { msg = "Butler's registeration limit reached against this referral. Try to register with another referral or without referral.", color = "red" });
                }
            }
            //End massam(26-12-22)


            bool chkUser = await new UserBL().AddUser(u, de);
            
            if (_user.Reffered_By != null)
            {
                var getReferedId = new UserBL().GetAllUsersList(de).Where(x => x.Email == u.Email).FirstOrDefault().Id;

                //getReferedId.Reffered_By = _user.Reffered_By;
                var refr = new Refferals
                {
                    RefferedId = getReferedId,
                    RefferalCode = _user.Reffered_By.Trim(),
                    RefferalType = 2,
                    IsActive = 1,
                    CreatedAt = DateTime.UtcNow,
                };
                //massam (26-12-22)
                if (_user.Role == 3)
                {
                    refr.ReferralEndTime = DateTime.UtcNow.AddMonths(6);
                    refr.ReferredUserType = 1;
                }
                else
                {
                    refr.ReferredUserType = 2;
                }
                //End massam (26-12-22)

                var addRef = new UserBL().AddRefferal(refr, de);
                //bool chkkUser = await new UserBL().AddUser(getReferedId, de);//update user
            }

            

            if (chkUser)
            {
                if (u.Role == 4) // by masam
                {
                    MailSender.ActivationEmail(u.Email);
                }

                //MailSender.ActivationEmail(u.Email);
                var getUser = new UserBL().GetAllUsersList(de).Where(x => x.Email == u.Email).LastOrDefault();
                bool check = await PostLogins(getUser.Email, getUser.Password);
               
                    if (check == true)
                    {
                    if(u.Role==4)
                    {
                        return RedirectToAction("Account", "Seller", new { msg = "An activation email send to your email please check your inbox to activate your account.", color = "green" });

                    }
                    // return RedirectToAction("Account", "Seller", new { msg = "Account Activation Email has been sent to your Email", color = "green" });
                    return RedirectToAction("Account", "Seller", new { msg = "Your Profile is under Review. We will send you an email once it is approved.", color = "green" });

                }
                else
                    {
                        return RedirectToAction("Register", "Home", new { msg = "Somethings' wrong", color = "red" });

                    } 
            }
            else
            {
                return RedirectToAction("Register", "Home", new { msg = "Somethings' wrong", color = "red" });
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> AccountAcctivate(string e = "", int id = -1, int ac=-1)
        {
            if (e != "" || id != -1)
            {
                var getEmail = StringCipher.Base64Decode(e);
                var getUser = new User();
                if (getEmail != "")
                {
                    getUser = new UserBL().GetAllUsersList(de).Where(x => x.IsActive == 3 && x.Email == getEmail).FirstOrDefault();

                }
                else
                {
                    getUser = new UserBL().GetAllUsersList(de).Where(x => x.IsActive == 3 && x.Id == id).FirstOrDefault();
                }
                if (getUser != null)
                {
                    //if (getUser.Role == 4)
                    //{
                        getUser.IsActive = 1;
                    //}
                    //else if (getUser.Role == 3)
                    //{
                    //    // getUser.IsActive = 2;
                       // getUser.IsActive = 1;
                    //}
                    bool check = await new UserBL().UpdateUser(getUser, de);
                    if (check == true)
                    {
                        var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

                        var GetUser = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
                        User loggedinUser = GetUser;
                        if (loggedinUser != null)
                        {
                            if (loggedinUser.Role != 1)
                            {
                                if (getUser.Role == 4)
                                {
                                    //return RedirectToAction("AccountActivated", new { msg = "Your Account is Activated", color = "green" });
                                    return RedirectToAction("CustomerRequestHelp", "Home", new { msg = "Your Account is Activated", color = "green" });

                                }
                                else if(getUser.Role == 3)
                                {
                                    return RedirectToAction("AccountActivated");
                                    //return RedirectToAction("CustomerRequestHelp", "Home", new { msg = "Your Account is Activated. Now Wait For Admin Approval", color = "green" });

                                }
                                else 
                                {
                                    //return RedirectToAction("AccountActivated", new { msg = "Your Account is Activated. Now Wait For Admin Approval", color = "green" });
                                    return RedirectToAction("CustomerRequestHelp", "Home", new { msg = "Your Account is Activated. Now Wait For Admin Approval", color = "green" });

                                }
                            }
                            else if(ac!=-1)
                            {
                                return RedirectToAction("ViewUser","Admin", new { category= getUser.Role, msg = "Account Activated Successfully!", color = "green" });
                            }
                            else
                            {
                                //return RedirectToAction("AccountActivated", new { msg = "Account Activated Successfully!", color = "green" });
                                return RedirectToAction("CustomerRequestHelp", "Home", new { msg = "Account Activated Successfully!", color = "green" });

                            }
                        }
                        else
                        {
                            //return RedirectToAction("AccountActivated", new { msg = "Your Account is Activated", color = "green" });
                            return RedirectToAction("CustomerRequestHelp", "Home", new { msg = "Your Account is Activated", color = "green" });

                        }
                    }
                }
            }
            return RedirectToAction("Login", new { msg = "Something went wrong", color = "red" });
        }


        [AllowAnonymous]
        public IActionResult AccountActivated(/*string msg = "", string color = ""*/)
        {
           

            return View();
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
                if (GetUser.Role == 3 || GetUser.Role == 4 || GetUser.Role == 1)
                {

                    ViewBag.User = u;
                    ViewBag.Message = msg;
                    ViewBag.Color = color;

                    return View();
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

        public async Task<IActionResult> PostUpdateProfile(string EncId, User _user, IFormFile ImagePath = null, string[] SkillName = null, string[] TagName = null,IFormCollection formCollection=null, int pcount=0,List<string> userlangs=null, List<string> butlerprefertime = null)
        {
            int uid = StringCipher.DecryptId(EncId);
            //langs = string.Join(",", arrayoflangs).TrimStart(',');
            string[] arrayoflangs = userlangs.ToArray();
            //List<bool> Addlanguage= new List<bool>();
            if (userlangs != null)
            {
                //var Addlanguage = AdduserLanguage(userlangs, uid.ToString());
                var Addlanguage = new UserBL().AddUpdateButlerlanguage(userlangs, uid,de);

            }

            
            int UserId = Convert.ToInt32(StringCipher.DecryptId(EncId));
            
            var imgUpload = new UserBL().UploadImage(ImagePath);
            

         
            User u = new UserBL().GetActiveUserById(UserId, de);
            
            bool checkEmail = true;

            if (_user.Email != u.Email)
            {
                checkEmail = gp.ValidateEmail(_user.Email, UserId);
            }

            //Skills skills = new UserBL().GetSkillsById(UserId, de);
            var skills = new UserBL().GetSkillsById(UserId, de); //masam change
            
            var UserTags = new UserBL().GetTagsById(UserId, de);
            
            if (checkEmail == false)
            {
                if (u.Role == 1)
                {
                    return RedirectToAction("UpdateProfile", "Auth", new { msg = "Email used by someone else, Please try another", color = "red" });
                }
                else
                {
                    return RedirectToAction("Account", "Seller", new { msg = "Email used by someone else, Please try another", color = "red" });
                }
            }

            //Massam Email Sending
            //Massam(15-12-22)
            //Massam Email Sending
            //SkillName = SkillName[1].Split(",");
            if (u.Role == 3)
            {
                if (butlerprefertime != null)
                {
                    //var AddOrUpdatePrefertime = Adduserprefertime(butlerprefertime, uid.ToString());
                    var AddOrUpdatePrefertime = new UserBL().AddUpdateButlerPreferTime(butlerprefertime, uid, de);

                }

                //SkillName = SkillName[1].Split(",");
                if (_user.Availability == 1)
                {
                    //await HubContext.Clients.All.SendAsync("customerrequestnotification", UserId.ToString());
                    if (SkillName.Length > 0)
                    {
                        var Requestsbyskill = new UserBL().GetRequestsBySkills(SkillName, de);
                        //int customeremailcount = 0;
                        foreach (var request in Requestsbyskill)
                        {
                            if (!request.EmailToButlers.Contains(UserId.ToString()))
                            {
                                var mailToButler = MailSender.CustomerRequestMailToButlers(u.Email);
                                if (mailToButler)
                                {
                                    request.EmailToButlers += string.Join(",", u.Id).TrimStart(',');
                                    if (request.EmailToCustomer == 0)
                                    {
                                        var mailToCustomer = MailSender.MatchingButlersMailToCustomer(request.userid.ToString());
                                        if (mailToCustomer)
                                        {
                                            request.EmailToCustomer = request.userid;
                                        }
                                        //customeremailcount++;
                                    }
                                }
                                new UserBL().UpdateRequestForEmailSend(request, de);
                            }
                        }
                    }
                }
                //massam (14-12-22)
                int skillsLoopLength = 0;
                if (SkillName.Length > skills.Count)
                {
                    skillsLoopLength = SkillName.Length;
                }
                else
                {
                    skillsLoopLength = skills.Count;
                }
                if (SkillName.Length != 0)
                {
                    //Add Skills Seprated by Comma
                    //var sk = string.Join(",",SkillName).TrimStart(',');
                    //skills.SkillName = sk.ToLower();
                    //skills.UpdatedAt = DateTime.UtcNow;
                    //var addskills = new UserBL().AddSkills(skills, de);
                    var new_skills = new Skills();
                    if (skills.Count != 0)
                    {
                        for (int i = 0; i < skillsLoopLength; i++)
                        {
                            //if (skills.Where(a => a.SkillName == SkillName[i]).FirstOrDefault() == null)
                            //{
                            //    new_skills.SkillName = SkillName[i].ToLower();
                            //    new_skills.CreatedAt = DateTime.UtcNow;
                            //    new_skills.IsActive = 1;
                            //    new_skills.UserId = UserId;
                            //    var addskills = new UserBL().AddSkills(new_skills, de);
                            //}
                            if (i < skills.Count && SkillName.Length > i)
                            {
                                skills[i].SkillName = SkillName[i].ToLower();
                                skills[i].CreatedAt = DateTime.UtcNow;
                                var addskills = new UserBL().AddSkills(skills[i], de);
                            }
                            else if (SkillName.Length > skills.Count)
                            {
                                new_skills.SkillName = SkillName[i].ToLower();
                                new_skills.CreatedAt = DateTime.UtcNow;
                                new_skills.IsActive = 1;
                                new_skills.UserId = UserId;
                                var addskills = new UserBL().AddSkills(new_skills, de);
                            }
                            else
                            {
                                skills[i].IsActive = 0;
                                skills[i].UpdatedAt = DateTime.UtcNow;
                                var addskills = new UserBL().AddSkills(skills[i], de);
                            }
                        }
                    }
                    else
                    {
                        //var new_skills = new Skills
                        //{
                        //    SkillName = sk.ToLower(),
                        //    CreatedAt = DateTime.UtcNow,
                        //    IsActive = 1,
                        //    UserId = UserId,
                        //};
                        //var addskills = new UserBL().AddSkills(new_skills, de);
                        for (int i = 0; i < SkillName.Length; i++)
                        {
                            new_skills.SkillName = SkillName[i].ToLower();
                            new_skills.CreatedAt = DateTime.UtcNow;
                            new_skills.IsActive = 1;
                            new_skills.UserId = UserId;
                            var addskills = new UserBL().AddSkills(new_skills, de);
                        }
                    }
                }

                if (TagName[1] != null && TagName.Length > 1)
                {
                    //Add TagName Seprated by Comma
                    //var sk = string.Join(",", TagName).TrimStart(',');
                    var new_tags = new Tags();
                    TagName = TagName[1].Split(",");
                    int tagsLoopLength = 0;
                    if (TagName.Length > UserTags.Count)
                    {
                        tagsLoopLength = TagName.Length;
                    }
                    else
                    {
                        tagsLoopLength = UserTags.Count;
                    }
                    if (UserTags.Count != 0)
                    {
                        //UserTags.TagName = sk.ToLower();
                        //UserTags.UpdatedAt = DateTime.UtcNow;
                        //var addTags = new UserBL().AddTags(UserTags, de);
                        for (int i = 0; i < tagsLoopLength; i++)
                        {
                            //if (skills.Where(a => a.SkillName == SkillName[i]).FirstOrDefault() == null)
                            //{
                            //    new_tags.TagName = TagName[i].ToLower();
                            //    new_tags.CreatedAt = DateTime.UtcNow;
                            //    new_tags.IsActive = 1;
                            //    new_tags.UserId = UserId;
                            //    var addTags = new UserBL().AddTags(new_tags, de);
                            //}
                            if (i < UserTags.Count && TagName.Length > i)
                            {
                                UserTags[i].TagName = TagName[i];
                                UserTags[i].CreatedAt = DateTime.UtcNow;
                                var addTags = new UserBL().AddTags(UserTags[i], de);
                            }
                            else if (TagName.Length > UserTags.Count)
                            {
                                new_tags.TagName = TagName[i].ToLower();
                                new_tags.CreatedAt = DateTime.UtcNow;
                                new_tags.IsActive = 1;
                                new_tags.UserId = UserId;
                                var addTags = new UserBL().AddTags(new_tags, de);
                            }
                            else
                            {
                                UserTags[i].IsActive = 0;
                                UserTags[i].UpdatedAt = DateTime.UtcNow;
                                var addTags = new UserBL().AddTags(UserTags[i], de);
                            }
                        }
                    }
                    else
                    {
                        //var new_tags = new Tags
                        //{
                        //    TagName = sk.ToLower(),
                        //    CreatedAt = DateTime.UtcNow,
                        //    IsActive = 1,
                        //    UserId = UserId,
                        //};
                        //var addTags = new UserBL().AddTags(new_tags, de);
                        for (int i = 0; i < TagName.Length; i++)
                        {
                            new_tags.TagName = TagName[i].ToLower();
                            new_tags.CreatedAt = DateTime.UtcNow;
                            new_tags.IsActive = 1;
                            new_tags.UserId = UserId;
                            var addTags = new UserBL().AddTags(new_tags, de);
                        }
                    }
                }
            }
            //End Massam(15-12-22)

            //Add Education
            if (pcount != 0)
                {
                for (int i = 1; i < pcount; i++)
                {
                    var degree = formCollection["degname" + i];
                    var institute = formCollection["insname" + i];
                    var startdate = formCollection["sd" + i];
                    var enddate = formCollection["ed" + i];
                    if (degree.Count() != 0 && institute.Count() != 0 && startdate.Count() != 0 && enddate.Count() != 0)
                    {
                        Education edu = new Education
                        {
                            DegreeName = degree,
                            InstituteName = institute,
                            UserId = UserId,
                            IsActive = 1,
                            CreatedAt = DateTime.UtcNow,
                            StartDate = Convert.ToDateTime(startdate).Date,
                            EndDate = Convert.ToDateTime(enddate).Date,

                        };
                        var saveedu = new UserBL().AddEducation(edu, de);

                    }
                }
            }

         
           
                
            //u.FirstName = u.FirstName;
            //u.LastName = u.LastName;
            u.Username = _user.Username.Trim();
            u.Email = _user.Email.Trim();
            u.Contact = _user.Contact;
            u.Description = _user.Description;
            u.Company = _user.Company;
            u.Country = _user.Country;
            u.DOB = _user.DOB;
            u.Experience_From = _user.Experience_From;
            u.Experience_To = _user.Experience_To;
            u.FacebookLink = _user.FacebookLink;
            u.Gender = _user.Gender;
            u.DribbleLink = _user.DribbleLink;
            u.GoogleLink = _user.GoogleLink;
            u.GitHubLink = _user.GitHubLink;
            u.StackOverFlowLink = _user.StackOverFlowLink;
            u.VimeoLink = _user.VimeoLink;
            // u.Language = _user.Language;

            var ulanguages = string.Join(",", arrayoflangs).TrimStart(',');
            u.Language = ulanguages;

            u.City= _user.City;
            u.Organization = _user.Organization;
            u.Position = _user.Position;
            u.Status = _user.Status;
            u.TwitterLink = _user.TwitterLink;
            u.Website = _user.Website;
            u.ZipCode = _user.ZipCode;
            u.Contact = _user.Contact;
            u.UpdatedAt = _user.UpdatedAt;
            u.StartingFrom = _user.StartingFrom;
            u.StripeId = _user.StripeId;
            
            if (_user.Availability != -1)
            {
                u.Availability = _user.Availability;
            }

            if (imgUpload.Result != null)
            {
                u.ImagePath = imgUpload.Result;
            }
            bool chk = await new UserBL().UpdateUser(u, de); 

            if (chk)
            {
                if(u.Role == 3 || u.Role==4)
                {
                    return RedirectToAction("Account", "Seller", new { msg = "Profile updated successfully!", color = "green" });
                }
              
                return RedirectToAction("UpdateProfile", "Auth", new { msg = "Profile updated successfully!", color = "green" });
            }
            else
            {
                if (u.Role == 3 || u.Role == 4)
                {
                    return RedirectToAction("Account", "Seller", new { msg = "Somthing is Wrong!", color = "red" });
                }
                return RedirectToAction("UpdateProfile", "Auth", new { msg = "Somthing is Wrong!", color = "red" });
            }
        }

        public List<bool> AdduserLanguage(List<string> languages, string userid = "")
        {
            var langs = languages.ToArray();
            List<bool> langstatuses = new List<bool>();
            for (int i = 0; i < langs.Length; i++)
            {
                UserLanguage lang = new UserLanguage()

                {

                    //Id = 1,
                    IsActive = 1,
                    CreatedAt = DateTime.Now.ToLongDateString(),
                    UserId = Convert.ToInt32(userid),
                    language = langs[i],



                };

                var langstatus = new UserBL().AddRequestlanguage(lang, de);
                langstatuses.Add(langstatus);
            }


            return langstatuses;

        }

        //public List<bool> Adduserprefertime(List<string> prefertime, string userid = "")
        //{
            
        //    List<bool> prefertimes = new List<bool>();
        //    for (int i = 0; i < prefertime.Count; i++)
        //    {
        //        butlerprefertime _pref = new butlerprefertime()

        //        {

        //            //Id = 1,
        //            IsActive = 1,
        //            CreatedAt = DateTime.Now.ToLongDateString(),
        //            UserId = Convert.ToInt32(userid),
        //            PreferTime = prefertime[i],

        //        };

        //        var prefstatus = new UserBL().AddUpdateButlerPreferTime(_pref,userid, de);
        //        prefertimes.Add(prefstatus);
        //    }


        //    return prefertimes;

        //}

        public IActionResult UpdatePassword(string msg = "", string color = "black")
        {
            try
            {
                var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;


                var GetUser = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
                User u = GetUser;
                if (GetUser.Role == 1)
                {
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


        
        public IActionResult UpdatePasswordUser(string msg = "", string color = "black")
        {
            try
            {
                var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;


                var GetUser = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
                User u = GetUser;
                if (GetUser.Role == 3 || GetUser.Role == 4)
                {
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

       
        public async Task<IActionResult> LogOut()
        {

            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            var GetUser = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            
            bool x = new AjaxController(de, confg, haccess,HubContext).StatusUpdate(GetUser.Id, 0);
            
            if (x==true)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion

    }
}
