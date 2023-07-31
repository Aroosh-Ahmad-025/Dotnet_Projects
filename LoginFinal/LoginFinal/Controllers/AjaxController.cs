using LoginFinal.BL;
using LoginFinal.Filters;
using LoginFinal.HelpingClasses;
using LoginFinal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Stripe.Infrastructure;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static LoginFinal.HelpingClasses.ProjectVariables;
using Stripe;
using Microsoft.AspNetCore.SignalR;
using LoginFinal.DataHub;
using Nancy.Json;
using IPinfo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LoginFinal.Controllers
{
    public class AjaxController : Controller
    {
        private readonly SqlConnection de;
        private readonly GeneralPurpose gp;
        private readonly IConfiguration confg;
        private readonly IHubContext<ChatHub> HubContext;
        //private readonly AppDbContext db;

        public AjaxController(SqlConnection de, IConfiguration confg, IHttpContextAccessor haccess, IHubContext<ChatHub> HubContext)
        {
            
            //this.de = new SqlConnection(confg.GetConnectionString("Default"));
            this.de = de;
            this.gp = new GeneralPurpose(de, haccess);
            this.confg = confg;
            var request = haccess.HttpContext.Request;
            this.HubContext = HubContext;
            baseUrl = $"{request.Scheme}://{request.Host}";
        }


        [HttpPost]
        
        public IActionResult AddInternalreview(int customerid=-1,string review=null,int orderid=0,int str=-1)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            InternalReview reviewtocustomer = new InternalReview()

            {
                customerid = customerid.ToString(),
                Sellerid= Userid,
                Orderid= orderid,
                Sellerreview = review,               
                IsActive = 1,
                CreatedAt = DateTime.Now,
                Stars=str



            };

            var customerinternaladd = new UserBL().Addinternalreview(reviewtocustomer, de);
            if (customerinternaladd)
            {
                //massam 07-01-23
                var user = new UserBL().GetUserById(customerid, de);
                var ilist = new UserBL().GetInternalReviewByUserId(user.Id, de).OrderByDescending(x => x.CreatedAt).Take(5).Where(x => x.Stars == 1).Count();
                if (ilist == 5)
                {
                    user.IsActive = 4;
                    new UserBL().UpdateUser(user, de);
                }
                var rlist = new UserBL().GetInternalReviewByUserId(user.Id, de).OrderByDescending(x => x.CreatedAt).Take(3).Where(x => x.Stars == 1).Count();
                if (rlist == 3)
                {
                    MailSender.AccountHoldWarning(user.Email);
                }
                //end massam 07-01-23

                var getcustomerreview = new UserBL().getInternalReviewbyId(customerid,Convert.ToInt32(Userid), de).Sellerreview;

                //return Json(customerinternaladd);
                return Json(getcustomerreview);
            }
            return Json("");
        }

        //Start massam 07-01-23
        [HttpPost]
        public IActionResult GetUserUndersReviewDataTableList(int category = -1, string Name = "", string email = "")
        {
            List<User> ulist = new UserBL().GetAllUsersList(de).Where(x => x.Role == category).ToList();
            List<User> userList = new List<User>();
            foreach (User item in ulist)
            {
                //var olist = new UserBL().GetOrderReviewByUserId(item.Id, de).OrderByDescending(x => x.CreatedAt).Take(5).Where(x => x.Stars == 1 ).Count();
                //if (olist == 5)
                //{
                //    userList.Add(item);
                //}

                if(item.IsActive==4)
                {
                    userList.Add(item);
                }

            }
            ulist = userList;
            if (!String.IsNullOrEmpty(Name))
            {
                ulist = ulist.Where(x => x.Username.ToLower().Contains(Name.ToLower())).ToList();
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
                ulist = ulist.Where(x => x.Email.ToLower().Contains(searchValue.ToLower()) ||
                                    x.Username != null && x.Username.ToLower().Contains(searchValue.ToLower())
                                    ).ToList();
            }
            int totalrowsafterfilterinig = ulist.Count();
            // pagination
            ulist = ulist.Skip(start).Take(length).ToList();
            List<UserDto> udto = new List<UserDto>();
            foreach (User u in ulist)
            {
                UserDto obj = new UserDto()
                {
                    Id = u.Id,
                    EncId = StringCipher.EncryptId(u.Id),
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Password = StringCipher.Decrypt(u.Password),
                    UserName = u.Username,
                    Email = u.Email,
                    Contact = u.Contact,
                    Encemail = StringCipher.Base64Encode(u.Email),
                    IsActive = (int)u.IsActive
                };
                udto.Add(obj);
            }
            return Json(new { data = udto, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        }
        //End massam 07-01-23

        #region experience

        public IActionResult Addexperience(string company_name,string position,string company_reference, string reference_contact, string from, string To,int userid)
        {
            Experience Butlerexperience = new Experience()

            {
                userid = userid,
                Organization = company_name,
                Designation = position,
                Organization_Reference = company_reference,
                ReferalContact = reference_contact,   
                FromDate = from,
                ToDate = To,
                IsActive = 1,
                CreatedAt = DateTime.Now,



            };

            var butlerexperienceadd = new UserBL().AddExperience(Butlerexperience, de);
            if(butlerexperienceadd)
            {
                return Json(butlerexperienceadd);
            }
            return Json("");
        }


        public IActionResult GetexperiencebyId(int expid)
        {
            

            var userexperience = new UserBL().Getexperiencebyid(expid,de);
            
                return Json(userexperience);
            
            
        }


        public IActionResult updateexperience(int expid, string company_name, string position, string company_reference, string reference_contact, string from, string To, int userid)
        {
            Experience Butlerexperience = new Experience()

            {
                Id = expid,
                userid = userid,
                Organization = company_name,
                Designation = position,
                Organization_Reference = company_reference,
                ReferalContact = reference_contact,
                FromDate = from,
                ToDate = To,
                IsActive = 1,
                updatedat = DateTime.Now,



            };

            var butlerexperienceupd = new UserBL().update_experience(Butlerexperience, de);
            if (butlerexperienceupd != null)
            {
                return Json(butlerexperienceupd);
            }
            return Json("");
        }


        public bool DeleteExperience(int id = -1)
        {
            try
            {
                if (id != -1)
                {
                    var check = new UserBL().DeleteExperiencebyid(id, de);
                    return check;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }


        #endregion


        #region User
        [HttpPost]
        public IActionResult GetUserDataTableList(int category = -1, string Name = "", string email = "")
        {
            List<User> ulist = new UserBL().GetAllUsersList(de).Where(x => x.IsActive != 0 && x.Role != 1).ToList();
            if (category == 4)
            {
                ulist = ulist.Where(x => x.Role == 4).ToList();
            }
            if (category == 3)
            {
                ulist = ulist.Where(x => x.Role == 3).ToList();
            }
            if (!String.IsNullOrEmpty(Name))
            {
                ulist = ulist.Where(x => x.Username.ToLower().Contains(Name.ToLower())).ToList();
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
                ulist = ulist.Where(x => x.Email.ToLower().Contains(searchValue.ToLower()) ||
                                    x.Username != null && x.Username.ToLower().Contains(searchValue.ToLower())
                                    ).ToList();
            }

            int totalrowsafterfilterinig = ulist.Count();


            // pagination
            ulist = ulist.Skip(start).Take(length).ToList();

            List<UserDto> udto = new List<UserDto>();

            foreach (User u in ulist)
            {
                UserDto obj = new UserDto()
                {
                    Id = u.Id,
                    EncId = StringCipher.EncryptId(u.Id),
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Password = StringCipher.Decrypt(u.Password),
                    UserName = u.Username,
                    Email = u.Email,
                    Contact = u.Contact,
                    Encemail = StringCipher.Base64Encode(u.Email),
                    IsActive = (int)u.IsActive
                };

                udto.Add(obj);
            }

            return Json(new { data = udto, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        }

        //public IActionResult GetUserRefferal(string Name = "", string email = "")
        //{
        //    var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        //    var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
        //    List<Refferals> ulist =new UserBL().GetAllReferrals(de).Where(x => x.RefferalCode == CurrentUserRecord.Refferal_Code).ToList();
        //    foreach(var x in ulist)
        //    {
        //        var getRefferedUser = new UserBL().GetActiveUserById(x.RefferedId, de);
        //        x.RefferedUser = getRefferedUser;
        //    }

        //    if (!String.IsNullOrEmpty(Name))
        //    {
        //        ulist = ulist.Where(x => x.RefferedUser.Username.ToLower().Contains(Name.ToLower())).ToList();
        //    }

        //    if (!String.IsNullOrEmpty(email))
        //    {
        //        ulist = ulist.Where(x => x.RefferedUser.Email.ToLower().Contains(email.ToLower())).ToList();
        //    }

        //    int start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
        //    int length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
        //    string searchValue = Request.Form["search[value]"].FirstOrDefault();
        //    string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
        //    string sortDirection = Request.Form["order[0][dir]"].FirstOrDefault();

        //    if (sortColumnName != "" && sortColumnName != null)
        //    {
        //        if (sortColumnName != "0")
        //        {
        //            if (sortDirection == "asc")
        //            {
        //                ulist = ulist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
        //            }
        //            else
        //            {
        //                ulist = ulist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
        //            }
        //        }
        //    }

        //    int totalrows = ulist.Count();

        //    //filter
        //    if (!string.IsNullOrEmpty(searchValue))
        //    {
        //        ulist = ulist.Where(x => x.RefferedUser.Email.ToLower().Contains(searchValue.ToLower()) ||
        //                            x.RefferedUser.Username != null && x.RefferedUser.Username.ToLower().Contains(searchValue.ToLower())
        //                            ).ToList();
        //    }

        //    int totalrowsafterfilterinig = ulist.Count();


        //    // pagination
        //    ulist = ulist.Skip(start).Take(length).ToList();

        //    List<UserDto> udto = new List<UserDto>();

        //    foreach (var u in ulist)
        //    {
        //        UserDto obj = new UserDto()
        //        {
        //            Id = u.RefferedUser.Id,
        //            FirstName = u.RefferedUser.FirstName,
        //            LastName = u.RefferedUser.LastName,
        //            UserName = u.RefferedUser.Username,
        //            Email = u.RefferedUser.Email,
        //            Role = u.RefferedUser.Role,
        //            Refferal_Code = u.RefferalCode,
        //            IsActive = u.RefferalType,
        //            City = Convert.ToDateTime(u.CreatedAt).ToString("dd-MMMM-yyyy"),
        //        };

        //        udto.Add(obj);
        //    }

        //    return Json(new { data = udto, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        //}


        public IActionResult GetResponses(string Name = "", string email = "")
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            List<Feedback> ulist = new UserBL().GetAllFeedBacks(de);

            if (!String.IsNullOrEmpty(Name))
            {
                ulist = ulist.Where(x => x.Name.ToLower().Contains(Name.ToLower())).ToList();
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
                ulist = ulist.Where(x => x.Email.ToLower().Contains(searchValue.ToLower()) ||
                                    x.Name != null && x.Name.ToLower().Contains(searchValue.ToLower())
                                    ).ToList();
            }

            int totalrowsafterfilterinig = ulist.Count();


            // pagination
            ulist = ulist.Skip(start).Take(length).ToList();

            List<Feedback> udto = new List<Feedback>();

            foreach (var u in ulist)
            {
                Feedback obj = new Feedback()
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Contact_No= u.Contact_No,
                    Message= u.Message,
                };

                udto.Add(obj);
            }

            return Json(new { data = udto, draw = Request.Form["draw"].FirstOrDefault(), recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig });
        }

        [HttpPost]
        public bool StatusUpdate(int id, int val)
        {
            var user = new UserBL().GetActiveUserById(id, de);
            user.Status = val;
            var update = new UserBL().UpdateUser(user, de);
            if (update.Result == true)
                return true;
            else
                return false;
        }

        [HttpPost]
        public bool AvailabilityUpdate(int id, int val)
        {
            var user = new UserBL().GetActiveUserById(id, de);
            user.Availability = val;
            var update = new UserBL().UpdateUser(user, de);
            if (update.Result == true)
                return true;
            else
                return false;
        }
        public async Task<bool>  updatenotification(int id,int isdelete=0)
        {
            //var user = new UserBL().GetActiveUserById(id, de);
            //user.Status = val;
            string LoggedinUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;


            var update = new UserBL().UpdateNotification(id,Convert.ToInt32(LoggedinUserId),isdelete, de);
            if (update.Result == true)
                return true;
            else
                return false;
        }

        [HttpPost]
        public IActionResult GetUserById(int id)
        {
            User u = new UserBL().GetActiveUserById(id, de);
            if (u == null)
            {
                return Json(0);
            }

            UserDto obj = new UserDto()
            {
                Id = u.Id,
                EncId = StringCipher.EncryptId(u.Id),
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Contact = u.Contact,
                Password = StringCipher.Decrypt(u.Password),
                Country = u.Country,
                Description = u.Description,
                UserName = u.Username,
                Gender = u.Gender,
                Website = u.Website,
                ZipCode = u.ZipCode,
                Role = u.Role,
                City = u.City,
                Organization = u.Organization,
                ImageName = u.ImagePath,

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

        #region Searches

        public List<string> GetSuggestions(string keyword = "")
        {
            try
            {
                if (keyword != "" && keyword != null)
                {
                    var results = new List<string>();

                    var getSkills = new UserBL().GetAllSkills(de).Where(a => a.SkillName.ToLower().Contains(keyword.ToLower())).ToList();
                    var getTags = new UserBL().GetAllTags(de).Where(a => a.TagName.ToLower().Contains(keyword.ToLower())).ToList();
                    if (getSkills.Count != 0)
                    {
                        for (int i = 0; i < getSkills.Count(); i++)
                        {
                            var getSkillstring = getSkills[i].SkillName.Split(',');
                            foreach (var x in getSkillstring)
                            {
                                if (x.Contains(keyword) && !results.Contains(x))
                                {
                                    results.Add(x);
                                }
                            }

                        }
                    }
                    else if (getTags.Count != 0)
                    {
                        for (int i = 0; i < getSkills.Count(); i++)
                        {
                            var getTagsString = getTags[i].TagName.Split(',');
                            foreach (var x in getTagsString)
                            {
                                if (x.Contains(keyword) && !results.Contains(x))
                                {

                                    results.Add(x);
                                }
                            }

                        }
                    }
                    results = results.Take(5).ToList();
                    return results;
                }
                return null;
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

        #region Messages

        [HttpPost]
        public string GetChatList(string Name = "")
        {
            try
            {
                var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
                var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
                var userr = CurrentUserRecord;
                var login = userr.Id;

                var frstrec = new List<Message>();

                frstrec = new ChatBL(confg).GetAllChats().Where(a => a.RecieverId == login || a.SenderId == login && a.IsActive == 1).Where(a => a.OrderId == 0).ToList();
               

                if (!String.IsNullOrEmpty(Name))
                {
                    frstrec = frstrec.Where(a => a.RecieverEnd.FirstName.ToLower().Contains(Name.ToLower()) || a.Users.FirstName.ToLower().Contains(Name.ToLower()) || a.RecieverEnd.LastName.ToLower().Contains(Name.ToLower()) || a.Users.LastName.ToLower().Contains(Name.ToLower())).ToList();
                }
                if (frstrec.Count() != 0)
                {
                    frstrec = frstrec.OrderByDescending(a => a.CreatedAt).ToList();

                    return JsonConvert.SerializeObject(frstrec, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                }
                else
                {
                    return "";
                }

            }
            catch
            {
                return null;
            }

        }

        [HttpPost]
        public string GetAllChats(int recieverid = -1)
        {
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
            var login = CurrentUserRecord.Id;
            if (recieverid != -1)
            {
                var msgz = new ChatBL(confg).GetAllChats().Where(a => (a.SenderId == login && a.RecieverId == recieverid) || (a.RecieverId == login && a.SenderId == recieverid)).Where(a => a.OrderId == 0).ToList();
                msgz = msgz.OrderBy(a => a.CreatedAt).ToList();
                return JsonConvert.SerializeObject(msgz, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            return null;
        }

        public bool UpdateChat(int id = -1, string modified_msg = "")
        {
            try
            {
                if (id != -1 && modified_msg != "")
                {
                    var upd_chat = new ChatBL(confg).UpdateChat(id, modified_msg);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificationsCount(string receiverid, int IsRead = -1, string Title = "")
        {
           
                
                    
            var  nCount = new UserBL().Getnotificationcount(Convert.ToInt32(receiverid), de).Where(x=>x.IsActive==1 && x.IsRead==0).Count();

           
            //if (nCount != 0)
            //{
            //    var noofisreadCount = new UserBL().Getnotificationcount(Convert.ToInt32(receiverid), de).Where(x=>x.IsActive==1 && x.IsRead==1).Count();

            //    if(nCount>=noofisreadCount)
            //    {
            //        nCount = nCount - noofisreadCount;
            //    }
                
            //    //return Json(nCount);
            //}

            return Json(nCount);
        }


        public async Task<IActionResult> GetNotifications(string receiverid, int isRead = -1)
        {
            
            var notificationList = new List<Notification>();

            notificationList = new UserBL().Getnotificationcount(Convert.ToInt32(receiverid), de).Where(x => x.IsActive == 1).OrderByDescending(x=>x.Id).ToList();

            //if (isRead == 1)
            //{
            //    notificationList = notificationList.Where(x => x.IsRead == 1).ToList();
            //}
            //if (isRead == 0)
            //{
            //    notificationList = notificationList.Where(x => x.IsRead == 0).ToList();
            //}
            
            return Json(notificationList);
        }


        public bool Unreadrequests(int id = -1, int cnt = -1)
        {
            try
            {
                if (id != -1 && cnt != -1)
                {
                    var getUser = new UserBL().GetActiveUserById(id, de);
                    getUser.ConnectionId = cnt.ToString();
                    var updt = new UserBL().UpdateUser(getUser, de);
                    return updt.Result;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public bool UnreadChat(int id = -1, int cnt = -1)
        {
            try
            {
                if (id != -1 && cnt != -1)
                {
                    var getUser = new UserBL().GetActiveUserById(id, de);
                    getUser.ConnectionId = cnt.ToString();
                    var updt = new UserBL().UpdateUser(getUser, de);
                    return updt.Result;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool UnreadComments(int id = -1, int cnt = -1, int UserId = -1)
        {
            try
            {
                if (id != -1 && cnt != -1 && UserId != -1)
                {
                    var getOrder = new UserBL().GetOrderById(id, de);
                    if (UserId == getOrder.Buyer.Id)
                    {
                        getOrder.BuyerCommentsCount = cnt.ToString();
                    }
                    else
                    {
                        getOrder.SellerCommentsCount = cnt.ToString();
                    }
                    var updt = new UserBL().UpdateOrder(getOrder, de);
                    return updt;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public string GetUnreadCountById(int OrderId = -1, int UserId = -1)
        {
            try
            {
                var Id = Convert.ToInt32(OrderId);
                var getOrder = new UserBL().GetOrderById(Id, de);
                var getUser = new UserBL().GetActiveUserById(Convert.ToInt32(UserId), de);
                string count = "";
                if (getUser.Role == 3)
                {
                    count = getOrder.SellerCommentsCount;
                }
                else
                {
                    count = getOrder.BuyerCommentsCount;
                }
                return count;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region Education
        public bool DeleteEducation(int id = -1)
        {
            try
            {
                if (id != -1)
                {
                    var check = new UserBL().DeleteEducationById(id, de);
                    return check;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }


        public bool UpdateEducation(int id = -1, string ins = "", string deg = "", string sd = "", string ed = "")
        {


            try
            {



                if (id != -1)
                {
                    var n = new UserBL().GetEducationById(id, de);
                    if (n != null)
                    {
                        n.StartDate = Convert.ToDateTime(sd);
                        n.EndDate = Convert.ToDateTime(ed);
                        n.InstituteName = ins;
                        n.DegreeName = deg;
                        n.UpdatedAt = DateTime.UtcNow;
                        var upd= new UserBL().UpdateEducation(n,de);
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region ZoomMeeting
        public IActionResult ZoomMeeting()
        {

            var getId = User.Claims.Where(a => a.Type == "id").FirstOrDefault();
            var Id = Convert.ToInt32(getId);
            var GetLoggedinUser = new UserBL().GetActiveUserById(Id, de);
            //Getting model token handler
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;

            //User API Secret
            var apiSecret = "7c23fjXKFqeFrV0LG6cJbGkLqlPduvBHItxL";

            //Getting Encrypted Key
            byte[] symmetricKey = Encoding.ASCII.GetBytes(apiSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "ou270DeyS92TZp9Yvbf0Ig",
                Expires = now.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var client = new RestClient("https://api.zoom.us/v2/users/usman78056@gmail.com/meetings");
            var request = new RestRequest(Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new { topic = "Zoom Meeting", duration = "40", starttime = DateTime.UtcNow, type = "2" });

            request.AddHeader("authorization", String.Format("Bearer {0}", tokenString));



            IRestResponse restResponse = client.Execute(request);
            HttpStatusCode statusCode = restResponse.StatusCode;
            int numericStatusCode = (int)statusCode;
            var jObject = JObject.Parse(restResponse.Content);


            //StartURL// HOSTLINK
            var Host = (string)jObject["start_url"];

            //Joining Link
            var Join = (string)jObject["join_url"];

            var links = new List<string>();


            links.Add(Host);
            links.Add(Join);
            //ViewBag.Response = Convert.ToString(numericStatusCode);

            return Json(links);
        }
        #endregion

        #region Order
        public bool PlaceOrder(int buyer = -1, int seller = -1, int ordid = -1)
        {
            try
            {
                if (buyer != -1 && seller != -1 && ordid != -1)
                {
                    var order = new UserBL().GetOrderById(ordid, de);

                    order.IsAccepted = 1;
                    order.CreatedAt = DateTime.UtcNow;
                    var update = new UserBL().UpdateOrder(order, de);

                    return update;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Delivery(int id = -1, int acp = -1, int str = -1, string exp = "", string upl = "", string des = "")
        {
            try
            {
                var transfer_Amount_To_Seller = false;
                var transfer_Amount_To_Referral = false;//massam(26-12-22)
                var update = false;
                if (id != -1)
                {
                    var getorder = new OrderBL().GetOrderById(id, de);
                    //massam 07-01-23
                    var user = new UserBL().GetUserById(getorder.SellerId, de);
                    //end massam 07-01-23

                    getorder.IsDelivered = 1;
                    if (acp != -1)
                    {
                        getorder.IsDelivered = acp;
                        if (acp == 2)
                        {
                            if (getorder.paidBy == 2)
                            {
                                var orderprice = getorder.OrderPrice;
                                getorder.OrderPrice = 0;
                                var chargeIds = getorder.Charge.Split(",");
                                for (int i = 0; i < chargeIds.Length; i++)
                                {
                                    var subscription = new OrderBL().GetSubscriptionById(Int32.Parse(chargeIds[i]), de);
                                    if (subscription.subscriptionType == 1)
                                    {
                                        getorder.OrderPrice = 12 * subscription.subtractedSessions;
                                    }
                                    else
                                    {
                                        getorder.OrderPrice = 12 * subscription.subtractedSessions;
                                    }
                                    transfer_Amount_To_Seller = TransferToSeller((int)getorder.OrderPrice, getorder.OrderTitle, id, subscription.SubscriptionCharge);
                                    if (transfer_Amount_To_Seller == true)
                                    {
                                        if (subscription.SessionCount <= 0)
                                        {
                                            subscription.IsActive = 0;
                                        }
                                        new OrderBL().UpdateSubscription(subscription, de);
                                        //new OrderBL().UpdateOrder(getorder, de);
                                    }
                                    else
                                    {
                                        return false;
                                        //break;
                                    }
                                }
                                getorder.OrderPrice = orderprice;
                                new OrderBL().UpdateOrder(getorder, de);
                            }
                            else
                            {
                                transfer_Amount_To_Seller = TransferToSeller((int)getorder.OrderPrice, getorder.OrderTitle, id);
                                //massam(26-12-22)
                                var butlerReferralReccord = new UserBL().GetReferralByButlerId(getorder.SellerId, de);
                                if (butlerReferralReccord != null)
                                {
                                    if (butlerReferralReccord.RefferalType == 1)
                                    {
                                        if (getorder.CreatedAt.Value.Date <= butlerReferralReccord.ReferralEndTime.Value.Date)
                                        {
                                            int referredByUserId = new UserBL().GetUserReccordByReferralCode(butlerReferralReccord.RefferalCode, de).Id;
                                            double referralCommission = (getorder.OrderPrice * 5) / 100;
                                            transfer_Amount_To_Referral = TransferToReferral(referralCommission, id, referredByUserId);
                                        }
                                    }
                                    if (butlerReferralReccord.RefferalType == 2)
                                    {
                                        int referredByUserId = new UserBL().GetUserReccordByReferralCode(butlerReferralReccord.RefferalCode, de).Id;
                                        double referralCommission = (double)(getorder.OrderPrice * 2) / 100;
                                        transfer_Amount_To_Referral = TransferToReferral(referralCommission, id, referredByUserId);
                                    }
                                }
                                //End massam(26-12-22)



                            }
                        }
                    }
                    getorder.UpdatedAt = DateTime.UtcNow;
                    if (upl != "" && upl != null)
                    {
                        var Review = new OrderReview
                        {
                            SellerComment = des,
                            OrdId = id,
                            CreatedAt = DateTime.UtcNow,
                            FilePath = upl,
                            IsActive = 1,
                            CommentId = getorder.SellerId,
                            CommentUser = getorder.BuyerId,
                        };
                        var addReview = new UserBL().AddReview(Review, de);
                    }
                    if (str != -1)
                    {
                        var Review = new OrderReview
                        {
                            BuyerComment = exp,
                            OrdId = id,
                            CreatedAt = DateTime.UtcNow,
                            IsActive = 1,
                            Stars = str,
                            CommentId = getorder.BuyerId,
                            CommentUser = getorder.SellerId,
                        };
                        var addReview = new UserBL().AddReview(Review, de);
                        // massam 07 - 01 - 23
                        var olist = new UserBL().GetOrderReviewByUserId(user.Id, de).OrderByDescending(x => x.CreatedAt).Take(5).Where(x => x.Stars == 1).Count();
                        if (olist == 5)
                        {
                            user.IsActive = 4;
                            new UserBL().UpdateUser(user, de);
                        }
                        var rlist = new UserBL().GetOrderReviewByUserId(user.Id, de).OrderByDescending(x => x.CreatedAt).Take(3).Where(x => x.Stars == 1).Count();
                        if (rlist == 3)
                        {
                            MailSender.AccountHoldWarning(user.Email);
                        }
                        //end massam 07-01-23

                    }
                    if (acp != 2)
                    {
                        update = new OrderBL().UpdateOrder(getorder, de);
                        //massam 7jan23
                        if (getorder.IsDelivered == 1)
                        {
                            var customerEmail = new UserBL().GetUserById(getorder.BuyerId, de).Email;
                            MailSender.OrderDelivered(customerEmail);
                        }
                        //end massam 7jan23
                    }
                    else
                    {
                        if (transfer_Amount_To_Seller == true)
                        {
                            update = new OrderBL().UpdateOrder(getorder, de);
                            //massam 7jan23
                            var butlerEmail = new UserBL().GetUserById(getorder.SellerId, de).Email; 
                            MailSender.OrderCompleted(butlerEmail);
                            //end massam 7jan23
                        }
                        else
                        {
                            return false;
                        }
                    }
                    //Calculating Stars

                    var getStars = new UserBL().GetAllReviews(de).Where(a => a.CommentUser == getorder.SellerId).Select(a => a.Stars).ToList();
                    var getTotalStars = 0;
                    if(getStars.Count()!=0)
                    {
                        getTotalStars = getStars.Sum() / getStars.Count();
                    }

                    //getting User
                    var getSeller = new UserBL().GetActiveUserById(getorder.SellerId, de);
                    //Updating Stars
                    getSeller.Stars = getTotalStars;
                    var updateSellerStars = new UserBL().UpdateUser(getSeller, de);
                    return update;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool TransferToReferral(double amount, int orderid = -1, int userId = -1)
        {
            try
            {
                var getSourceCharge = new OrderBL().GetOrderById(orderid, de);
                var getUser = new UserBL().GetActiveUserById(userId, de);
                var options = new TransferCreateOptions
                {
                    Currency = "CAD",
                    Destination = getUser.StripeId,
                };
                options.Amount = (long)(Math.Round((double)amount, 2) * 100);
                options.SourceTransaction = getSourceCharge.Charge;
                var service = new TransferService();
                var charge = service.Create(options);
                if (charge != null)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public bool AddSellerReview(int OrderId = -1, int stars = -1, string review = "")
        {
            try
            {
                var getOrder = new UserBL().GetOrderById(OrderId, de);

                var Review = new OrderReview
                {
                    SellerComment = review,
                    OrdId = OrderId,
                    Stars = stars,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = 1,
                    CommentId = getOrder.SellerId,
                    CommentUser = getOrder.BuyerId,
                };

                var AddReview = new UserBL().AddReview(Review, de);

                return AddReview;
            }
            catch
            {
                return false;
            }
        }
        public FileResult DownloadFile(string fileName)
        {


            if (fileName.Contains(baseUrl))
            {
                fileName = fileName.Split(baseUrl)[1];
            }
            string path = "wwwRoot/" + fileName;
            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }



        public bool updateOrder(int id = -1, string reason = "", int reason_type = -1, string End_Date = "", int decision = -2)
        {
            try
            {
                var getOrder = new OrderBL().GetOrderById(id, de);
                if (decision == 1)
                {
                    if (reason_type == 1 || reason_type == 3)  // reason_type 1 extend // reason_type 2 cancellation // reason_type 3 revision
                    {
                        var adddays = Convert.ToInt32(End_Date.Split(" ")[0]);
                        getOrder.EndDate = getOrder.EndDate.Value.AddDays(adddays);
                        if (reason_type == 1)
                            getOrder.Extending_Reason = reason;
                        else
                            getOrder.Revision_Reason = reason;
                    }
                    else if (reason_type == 2)
                    {
                        getOrder.Cancellation_Reason = reason;
                        getOrder.IsAccepted = -1;
                        if (getOrder.paidBy == 2)
                        {
                            var chargeIds = getOrder.Charge.Split(",");
                            for (int i = 0; i < chargeIds.Length; i++)
                            {
                                var subscription = new OrderBL().GetSubscriptionById(Int32.Parse(chargeIds[i]), de);
                                subscription.SessionCount = subscription.SessionCount + subscription.subtractedSessions;
                                subscription.subtractedSessions = 0;
                                new OrderBL().UpdateSubscription(subscription, de);
                            }
                        }
                        else
                        {
                            var refundCharge = new RefundCreateOptions()
                            {
                                Charge = getOrder.Charge,
                                Amount = getOrder.OrderPrice * 100 - Convert.ToInt32(getOrder.OrderPrice * 3.34),
                            };
                            var refundService = new RefundService();
                            var createRefund = refundService.Create(refundCharge);
                        }
                    }
                }
                var update_order = new OrderBL().UpdateOrder(getOrder, de);
                return update_order;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Stripe
        public bool Charge(string stripeEmail, string stripeToken, long stripeAmount = -1, string stripeDescription = "", string orderid = "")
        {
            try
            {
                //Get Order Details
                var getOrder = new OrderBL().GetOrderById(Int32.Parse(orderid), de);
                //Creating Stripe Services
                var customerService = new CustomerService();
                var chargeService = new ChargeService();
                var serviceToken = new TokenService();
                var customer = customerService.Create(new CustomerCreateOptions
                {
                    Email = stripeEmail,
                    Source = stripeToken,
                });
                var options = new ChargeCreateOptions
                {
                    Amount = stripeAmount * 100,
                    Currency = "CAD",
                    Description = stripeDescription,
                    Customer = customer.Id,
                };
                var charge = chargeService.Create(options);
                //saving chargeId for later transfer
                getOrder.Charge = charge.Id;
                var update = new OrderBL().UpdateOrder(getOrder, de);
                if (charge.Paid == true)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool SubscriptionCharge(string stripeEmail, string stripeToken, long stripeAmount = -1, string stripeDescription = "", int subscriptionId = -1)
        {
            try
            {
                //Get Order Details
                var getSubscription = new OrderBL().GetSubscriptionById(subscriptionId, de);
                //Creating Stripe Services
                var customerService = new CustomerService();
                var chargeService = new ChargeService();
                var serviceToken = new TokenService();
                var customer = customerService.Create(new CustomerCreateOptions
                {
                    Email = stripeEmail,
                    Source = stripeToken,
                });
                var options = new ChargeCreateOptions
                {
                    Amount = stripeAmount * 100,
                    Currency = "CAD",
                    Description = stripeDescription,
                    Customer = customer.Id,
                };
                var charge = chargeService.Create(options);
                //saving chargeId for later transfer
                getSubscription.SubscriptionCharge = charge.Id;
                var updateSubscription = new OrderBL().UpdateSubscription(getSubscription, de);
                if (charge.Paid == true)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public bool TransferToSeller(int amount, string description, int orderid = -1, string chargeId = "")
        {
            try
            {
                var getSourceCharge = new OrderBL().GetOrderById(orderid, de);
                var getUser = new UserBL().GetActiveUserById(getSourceCharge.SellerId, de);
                var options = new TransferCreateOptions
                {
                    Currency = "CAD",
                    Destination = getUser.StripeId,
                };
                if (getSourceCharge.paidBy == 2)
                {
                    options.Amount = amount*100;
                    options.SourceTransaction = chargeId;
                }
                else
                {
                    //options.Amount = (getSourceCharge.OrderPrice - (Convert.ToInt32(getSourceCharge.OrderPrice * (3.34 + 20)) / 100)) * 100;
                    //options.Amount = (getSourceCharge.OrderPrice - (getSourceCharge.OrderPrice * (17) / 100)) * 100;
                    if (getUser.StartingFrom == "25")
                    {
                        options.Amount = (getSourceCharge.OrderPrice - (getSourceCharge.OrderPrice * (17) / 100)) * 100;
                    }
                    else if (getUser.StartingFrom == "30")
                    {
                        options.Amount = (getSourceCharge.OrderPrice - (getSourceCharge.OrderPrice * (14) / 100)) * 100;
                    }
                    else
                    {
                        options.Amount = (getSourceCharge.OrderPrice - (getSourceCharge.OrderPrice * (12) / 100)) * 100;
                    }
                    options.SourceTransaction = getSourceCharge.Charge;
                }
                //var options = new TransferCreateOptions
                //{
                //    Amount = (getSourceCharge.OrderPrice - (Convert.ToInt32(getSourceCharge.OrderPrice * (3.34 + 20)) / 100)) * 100,
                //    Currency = "CAD",
                //    SourceTransaction = getSourceCharge.Charge,
                //    Destination = getUser.StripeId,
                //};
                var service = new TransferService();
                var charge = service.Create(options);
                if (charge != null)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public string GetCountryByIP()
        {

            string info = new WebClient().DownloadString("http://ipinfo.io");

            var ipInfo = JsonConvert.DeserializeObject<IPDetails>(info);

            RegionInfo region = new RegionInfo(ipInfo.Country);

            return region.Name;

        }

        

        public async Task<List<string>> CreateAccount(string email = "")
        {
            try
            {
                var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
                var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
                //var region = GetCountryByIP();
                var options = new AccountCreateOptions
                {

                    Type = "custom",
                    //Country = region,
                    Country = "CA",
                    Email = email,
                    DefaultCurrency = "usd",
                    Capabilities = new AccountCapabilitiesOptions
                    {
                        CardPayments = new AccountCapabilitiesCardPaymentsOptions
                        {
                            Requested = true,
                        },
                        Transfers = new AccountCapabilitiesTransfersOptions
                        {
                            Requested = true,
                        },

                    },
                    Settings = new AccountSettingsOptions()
                    {

                        Payouts = new AccountSettingsPayoutsOptions()
                        {
                            Schedule = new AccountSettingsPayoutsScheduleOptions()
                            {
                                Interval = "manual",
                            },
                        }
                    },
                    BusinessType = "individual",
                };


                var service = new AccountService();
                var account = service.Create(options);


                var result = Generate_Verification_Link(account.Id,"",CurrentUserRecord.Id);
                var _dataResponse = JToken.Parse(JsonConvert.SerializeObject(result));
                var Verification_Link = _dataResponse.Value<string>("Value") ?? "";




                var getId = Convert.ToInt32(User.Claims.Where(a => a.Type == ClaimTypes.Sid).FirstOrDefault().Value);


                var getLoggedinUser = new UserBL().GetActiveUserById(getId, de);
                
                getLoggedinUser.StripeId = account.Id;

                getLoggedinUser.Account_Verification_Link = Verification_Link;

                var update_user = await new UserBL().UpdateUser(getLoggedinUser, de);


                var lists= new List<string>();
                lists.Add(Verification_Link);
                lists.Add(account.Id);

              

                return lists;
            }
            catch
            {
                return null;
            }
        }

        public IActionResult DeleteStripe(string user_id="")
        {
            try
            {
                if(user_id != "")
                {
                    var GetUser = new UserBL().GetActiveUserById(Convert.ToInt32(StringCipher.Decryptid(user_id)), de);
                    GetUser.Account_Verification_Link = "";
                    GetUser.StripeId = "";
                    GetUser.Is_Verified = 0;
                    var delete_stripe = new UserBL().UpdateUser(GetUser, de);
                    return RedirectToAction("Account","Seller");
                }
                return RedirectToAction("Account", new {msg="Account Not Found ! Please Try Again !"});
            }
            catch
            {
                return RedirectToAction("Account", new { msg = "Something went wrong ! Please Try Again" });
              
            }
        }

        public bool ValidateStripe(string val="")
        {
            try
            {
                if (val != "" && val!=null)
                {
                    var GetService = new AccountService();
                    var GetAccount = GetService.Get(val);
                    if (GetAccount.StripeResponse.StatusCode== HttpStatusCode.OK)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public IActionResult Generate_Verification_Link(string Id, string typ="", int Uid=-1)
        {
            var AccountLinkService = new AccountLinkService();

            var result = AccountLinkService.Create(new AccountLinkCreateOptions
            {
                Account = Id,
                RefreshUrl = baseUrl + "/Account?msg=Account Verification Failed",
                ReturnUrl = baseUrl + "/Account?msg=Account Verification Success&verify_id="+Uid,
                Type = "account_onboarding",
                Collect = "eventually_due",
            });
            if (typ == "1")
            {
                return Redirect(result.Url);
            }
            return Json(result.Url);
        }

        public async Task<bool> Add_Bank(string user_id="",string connect_acc_id = "", string acc_no = "", string routing_no = "", string acc_holder_name = "")
        {
            try
            {

                //Required Info 

                //AccountNumber = acc_no,
                //AccountHolderName = acc_holder_name,
                //AccountHolderType = "individual",
                //Country = "US",
                //RoutingNumber = routing_no,
                //Currency = "USD",

                //var region = GetCountryByIP();

                var options = new ExternalAccountCreateOptions
                {
                    ExternalAccount = new AccountBankAccountOptions
                    {
                        AccountNumber = acc_no,
                        AccountHolderName = acc_holder_name,
                        AccountHolderType = "individual",
                        Country = "CA",
                        //Country = region,
                        RoutingNumber = routing_no,
                        Currency= "CAD",
                    }
                };

                var service = new ExternalAccountService();
                service.Create(connect_acc_id, options);
                
                var getUser = new UserBL().GetActiveUserById(Convert.ToInt32(user_id), de);
                getUser.Is_Verified = 2;
                var update_user = await new UserBL().UpdateUser(getUser, de);

                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool Payout_User(string Amount="",string user_id="")
        {
            try
            {
                var getUser = new UserBL().GetActiveUserById(Convert.ToInt32(user_id), de);


                //Amount = "12";
                var payout_to_bank = new PayoutCreateOptions
                {
                    
                    Amount = Convert.ToInt32(Amount)*100,
                    Currency = "cad",
                   
                };
                var requestOptions = new RequestOptions();
                requestOptions.StripeAccount = getUser.StripeId;
                
                
                var payoutservice = new PayoutService();

                var payout_create= payoutservice.Create(payout_to_bank,requestOptions);

                return true;
            }
            catch
            {
                return false;
            }
          
        }
        #endregion
    }


}

