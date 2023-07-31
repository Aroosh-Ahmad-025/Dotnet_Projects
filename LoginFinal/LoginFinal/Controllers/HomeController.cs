using LoginFinal.Models;
using Microsoft.AspNetCore.Mvc;
using LoginFinal.HelpingClasses;
using LoginFinal.Filters;
using Microsoft.AspNetCore.Http;
using LoginFinal.BL;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Net;
using System.Reflection;
using X.PagedList;
using static LoginFinal.HelpingClasses.ProjectVariables;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.SignalR;
using LoginFinal.DataHub;
using Nancy.Extensions;
using ExpressTimezone;

namespace LoginFinal.Controllers
{
    [ExceptionFilter]
    public class HomeController : Controller
    {
        private readonly SqlConnection de;
        private readonly IHttpContextAccessor haccess;
        private readonly IConfiguration confg;
        private readonly IHubContext<ChatHub> HubContext;
        public HomeController(SqlConnection de, IConfiguration confg, IHttpContextAccessor haccess, IHubContext<ChatHub> hubContext)
        {
            //this.de = new SqlConnection(confg.GetConnectionString("Default"));
            this.de = de;
            var request = haccess.HttpContext.Request;
            this.haccess = haccess;
            this.confg = confg;
            baseUrl = $"{request.Scheme}://{request.Host}";
            this.HubContext = hubContext;
        }

        #region LandingPage
        //Landing Page
        [HttpGet]
        public IActionResult Welcome()
        {
            return View();
        }



        public IActionResult Index(string msg="")
        {

           //get logged in user details
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);

            ViewBag.msg = msg;
            
            //get all details about logged in user
            var getLogs = new List<Logging>();
            
            var get_all_ids = new List<int?>();

            var getAllids =  new List<List<int?>>();

            var getTags = new UserBL().GetAllLogs(de).OrderByDescending(a=>a.CreatedAt).Distinct().ToList();
            
            var pop = new List<String>();
            
            var rv = new List<User>();
            
            var recm = new List<User>();

            //Check if user is logged in 
            if(CurrentUserRecord != null)
            {
                getLogs= new UserBL().GetAllLogs(de).Where(a=>a.UserId== CurrentUserRecord.Id)/*.DistinctBy(x => x.UserId)*/.ToList();
                if (getLogs.Count() != 0)
                {
                    foreach (var keyword in getLogs)
                    {
                        var km = keyword.SearchKeyword.Split(',');
                        foreach (var kw in km)
                        {
                            get_all_ids = new UserBL().GetAllLogs(de).Where(a => a.SearchKeyword.Contains(kw)).Select(x => x.ProfileId).ToList();
                            get_all_ids = get_all_ids.Distinct().ToList();
                            getAllids.Add(get_all_ids);
                        }
                    }
                }
            }

            getAllids = getAllids.Distinct().ToList();
            //get recommended profiles
            foreach (var x in getAllids)
            {
                foreach (var y in x)
                {
                    var m= y ?? default(int);

                    var recmd = new UserBL().GetUserById(m, de);
                    if (!recm.Contains(recmd))
                    {
                        if (recmd != null)
                        {
                            recm.Add(recmd);
                        }
                    }
                }
            }

            //Get recently viewed users
            foreach (var x in getLogs)
            {
                var m = x.ProfileId ?? default(int);
                var recent = new UserBL().GetUserById(m, de);
                if(!rv.Contains(recent))
                {
                    if (recent != null)
                    {
                        rv.Add(recent);
                    }
                }
            }

            //get all popular tags 
            foreach (var x in getTags)
            {
                var y = x.SearchKeyword.Split(" ");
                foreach (var z in y)
                {
                    if (!pop.Contains(z))
                    {
                        pop.Add(z);
                    }
                }
            }

            //recently viewed sorting
            if (rv.Count() != 0)
            {
                rv = rv.OrderByDescending(a => a.CreatedAt).ToList();
            }

            //get top 5 popular tags
            pop = pop.Take(5).ToList();
          
            
            //popular tags
            ViewBag.Popular = pop;
            

            //recently viewed profiles
            ViewBag.rv = rv.DistinctBy(x=>x.Id);
            
            //recommended profiles
            ViewBag.recm = recm.DistinctBy(x=>x.Id);
           
            return View();
        }

        public IActionResult SignUpRefferal(string refferal)
        {
            ViewBag.Refferal_Code = refferal;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        #endregion

        #region Search


        [Route("Search")]
        public IActionResult Search(string search, int currentPage = 1)
        {

            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            try
            {
                ViewBag.srch = search;
                if (search == null || search == "")
                {
                    return RedirectToAction("Index");
                }

                //Saving Search For Profile Refresh
                ViewBag.search = search;

                if (Userid != null)
                {
                    Logging log = new Logging
                    {
                        SearchKeyword = search.ToLower(),
                        CreatedAt = DateTime.UtcNow,
                        IsActive = 1,
                        UserId = Convert.ToInt32(Userid),
                    };
                    var logd = new UserBL().AddLog(log, de);
                }
                else
                {
                    Logging log = new Logging
                    {
                        SearchKeyword = search.ToLower(),
                        CreatedAt = DateTime.UtcNow,
                        IsActive = 1,
                        UserId=1,
                    };
                    var logd = new UserBL().AddLog(log, de);
                }
                
                //List for Getting All Selected Users
                List<User> users = new List<User>();

                //spliting search keywords
                string[] search2 = search.Split(" ");

                for (int i = 0; i < search2.Length; i++)
                {
                    //Matching Results

                    //Getting all skills 
                    var skills = new UserBL().GetAllSkills(de).Where(x => x.SkillName.Contains(search2[i].ToLower())).ToList();

                    //Getting Tags
                    var tags = new UserBL().GetAllTags(de).Where(x => x.TagName.Contains(search2[i].ToLower())).ToList();

                    //Getting Matching UserName or Name
                    var usern = new UserBL().GetActiveUserList(de).Where(x => (x.FirstName.ToLower() == search2[i].ToLower()) || (x.FirstName.ToLower() == search2[0].ToLower() && x.LastName.ToLower().Contains(search2[i].ToLower()) || (x.Username.ToLower() == search2[i].ToLower()))).ToList();

                    usern = usern.Where(x => x.Role == 3).ToList();


                    //If doesn't match skills
                    if (skills.Count == 0)
                    {
                        //if match tags
                        if (tags.Count != 0)
                        {
                            foreach (var tag in tags)
                            {
                                var user = new UserBL().GetUserById(tag.UserId, de);
                                if (!users.Contains(user))
                                {
                                    users.Add(user);
                                }
                            }

                            //Collect all users after last ilteration
                            if (i == search2.Length - 1)
                            {
                                if (users != null)
                                {
                                    users = users.OrderByDescending(a => a.Stars).ThenBy(a => a.Status == 0).DistinctBy(a=>a.Id).ToList();
                                }
                                var pagedlist = users.ToPagedList(currentPage, 12);
                                return View(pagedlist);
                            }

                        }
                    }
                    
                    //If Matches Skills
                    if (skills.Count != 0)
                    {

                        //get User Matching Skills
                        foreach (var skill in skills)
                        {
                            var user = new UserBL().GetUserById(skill.UserId, de);
                            if (!users.Contains(user))
                            {
                                if (user != null && user.Stars == null)
                                {
                                    user.Stars = 0;
                                }
                                if (user != null)
                                {
                                    users.Add(user);
                                }
                            }

                        }

                        //Collect all users after last ilteration
                        if (i == search2.Length - 1)
                        {
                            if (users.Count() != 0)
                            {
                                users = users.OrderByDescending(a => a.Stars).ThenBy(a => a.Status == 0).DistinctBy(a => a.Id).ToList();
                                var pagedlist = users.ToPagedList(currentPage, 12);
                                return View(pagedlist);
                            }

                        }
                    }



                    //If keywords matches username or name
                    if (usern.Count != 0)
                    {
                        foreach (var w in usern)
                        {
                            if (!users.Contains(w))
                            {
                                if (w != null && w.Stars == null)
                                {
                                    w.Stars = 0;
                                }
                                if (w != null)
                                {
                                    users.Add(w);
                                }
                            }
                        }


                        //Collect all users after last ilteration
                        if (i == search2.Length - 1)
                        {
                            if (users.Count() != 0)
                            {
                                users = users.OrderByDescending(a => a.Stars).ThenBy(a => a.Status == 0).DistinctBy(a => a.Id).ToList();

                                var pagedlist = users.ToPagedList(currentPage, 12);
                                if (pagedlist.Count! != 0)
                                {
                                    return View(pagedlist);
                                }
                            }
                            return RedirectToAction("NoResultFound", new { srch=search});
                        }
                    }
                }
                return RedirectToAction("NoResultFound", new { srch = search });
            }
            catch
            {
                return RedirectToAction("NoResultFound", new { srch = search });
            }
           
        }

        [AllowAnonymous]
        [Route("Preview")]
        public IActionResult UserProfile(string idx, int pr = -1, string keyword = "", string msg = "")
        {
           // getLastrequestbyuserid
            ViewBag.msg = msg;
            ViewBag.userRequestType = null;

            var utcstarttime = DateTime.Now.ToUniversalTime();
            var utcSystemTimeFormat = utcstarttime.UTCToSystemTime();
            ViewBag.systemdatetime= utcSystemTimeFormat;

            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);


            if (CurrentUserRecord.Role == 4)
            {
                var getuserLastRequest = new UserBL().getLastrequestbyuserid(Convert.ToInt32(Userid), de);
                if (getuserLastRequest != null)
                {
                    ViewBag.userRequestType = getuserLastRequest.RequestType;
                }
                //ViewBag.userRequestType=getuserLastRequest.RequestType;
            }
            var id = StringCipher.DecryptId(idx);
            var subscriptions = new OrderBL().GetSubscriptionsAgainstSpecificButler(Convert.ToInt32(Userid), id, de);
            var sessionCount = 0;
            foreach (var sub in subscriptions)
            {
                if (DateTime.UtcNow >= sub.SubscriptionEnd)
                {
                    sub.IsActive = 0;
                }
                if (sub.IsActive > 0)
                {
                    sessionCount += sub.SessionCount;
                }
            }
            ViewBag.subscribed = sessionCount;
            ViewBag.RecId = id;
            var getuserexperience = new UserBL().Getexperiencebyuserid(Convert.ToInt32(id), de);
            //var check_connectivity = new GeneralPurpose(de, haccess).CheckInternet();
            ViewBag.userexperience = getuserexperience;
            ViewBag.srch = keyword;

            ViewBag.prefertimeforsession = "N/A";
            var getbutlerprefertime = new UserBL().Getbutlerprefertime(Convert.ToInt32(id), de);
            if (getbutlerprefertime.Count() > 0)
            {
                ViewBag.prefertimeforsession=getbutlerprefertime;
            }
            var userlanguage = new UserBL().Getuserlanguagebyuserid(id, de).Select(a=>a.language).ToList();
            ViewBag.userlanguages = "N/A";

            if(userlanguage.Count > 0)
            {
               var languageString = string.Join(",", userlanguage);
                ViewBag.userlanguages= languageString;

            }
            //Logging Opened Profile
            if (keyword != "" && CurrentUserRecord != null)
            {
                Logging log = new Logging
                {
                    IsActive = 1,
                    CreatedAt = DateTime.UtcNow,
                    SearchKeyword = keyword,
                    ProfileId = id,
                    UserId = Convert.ToInt32(Userid)
                };
                var logd = new UserBL().AddLog(log, de);
            }
            //Getting User Details
            var user = new UserBL().GetActiveUserById(id, de);
            var getUserEduc = new UserBL().GetAllEducations(de).Where(a => a.UserId == id).ToList();
            var getUserTags = new UserBL().GetAllTags(de).Where(a => a.UserId == id).ToList();
            var getUserSkills = new UserBL().GetAllSkills(de).Where(a => a.UserId == id).ToList();
            user.Education = getUserEduc;
            user.Tags = getUserTags;
            user.Skills = getUserSkills;
            //.Include(a=>a.Buyer).Include(a=>a.Seller)
            //GetReviews
            var Reviews = new UserBL().GetAllReviews(de).Where(a => a.CommentUser == id).ToList();
            foreach (var x in Reviews)
            {
                var Commenter = new UserBL().GetActiveUserById(x.CommentId, de);
                var Commente = new UserBL().GetActiveUserById(x.CommentUser, de);
                x.Buyer = Commente;
                x.Seller = Commenter;
            }
            Reviews = Reviews.TakeLast(5).ToList();
            ViewBag.Reviews = Reviews;
            if (user != null)
            {
                ViewBag.Status = user.Status;
                return View(user);
            }
            return RedirectToAction("NoResultFound", new { msg = "No Result Found" });
        }


        [HttpPost]
        public IActionResult Getorders(int userid = -1)
        {
            List<Order> orderlist=new List<Order>();
            if(userid!=-1)
            {
                orderlist = new UserBL().GetOrdersByUserId(userid,de);

            }
            

            List<ordersdto> tlist = new List<ordersdto>();

            string color = "blue";
            foreach (var item in orderlist)
            {
                var encorderid=StringCipher.EncryptId(item.Id);

                ordersdto obj = new ordersdto()
                {
                    Id = item.Id,
                    OrderTitle = item.OrderTitle,
                    StartDate = item.StartDate,
                    //need to add 1 day in end date, because it is exclusive and will always show 1 less day
                    EndDate = item.EndDate/*.Value.AddHours(1)*/,
                    PopoverString = @"<b>Details: </b>" + item.OrderDescription + "<br> <b>Start Date: </b>" + item.StartDate.Value.ToString("MM/dd/yyyy") + "<br> <b>End Date: </b>" + item.EndDate.Value.ToString("MM/dd/yyyy"),
                    color = color,
                    url = "../OrderDetails?id2=" + encorderid, // passing orderdetail as route path function
                    //url = "../Seller/OrderDetails?id2=" + item.Id,
                    NewEndDate = item.EndDate.Value.ToString("yyyy-MM-dd")
                };

                tlist.Add(obj);
            }

            return Json(tlist);
        }


        [HttpPost]
        public IActionResult GetRegionaltimezone(string date)
        {
          
            var utcstarttime=Convert.ToDateTime(date).ToUniversalTime();
            var utcendtime = utcstarttime.AddHours(1);

            List<string> standendtm = new List<string>();
            standendtm.Add(utcstarttime.ToString("MM/dd/yyyy hh:00:00 tt"));
            standendtm.Add(utcendtime.ToString("MM/dd/yyyy hh:00:00 tt"));
            //var mysystemtime = utctime.UTCToSystemTime();

            return Json(standendtm);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("CheckOut")]
        public IActionResult CheckOut(CheckOutDTO Data)
        {
            var subscriptions = new OrderBL().GetSubscriptionsAgainstSpecificButler(Data.customerId, Data.butlerId, de);
            Data.sessionCount = 0;
            foreach (var sub in subscriptions)
            {
                if (DateTime.UtcNow >= sub.SubscriptionEnd)
                {
                    sub.IsActive = 0;
                }
                if (sub.IsActive > 0)
                {
                    Data.sessionCount += sub.SessionCount;
                }
            }
            if (Data.title == "Subscription")
            {
                if (Data.subscribtionCharges == 100)
                {
                    Data.duration = "1 Year";
                }
                else
                {
                    Data.duration = "2 Years";
                }
                ViewBag.PayableAmount = Math.Ceiling(Data.subscribtionCharges * 100);
                Data.stripeFee = Math.Ceiling((Data.subscribtionCharges * 4) / 100);
                Data.platformFee = Math.Ceiling((Data.subscribtionCharges * 13) / 100);
                return View(Data);
            }

            //scale pay massam
            var butlerOrderRecords = new UserBL().GetAllOrders(de).Where(x => x.SellerId == Data.butlerId && x.IsDelivered == 2);
            var butlerOrderRecordsCount = butlerOrderRecords.Count();
            var butlerRating = 0.0;
            var butlerUserReccord = new UserBL().GetActiveUserById(Data.butlerId, de);
            if (butlerOrderRecordsCount != 0)
            {
                foreach (var butlerOrderRecord in butlerOrderRecords)
                {
                    //start massam 09-01-23
                    var value = new UserBL().GetReviewById(butlerOrderRecord.Id, de);
                    if (value != null)
                    {
                        butlerRating += value.Stars;
                    }
                    //End massam 09-01-23
                }
                butlerRating = butlerRating / butlerOrderRecordsCount;//Average
                if (butlerOrderRecordsCount >= 5 && butlerOrderRecordsCount < 10 && butlerRating >= 4.8)
                {
                    butlerUserReccord.StartingFrom = "30";
                }
                else if (butlerOrderRecordsCount >= 10 && butlerRating >= 4.8)
                {
                    butlerUserReccord.StartingFrom = "35";
                }
                var check = new UserBL().UpdateUser(butlerUserReccord, de);
            }


            //end scale pay massam



            var recReccord = new UserBL().GetActiveUserById(Data.butlerId, de);
            Data.hourlyRate = recReccord.StartingFrom;
            var workingHours = Math.Ceiling(TimeSpan.FromTicks(Data.ToDateTime.Subtract(Data.FromDateTime).Ticks).TotalHours);
            Data.wHours = workingHours.ToString();
            Data.workCharges = Math.Ceiling(float.Parse(recReccord.StartingFrom) * float.Parse(Data.wHours));
            ViewBag.PayableAmount = Math.Ceiling((float)Data.workCharges * 100);
            Data.stripeFee = Math.Ceiling((float)(Data.workCharges * 4) / 100);
            Data.platformFee = Math.Ceiling((float)(Data.workCharges * 13) / 100);
            Data.butlerServiceCharges = Math.Ceiling((float)Data.workCharges - (float)(Data.stripeFee + Data.platformFee));
            return View(Data);
        }

        [AllowAnonymous]
        [Route("NoResultFound")]
        public IActionResult NoResultFound(string srch="")
        {
            ViewBag.srch = srch;
            return View();
        }
        #endregion

        #region Register
        public IActionResult Register(string value = "", string rdx="")
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
                else if (value== "I.T. Butler" && loggedinUser.Role==4)
                {
                    //return RedirectToAction("Register", "Home", new{ value = "I.T. Butler" , rdx="" });
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.Category = value;
            ViewBag.rdx = rdx;

            return View();
        }
        #endregion

        #region Message
        //Get Message
        public async Task<IActionResult> Messages(string rec = "", string refx="",string msg="")
        {
            try
            {
                //var CreateAcc = new AjaxController(de,haccess).CreateAccount("test110@gmail.com");
                var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
                  
                var CurrentUserRecord = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);
               

                //In case of new msg with or without refferal
                if (rec != "")
                    {
                        var getUserdet = new UserBL().GetUserById(StringCipher.DecryptId(rec), de);
                        var chk = new ChatBL(confg).GetAllChats().Where(a => a.SenderId == Convert.ToInt32(Userid) && a.RecieverId == StringCipher.DecryptId(rec)).FirstOrDefault();
                        //If no message exsists for current user
                        if (chk == null)
                        {
                        Message m = new Message();
                        m.CreatedAt = DateTime.UtcNow;
                        m.SenderId = CurrentUserRecord.Id;
                        m.RecieverId = StringCipher.DecryptId(rec);
                        m.IsActive = 1;
                        
                         m.Message_Description = "Hi! I am interested in your service! Please reach me back asap. Thanks!";



                        var rt = new ChatBL(confg).AddChat(m);
                        getUserdet.ConnectionId = "1";
                        var updateTotal = await new UserBL().UpdateUser(getUserdet, de);

                        //new ChatHub(confg, haccess).SendMessage(CurrentUserRecord.FirstName + CurrentUserRecord.LastName, m.Message_Description, m.SenderId.ToString(), m.RecieverId.ToString());
                        if (rt == -1)
                        {
                            return RedirectToAction("Message", "Home", new { msg = "Something went wrong" });
                        }
                    }
                        
                        //if refferal number exsists and no recent message exsists 
                        if (refx != "" && refx != CurrentUserRecord.Refferal_Code && chk == null)
                        {
                            var getLoggedinUser = CurrentUserRecord;
                            var refferal = new Refferals
                            {
                                RefferalCode = refx,
                                RefferalType = 1,
                                RefferedId = getLoggedinUser.Id,
                                CreatedAt = DateTime.UtcNow,
                                IsActive = 1,
                            };
                            var addref = new UserBL().AddRefferal(refferal, de);
                        }
                    }
                    var msgcount = new ChatBL(confg).GetAllChats().Where(a => a.SenderId == Convert.ToInt32(Userid) || a.RecieverId == Convert.ToInt32(Userid)).ToList();
                    ViewBag.MsgCount = msgcount.Count();
                    ViewBag.SenderId = CurrentUserRecord.Id;

                    return View();
            }
            catch
            {
              
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        #region Feedback

        public IActionResult Contact(string msg="")
        {
            ViewBag.Message = msg;
            return View();
        }

        [HttpPost]
        public IActionResult PostFeedback(Feedback model)
        {
            
            if(model!=null)
            {
                model.Email = WebUtility.HtmlEncode(model.Email);
                model.Contact_No = WebUtility.HtmlEncode(model.Contact_No);
                model.Name = WebUtility.HtmlEncode(model.Name);
                model.Message = WebUtility.HtmlEncode(model.Message); 
                var addFeedback = new UserBL().AddFeedback(model, de);
                return RedirectToAction("Contact", new { msg = "Thanks for your Feedback ! We would look into it and contact you back as soon as possible." });
            }
            return RedirectToAction("Contact", new { msg = "Something Went Wrong ! Please Try Again !" });
        }
        #endregion


        #region Customer Request Help

        public IActionResult CustomerRequestHelp(string msg="",string color="")
        {
            //if(msg!= "Something went wrong" && color!="red")
            //{
            //    ViewBag.messg = msg;
            //    ViewBag.color = color;
            //}
            ViewBag.messg=msg;
            ViewBag.color=color;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult>PostCustomerRequestHelp(Requesthelp Customerrequest,string[] language=null, string[] multiskills = null, string[] multitags=null, List<string> preferservicetime = null,string requesttype=null,/*string skillcategory=null,*/string issuecategory=null)
        {
            int uid = Convert.ToInt32(Customerrequest.userid);
            
            var LoggedinUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            var LoggedinUserEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var langs = "";
            if (language.Length!=0)
            {
                //var Addlanguage = AddLanguage(language, LoggedinUserId);
                langs = string.Join(",", language).TrimStart(',');


            }
            
            if (multitags[1]!=null && multitags.Length > 1)
            {
                //var Addlanguage = AddLanguage(language, LoggedinUserId);
                //Tags = string.Join(",", multitags).TrimStart(',');
                multitags = multitags[1].Split(",");

            }

            var skills = string.Join(",", multiskills).TrimStart(',');

            if (Customerrequest!=null)
            {
                Requesthelp _reqhelp = new Requesthelp()
                {
                    Title = Customerrequest.Title,
                    budget = Customerrequest.budget,
                    City = Customerrequest.City,
                    FromDateTime = Customerrequest.FromDateTime,
                    ToDateTime = Customerrequest.ToDateTime,
                    //Tags = Tags,
                    Tags = null,
                    //skills = Customerrequest.skills,
                    skills = skills,
                    RequestType=requesttype,
                    Zipcodes = Customerrequest.Zipcodes,
                    Description = Customerrequest.Description,
                    Isactive = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    DeletedAt = null,
                    userid = Convert.ToInt32(LoggedinUserId),
                    Language = langs,
                    servicetime= Customerrequest.servicetime,
                    prefferservicetime= Customerrequest.prefferservicetime,
                    
                    //Requesteduser=null

                };
              
                int idd = Convert.ToInt32(LoggedinUserId);
                var LoggedinUserdata = new UserBL().GetActiveUserById(idd, de);
                

                //bool chkrequest = await new UserBL().AddRequest(_reqhelp, de);
                int reqid =  new UserBL().AddRequest(_reqhelp, de);
                _reqhelp.Id = reqid;//masam



                if (reqid!=0)
                {
                    int current = 1;
                    var addservicetime = Addpreferservicetime(preferservicetime, reqid);

                    var addrequestskills = Addrequestskills(multiskills,Customerrequest.budget,/*skillcategory,*/ issuecategory, reqid); //add request skills

                    var addrequesttags = AddrequestTags(multitags, reqid); //add request Tags


                    //matching butlers for send live notifications

                    var allrequestskills = new UserBL().Getallrequestskillbyid(reqid, de).Select(a=>a.Requested_SkillName).ToList();//get request skills
                   
                    var allrequestTags = new UserBL().GetallrequestTagsbyid(reqid, de).Select(a => a.Requested_TagName).ToList();//get request tags


                    //var matchingbutlerbyskill = new UserBL().GetButlersListbyskill(_reqhelp.Language, LoggedinUserdata.ZipCode, LoggedinUserdata.City, _reqhelp.skills,de,requesttype);
                    var matchingbutlerbyskill = new UserBL().GetButlersListbyskill(_reqhelp.Language, LoggedinUserdata.ZipCode, LoggedinUserdata.City, allrequestskills,reqid, de,requesttype);

                    //int customeremailcount = 0;

                    if (matchingbutlerbyskill.Count() != 0)
                    {
                        //List<ViewRequestDto> matchingrequestlist = new List<ViewRequestDto>();
                        foreach (var item in matchingbutlerbyskill)
                        { 
                            if(requesttype== "Livenow") // sending notifications against live now request
                            {

                                if (item.Availability == 1)
                                {
                                    if (_reqhelp.Description == null)
                                    {
                                        _reqhelp.Description = "N/A";
                                    }
                                    Notification _notif = new Notification()
                                    {

                                        IsActive = 1,
                                        IsRead = 0,
                                        CreatedAt = DateTime.Now.ToLongDateString(),
                                        UserId = item.Id,
                                        Senderid = Convert.ToInt32(LoggedinUserId),
                                        Title = _reqhelp.Title,
                                        Description = _reqhelp.Description,
                                        Requestid = reqid


                                    };

                                    var addnotification = new UserBL().Addnotification(_notif, de);

                                    if (addnotification.Result)
                                    {
                                        //if (item.Availability == 1)
                                        //{
                                        await HubContext.Clients.All.SendAsync("customerrequestnotification", item.Id.ToString());
                                        //masam

                                        var mailToButler = MailSender.CustomerRequestMailToButlers(item.Email);
                                        if (mailToButler)
                                        {
                                            _reqhelp.EmailToButlers += string.Join(",", item.Id).TrimStart(',');
                                            if (_reqhelp.EmailToCustomer == 0)
                                            {
                                                var mailToCustomer = MailSender.MatchingButlersMailToCustomer(LoggedinUserEmail);
                                                if (mailToCustomer)
                                                {
                                                    _reqhelp.EmailToCustomer = Int32.Parse(LoggedinUserId);
                                                }
                                                //customeremailcount++;
                                            }
                                        }
                                        new UserBL().UpdateRequestForEmailSend(_reqhelp, de);
                                        //}
                                    }

                                }


                            }
                            else if(requesttype== "Schedule")
                            {
                                if (_reqhelp.Description == null)
                                {
                                    _reqhelp.Description = "N/A";
                                }
                                Notification _notif = new Notification()
                                {

                                    IsActive = 1,
                                    IsRead = 0,
                                    CreatedAt = DateTime.Now.ToLongDateString(),
                                    UserId = item.Id,
                                    Senderid = Convert.ToInt32(LoggedinUserId),
                                    Title = _reqhelp.Title,
                                    Description = _reqhelp.Description,
                                    Requestid = reqid


                                };

                                var addnotification = new UserBL().Addnotification(_notif, de);

                                if (addnotification.Result)
                                {
                                    //if (item.Availability == 1)
                                    //{
                                    await HubContext.Clients.All.SendAsync("customerrequestnotification", item.Id.ToString());
                                    //masam

                                    var mailToButler = MailSender.CustomerRequestMailToButlers(item.Email);
                                    if (mailToButler)
                                    {
                                        _reqhelp.EmailToButlers += string.Join(",", item.Id).TrimStart(',');
                                        if (_reqhelp.EmailToCustomer == 0)
                                        {
                                            var mailToCustomer = MailSender.MatchingButlersMailToCustomer(LoggedinUserEmail);
                                            if (mailToCustomer)
                                            {
                                                _reqhelp.EmailToCustomer = Int32.Parse(LoggedinUserId);
                                            }
                                            //customeremailcount++;
                                        }
                                    }
                                    new UserBL().UpdateRequestForEmailSend(_reqhelp, de);
                                    //}
                                }


                            }
                            

                        }


                    }


                    else
                    {
                        //if (_reqhelp.Tags != "N/A")
                        if (allrequestTags.Count() != 0)
                        {
                            //var matchingbuttlerbytag = new UserBL().GetButlersListbyTag(LoggedinUserdata.ZipCode, LoggedinUserdata.City, _reqhelp.Tags, de);
                            var matchingbuttlerbytag = new UserBL().GetButlersListbyTag(LoggedinUserdata.ZipCode, LoggedinUserdata.City, allrequestTags, de,requesttype);

                            if (matchingbuttlerbytag.Count() != 0)
                            {
                                //List<ViewRequestDto> matchingrequestlist = new List<ViewRequestDto>();
                                foreach (var item in matchingbuttlerbytag)
                                {
                                    if (requesttype == "Livenow")
                                    {
                                        if (item.Availability == 1)
                                    {
                                        if (_reqhelp.Description == null)
                                        {
                                            _reqhelp.Description = "N/A";
                                        }
                                        Notification _notif = new Notification()
                                        {

                                            IsActive = 1,
                                            IsRead = 0,
                                            CreatedAt = DateTime.Now.ToLongDateString(),
                                            UserId = item.Id,
                                            Senderid = Convert.ToInt32(LoggedinUserId),
                                            Title = _reqhelp.Title,
                                            Description = _reqhelp.Description,
                                            Requestid = reqid


                                        };

                                        var addnotification = new UserBL().Addnotification(_notif, de);

                                        if (addnotification.Result)
                                        {
                                            //if (item.Availability == 1)
                                            //{
                                            await HubContext.Clients.All.SendAsync("customerrequestnotification", item.Id.ToString());

                                            //masam

                                            var mailToButler = MailSender.CustomerRequestMailToButlers(item.Email);
                                            if (mailToButler)
                                            {
                                                _reqhelp.EmailToButlers += string.Join(",", item.Id).TrimStart(',');
                                                if (_reqhelp.EmailToCustomer == 0)
                                                {
                                                    var mailToCustomer = MailSender.MatchingButlersMailToCustomer(LoggedinUserEmail);
                                                    if (mailToCustomer)
                                                    {
                                                        _reqhelp.EmailToCustomer = Int32.Parse(LoggedinUserId);
                                                    }
                                                    //customeremailcount++;
                                                }
                                            }
                                            new UserBL().UpdateRequestForEmailSend(_reqhelp, de);

                                            //}
                                        }
                                    }

                                }
                                    else if(requesttype == "Schedule")
                                    {

                                        if (_reqhelp.Description == null)
                                        {
                                            _reqhelp.Description = "N/A";
                                        }
                                        Notification _notif = new Notification()
                                        {

                                            IsActive = 1,
                                            IsRead = 0,
                                            CreatedAt = DateTime.Now.ToLongDateString(),
                                            UserId = item.Id,
                                            Senderid = Convert.ToInt32(LoggedinUserId),
                                            Title = _reqhelp.Title,
                                            Description = _reqhelp.Description,
                                            Requestid = reqid


                                        };

                                        var addnotification = new UserBL().Addnotification(_notif, de);

                                        if (addnotification.Result)
                                        {
                                            //if (item.Availability == 1)
                                            //{
                                            await HubContext.Clients.All.SendAsync("customerrequestnotification", item.Id.ToString());

                                            //masam

                                            var mailToButler = MailSender.CustomerRequestMailToButlers(item.Email);
                                            if (mailToButler)
                                            {
                                                _reqhelp.EmailToButlers += string.Join(",", item.Id).TrimStart(',');
                                                if (_reqhelp.EmailToCustomer == 0)
                                                {
                                                    var mailToCustomer = MailSender.MatchingButlersMailToCustomer(LoggedinUserEmail);
                                                    if (mailToCustomer)
                                                    {
                                                        _reqhelp.EmailToCustomer = Int32.Parse(LoggedinUserId);
                                                    }
                                                    //customeremailcount++;
                                                }
                                            }
                                            new UserBL().UpdateRequestForEmailSend(_reqhelp, de);

                                            //}
                                        }

                                    }

                                }

                                    

                                    

                            }




                            //List<ViewRequestDto> matchingrequestlist = new List<ViewRequestDto>();
                            //foreach (var item in matchingbuttlerbytag)
                            //{
                            //    if (item.Description == null)
                            //    {
                            //        item.Description = "Description not available";
                            //    }
                            //    else if (item.StartingFrom == null)
                            //    {
                            //        item.StartingFrom = "N/A";
                            //    }
                            //    ViewRequestDto obj = new ViewRequestDto()
                            //    {
                            //        UserId = StringCipher.EncryptId(item.Id),
                            //        FirstName = item.FirstName,
                            //        LastName = item.LastName,
                            //        City = item.City,
                            //        Country = item.Country,
                            //        Description = item.Description,
                            //        ImagePath = item.ImagePath,
                            //        StartingFrom = item.StartingFrom,
                            //        Stars = item.Stars.ToString(),
                            //    };

                            //    matchingrequestlist.Add(obj);
                            //}






                        }

                    }

                    //End matching butlers for send live notifications


                    return RedirectToAction("ViewRequestDetail", "Home", new { reqid, msg = "Your Request Submitted Successfully", color = "Green", current });

                    //return RedirectToAction("ViewRequestDetail", "Home", new { reqid, msg = "Your Request Submitted Successfully", color = "Green", current });

                }


            }



            return RedirectToAction("CustomerRequestHelp", "Home", new { msg = "Something went wrong please try again !!!", color = "red" });


        }

            public List<bool> Addrequestskills(string[] multiskills,string skillcategory,string issuecategory, int requestid)
        {
            //var skills = multiskills.Count();
            var LoggedinUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            if(multiskills.Length == 0)
            {
                multiskills[0] = skillcategory;
            }
            List<bool> listofskills = new List<bool>();

            for (int i = 0; i < multiskills.Length; i++)
            {
                Requestkills skill = new Requestkills()

                {

                    //Id = 1,
                    IsActive = 1,
                    CreatedAt = DateTime.Now,
                    Requestid = requestid,
                    Skillcategory=skillcategory,
                    Requested_SkillName = multiskills[i],
                    subcategoryissue=issuecategory



                };

                //skill.Requested_SkillName = skillcategory;
                var skillstatus =new UserBL().AddRequestSkills(skill, de);
                if(skillstatus)
                {
                    listofskills.Add(skillstatus);

                }
            }


            return listofskills;

        }

        
            public List<bool> AddrequestTags(string[] multitags,int requestid)
             {
            //var skills = multiskills.Count();
            var LoggedinUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            List<bool> listoftags = new List<bool>();
            var tagslength=multitags.Length;
            if (multitags[1] != null && tagslength != 0)
            {
                for (int i = 0; i < tagslength; i++)
                {
                    RequestTags tags = new RequestTags()

                    {

                        //Id = 1,
                        IsActive = 1,
                        CreatedAt = DateTime.Now,
                        Requestid = requestid,
                        Requested_TagName = multitags[i],


                    };

                    //skill.Requested_SkillName = skillcategory;
                    var tagstatus = new UserBL().AddRequestTags(tags, de);
                    if (tagstatus)
                    {
                        listoftags.Add(tagstatus);

                    }
                }
            }

           


            return listoftags;

        }

            public List<bool> Addpreferservicetime(List<string> servicetimes, int requestid)
        {
            var times = servicetimes.ToArray();
            List<bool> listservicetime = new List<bool>();
            for (int i = 0; i < times.Length; i++)
            {
                preferservicetime pref = new preferservicetime()

                {

                    //Id = 1,
                    IsActive = 1,
                    CreatedAt = DateTime.Now.ToLongDateString(),
                    requestid=requestid,
                    servicetime=times[i],
                   


                };

                var preftimestatus = new UserBL().AddRequestprefertime(pref, de);
                listservicetime.Add(preftimestatus);
            }


            return listservicetime;

        }


            public List<bool>  AddLanguage(List<string> languages, string userid="")
        {
            var langs = languages.ToArray();
            List<bool> langstatuses=new List<bool>();
            for(int i=0;i< langs.Length;i++)
            {
                UserLanguage lang = new UserLanguage()

                {

                    //Id = 1,
                    IsActive = 1,
                    CreatedAt = DateTime.Now.ToLongDateString(),
                    UserId =Convert.ToInt32(userid),
                    language = langs[i],



                };

                var langstatus = new UserBL().AddRequestlanguage(lang, de);
                langstatuses.Add(langstatus);
            }
       
            
                return langstatuses;
            
        }

            public IActionResult CustomerRequests(Nullable<int>requestid,string msg="",string color="",string userrole="")
        {
            var LoggedinUserRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            if(userrole==null || userrole=="")
            {
                userrole = LoggedinUserRole;
            }

            string LoggedinUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            ViewBag.userrole=userrole;
            ViewBag.message = msg;
            ViewBag.color = color;
            ViewBag.requestid=requestid;

            if (userrole=="4")
            {
                if (LoggedinUserId != null)
                {
                    int loggedinid = Convert.ToInt32(LoggedinUserId);
                    //var getallrequests = new UserBL().GetRequestsbyuserid(loggedinid, de);

                    
                    var getallrequests = new UserBL().GetRequestsbyuserid(loggedinid, de);
                   
                    

                    List<RequesthelpDto> Alluserrequests = new List<RequesthelpDto>();
                    List<string> skillnames = new List<string>();
                    foreach (var item in getallrequests)
                    {

                        //request skills // merge to main project ..
                        var getrequestskills = new UserBL().GetRequestskillsbyrequestid(item.Id, de).Select(a => a.Requested_SkillName).ToList();
                        skillnames.AddRange(getrequestskills);

                        var sknames = string.Join(",", skillnames).TrimStart();


                        // endrequest skills

                        RequesthelpDto reqobj = new RequesthelpDto()
                        {
                            Requestid = item.Id,
                            CreatedAt =item.CreatedAt ,
                            Title = item.Title,
                            Description = item.Description,
                            budget = item.budget,
                            FromDateTime = item.FromDateTime,
                            ToDateTime = item.ToDateTime,
                            //skills = item.skills,
                            skills = sknames,
                            Tags = item.Tags,
                            Zipcodes = item.Zipcodes,
                            City = item.City,

                        };


                        Alluserrequests.Add(reqobj);

                        skillnames.Clear();
                    }

                    var desclist = Alluserrequests.OrderByDescending(x => x.Requestid).ToList();
                    //    ViewBag.requests = Allrequests;
                    return View(desclist);
                }

            }

            //babar old code
            //else if (userrole == "3")
            //{ 
            //    int loggedinid = Convert.ToInt32(LoggedinUserId);
            //    User updateduser = new UserBL().GetActiveUserById(loggedinid,de);//for check user availability
            //   // var requestids = new UserBL().Getnotificationcount(loggedinid,de).Select(x=>x.UserId).ToList(); 

            //    var requestsbyuserskill = new UserBL().GetRequestsbyuserskills(loggedinid, de);

            //    var requestsbyusertag = new UserBL().GetRequestsbyuserTags(loggedinid, de);

            //    List<RequesthelpDto> Alluserrequests = new List<RequesthelpDto>();

            //    //var getallrequests = new UserBL().GetAllRequestss(de);
            //    if (requestsbyuserskill.Count()!=0)
            //    {

            //        foreach (var item in requestsbyuserskill)
            //        {
            //            if (updateduser.Availability == 1 /*&& item.CreatedAt > updateduser.UpdatedAt*/)
            //            {

            //                RequesthelpDto reqobj = new RequesthelpDto()
            //                {
            //                    Requestid = item.Id,
            //                    CreatedAt = item.CreatedAt,
            //                    Title = item.Title,
            //                    Description = item.Description,
            //                    budget = item.budget,
            //                    FromDateTime = item.FromDateTime,
            //                    ToDateTime = item.ToDateTime,
            //                    skills = item.skills,
            //                    Tags = item.Tags,
            //                    Zipcodes = item.Zipcodes,
            //                    City = item.City,
            //                    servicetime = item.servicetime,
            //                    userid = item.userid,

            //                };

            //                Alluserrequests.Add(reqobj);
            //            }

            //        }


            //    }

            //    if(requestsbyusertag.Count() !=0)
            //    {

            //            foreach (var item in requestsbyusertag)
            //            {
            //                RequesthelpDto reqobj = new RequesthelpDto()
            //                {
            //                    Requestid = item.Id,
            //                    CreatedAt = item.CreatedAt,
            //                    Title = item.Title,
            //                    Description = item.Description,
            //                    budget = item.budget,
            //                    FromDateTime = item.FromDateTime,
            //                    ToDateTime = item.ToDateTime,
            //                    skills = item.skills,
            //                    Tags = item.Tags,
            //                    Zipcodes = item.Zipcodes,
            //                    City = item.City,
            //                    servicetime=item.servicetime,
            //                    userid=item.userid,


            //                };


            //                Alluserrequests.Add(reqobj);

            //            }

            //    }
            //    Alluserrequests= Alluserrequests.OrderByDescending(a=>a.Requestid).DistinctBy(a=>a.Requestid).ToList();
            //    return View(Alluserrequests);

            //}
            //end babar old code

            //new code customerrequests based on notifications

            else if (userrole == "3")
            {
                int loggedinid = Convert.ToInt32(LoggedinUserId);
                //User updateduser = new UserBL().GetActiveUserById(loggedinid, de);//for check user availability
                var requestids = new UserBL().Getnotificationcount(loggedinid,de).Select(x=>x.Requestid).ToList(); 

                List<RequesthelpDto> Alluserrequests = new List<RequesthelpDto>();
                if(requestids.Count > 0)
                {
                    List<Requestkills> Requestskills = new List<Requestkills>();

                    List<string> skillnames = new List<string>();
                    List<string> tagnames = new List<string>();

                    foreach (var item in requestids)
                    {
                        var getrequests = new UserBL().GetRequestbyid(item.Value, de);
                        
                            
                        if (getrequests!=null)
                        {
                            
                            //getting request skills
                            var getrequestskills = new UserBL().GetRequestskillsbyrequestid(getrequests.Id, de).Select(x => x.Requested_SkillName).ToList();
                            skillnames.AddRange(getrequestskills);

                            var sknames = string.Join(",", skillnames).TrimStart();

                            //end get request skills


                            //get request tags

                            var getrequesttags= new UserBL().GetallrequestTagsbyid(getrequests.Id, de).Select(x => x.Requested_TagName).ToList();
                            tagnames.AddRange(getrequesttags);

                            var tgnames = "N/A";
                            if(tagnames.Count > 0)
                            {
                               tgnames = string.Join(",", tagnames).TrimStart();

                            }


                            //end request tags

                            RequesthelpDto reqobj = new RequesthelpDto()
                            {
                                Requestid = getrequests.Id,
                                CreatedAt = getrequests.CreatedAt,
                                Title = getrequests.Title,
                                Description = getrequests.Description,
                                budget = getrequests.budget,
                                FromDateTime = getrequests.FromDateTime,
                                ToDateTime = getrequests.ToDateTime,
                                //skills = getrequests.skills,
                                skills = sknames,
                                //Tags = getrequests.Tags,
                                Tags = tgnames,
                                Zipcodes = getrequests.Zipcodes,
                                City = getrequests.City,
                                servicetime = getrequests.servicetime,
                                userid = getrequests.userid,

                            };

                            Alluserrequests.Add(reqobj);

                            skillnames.Clear();
                        }

                        

                    }

                    Alluserrequests = Alluserrequests.OrderByDescending(a => a.Requestid).DistinctBy(a => a.Requestid).ToList();
                    return View(Alluserrequests);

                }

                

            }

            return View();
        }

            public IActionResult ViewRequestDetail(int reqid,string msg="",string color="",int currentPage=1)
        {
            if(reqid!=0)
            {
                var requestdetail = new UserBL().GetRequestbyid(reqid, de);
                

                if (requestdetail != null)

                {
                    if(requestdetail.Tags==null)
                    {
                        requestdetail.Tags = "N/A";
                    }
                    else if(requestdetail.Description==null)
                        
                    {
                        requestdetail.Description = "N/A";
                    }
                    // requested skills
                    var allrequestskills = new UserBL().Getallrequestskillbyid(reqid, de).Select(a=>a.Requested_SkillName).ToList();
                    //List<string> listofskills = new List<string>();


                    //end requested skills

                    //request tAGS
                    var allrequestags = new UserBL().GetallrequestTagsbyid(reqid, de).Select(a => a.Requested_TagName).ToList();


                    var skilljoin = string.Join(",", allrequestskills).TrimStart(',');
                    var Tagsjoin = string.Join(",", allrequestags).TrimStart(',');
                    
                    RequesthelpDto reqobj = new RequesthelpDto()
                    {
                        Requestid = requestdetail.Id,
                        CreatedAt = requestdetail.CreatedAt,
                        Title = requestdetail.Title,
                        Description = requestdetail.Description,
                        budget = requestdetail.budget,
                        FromDateTime = requestdetail.FromDateTime,
                        ToDateTime = requestdetail.ToDateTime,
                        skills = skilljoin,
                        //skills = requestdetail.skills,
                        Language=requestdetail.Language,

                        //Tags = requestdetail.Tags,
                        Tags = Tagsjoin,

                        Zipcodes = requestdetail.Zipcodes,
                        City = requestdetail.City,

                    };

                    var LoggedinUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
                    int idd = Convert.ToInt32(LoggedinUserId);
                    var LoggedinUserdata = new UserBL().GetActiveUserById(idd, de);

                    


                   // var matchingbutlerbyskill = new UserBL().GetButlersListbyskill(reqobj.Language,LoggedinUserdata.ZipCode,LoggedinUserdata.City,reqobj.skills, de);
                    var matchingbutlerbyskill = new UserBL().GetButlersListbyskill(reqobj.Language, LoggedinUserdata.ZipCode, LoggedinUserdata.City, allrequestskills, reqid, de,requestdetail.RequestType);

                    List<ViewRequestDto> matchingrequestlist = new List<ViewRequestDto>();

                    ViewBag.mssg = 0;

                    if (matchingbutlerbyskill.Count()!= 0)
                    {
                        //List<ViewRequestDto> matchingrequestlist=new List<ViewRequestDto>();
                        foreach(var item in matchingbutlerbyskill)
                        {
                            if(item.Description==null)
                            {
                                item.Description = "Description not available";
                            }
                            else if(item.StartingFrom==null)
                            {
                                item.StartingFrom = "N/A";
                            }
                            ViewRequestDto obj = new ViewRequestDto()
                            {
                                UserId=StringCipher.EncryptId(item.Id),
                                FirstName= item.FirstName,
                                LastName= item.LastName,
                                City=item.City,
                                Country=item.Country,
                                Description=item.Description,
                                ImagePath=item.ImagePath,
                                StartingFrom=item.StartingFrom,
                                Stars =item.Stars.ToString(),
                            };

                            matchingrequestlist.Add(obj);
                        }

                        



                        //var pagedlist = matchingrequestlist.ToPagedList(currentPage, 4);
                        //ViewBag.butl = pagedlist;
                        ViewBag.butlers= matchingrequestlist;
                    }

                    else
                    {
                        //if (reqobj.Tags != "N/A" || reqobj.Tags!="")
                        //{
                            //var matchingbuttlerbytag = new UserBL().GetButlersListbyTag(LoggedinUserdata.ZipCode, LoggedinUserdata.City, reqobj.Tags, de);
                            var matchingbuttlerbytag = new UserBL().GetButlersListbyTag(LoggedinUserdata.ZipCode, LoggedinUserdata.City,allrequestags, de,requestdetail.RequestType);

                            //List<ViewRequestDto> matchingrequestlist = new List<ViewRequestDto>();

                            if(matchingbuttlerbytag.Count()!=0)
                            {

                                foreach (var item in matchingbuttlerbytag)
                                {
                                    if (item.Description == null)
                                    {
                                        item.Description = "Description not available";
                                    }
                                    else if (item.StartingFrom == null)
                                    {
                                        item.StartingFrom = "N/A";
                                    }
                                    ViewRequestDto obj = new ViewRequestDto()
                                    {
                                        UserId = StringCipher.EncryptId(item.Id),
                                        FirstName = item.FirstName,
                                        LastName = item.LastName,
                                        City = item.City,
                                        Country = item.Country,
                                        Description = item.Description,
                                        ImagePath = item.ImagePath,
                                        StartingFrom = item.StartingFrom,
                                        Stars = item.Stars.ToString(),
                                    };

                                    matchingrequestlist.Add(obj);
                                }
                            }

                        else  // show other butler
                        {

                            var showotherbutler = new UserBL().GetButlersList(de).Where(x => (x.ZipCode.ToLower().Contains(LoggedinUserdata.ZipCode) || x.City.ToLower().Contains(LoggedinUserdata.City)) && x.Stars <= 5).OrderByDescending(x => x.Stars).ThenBy(a => a.Availability).ToList();
                            if (showotherbutler.Count != 0)
                            {
                                ViewBag.mssg = 1;
                                foreach (var item in showotherbutler)
                                {
                                    if (item.Description == null)
                                    {
                                        item.Description = "Description not available";
                                    }
                                    else if (item.StartingFrom == null)
                                    {
                                        item.StartingFrom = "N/A";
                                    }
                                    ViewRequestDto obj = new ViewRequestDto()
                                    {
                                        UserId = StringCipher.EncryptId(item.Id),
                                        FirstName = item.FirstName,
                                        LastName = item.LastName,
                                        City = item.City,
                                        Country = item.Country,
                                        Description = item.Description,
                                        ImagePath = item.ImagePath,
                                        StartingFrom = item.StartingFrom,
                                        Stars = item.Stars.ToString(),
                                    };

                                    matchingrequestlist.Add(obj);
                                }

                            }

                            else
                            {
                                ViewBag.mssg = 1;
                                List<User> showotherbutlerss = new List<User>();
                                showotherbutlerss = new UserBL().GetButlersList(de).Where(x => x.Stars <= 5).GroupBy(x => x.Availability == 1).SelectMany(a=>a).ToList();
                              if(showotherbutler.Count()==0)
                                {
                                    showotherbutlerss = new UserBL().GetButlersList(de).Where(x => x.Availability == 1).ToList();

                                }

                                foreach (var item in showotherbutlerss)
                                {
                                    if (item.Description == null)
                                    {
                                        item.Description = "Description not available";
                                    }
                                    else if (item.StartingFrom == null)
                                    {
                                        item.StartingFrom = "N/A";
                                    }
                                    ViewRequestDto obj = new ViewRequestDto()
                                    {
                                        UserId = StringCipher.EncryptId(item.Id),
                                        FirstName = item.FirstName,
                                        LastName = item.LastName,
                                        City = item.City,
                                        Country = item.Country,
                                        Description = item.Description,
                                        ImagePath = item.ImagePath,
                                        StartingFrom = item.StartingFrom,
                                        Stars = item.Stars.ToString(),
                                    };

                                    matchingrequestlist.Add(obj);
                                }
                            }


                        }



                        //var pagedlist = matchingrequestlist.ToPagedList(currentPage, 4);
                        //ViewBag.butl = pagedlist;
                        //ViewBag.mssg = 0;
                        //    if (matchingrequestlist.Count()==0)
                        //{
                        //    ViewBag.mssg = 1;
                        //}
                            ViewBag.butlers = matchingrequestlist;

                        //}

                    }



                    //var matchingbuttlerbytag = new UserBL().GetButlersListbyTag(reqobj.Tags, de);

                    //var matchingbutlerbyzipcode = new UserBL().GetButlersList(reqobj.Zipcodes,reqobj.City,de);

                    ViewBag.thankyou = msg;
                    ViewBag.colors = color;



                    return View(reqobj);
                }

            }
            
            
            return View();
        }

        public IActionResult DeleteRequest(int reqid)
        {
            var deleterequest =new UserBL().DeleteRequest(reqid,de);

            if (deleterequest.Result)
            {
                var deleterequestnotification = new UserBL().DeleteRequestNotification(reqid,de);

                return RedirectToAction("CustomerRequests", "Home", new { msg = "Your Request Deleted Successfully..!!", color = "green" });

            }
            return View();
        }

        public IActionResult Getuserrequestbyid(int reqid)
        {
            var Getrequest = new UserBL().Getrequestbyrequestid(reqid, de);

            if (Getrequest!=null)
            {
                
                return Json(Getrequest);

            }
            return Json(null);
        }

        public IActionResult UpdateRequest(Requesthelp updrequest, string[] language = null, /*string[] multiskills*/ List<string> multiskills = null, string[] multitags = null, List<string> preferservicetime = null)
        {
            var langs = "";
            if (language.Length != 0)
            {
                //var Addlanguage = AddLanguage(language, LoggedinUserId);
                langs = string.Join(",", language).TrimStart(',');

            }
            var Tags = "";
            if (multitags.Length != 0)
            {
                //var Addlanguage = AddLanguage(language, LoggedinUserId);
                Tags = string.Join(",", multitags).TrimStart(',');


            }

            //var skills = string.Join(",", multiskills).TrimStart(',');

            var updaterequest = new UserBL().updaterequest(updrequest, multiskills, Tags,langs, preferservicetime, de);

            if (updaterequest.Result)
            {
                //var addservicetime = Addpreferservicetime(preferservicetime,updrequest.Id);
                return RedirectToAction("CustomerRequests", "Home", new { msg = "Your Request Updated Successfully..!!", color = "Green" });

            }
            return View();
        }

        public IActionResult viewallnotifications(string msg = "", string color = "", string userrole = "")
        {
            var LoggedinUserRole = User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            if (userrole == null || userrole == "")
            {
                userrole = LoggedinUserRole;
            }

            string LoggedinUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            ViewBag.userrole = userrole;
            ViewBag.message = msg;
            ViewBag.color = color;

            var  Allnotifications = new UserBL().Getnotificationcount(Convert.ToInt32(LoggedinUserId), de).Where(x=>x.IsActive==1).ToList();



            return View(Allnotifications);
        }


        #endregion



        #region calendar

        //[HttpPost]
        //public IActionResult GetTaskData(string startdate = "", string enddate = "")
        //{

        //    List<Testorder> tasklist = new UserBL().GetActiveTaskList(de);

        //    List<ordersdto> tlist = new List<ordersdto>();

        //    string color = "blue";
        //    foreach (Testorder item in tasklist)
        //    {
        //        //if (item.Priority == 1)
        //        //{
        //        //    color = "red";
        //        //}
        //        //else if (item.Priority == 2)
        //        //{
        //        //    color = "blue";
        //        //}
        //        //else
        //        //{
        //        //    color = "green";
        //        //}

        //        ordersdto obj = new ordersdto()
        //        {
        //            Id = item.Id,
        //            OrderTitle = item.OrderTitle,
        //            StartDate = item.StartDate.Value.ToString("yyyy-MM-dd"),
        //            //need to add 1 day in end date, because it is exclusive and will always show 1 less day
        //            EndDate = (Convert.ToDateTime(item.EndDate).AddDays(1)).ToString("yyyy-MM-dd"),
        //            PopoverString = @"<b>Details: </b>" + item.OrderDescription + "<br> <b>Start Date: </b>" + item.StartDate.Value.ToString("MM/dd/yyyy") + "<br> <b>End Date: </b>" + item.EndDate.Value.ToString("MM/dd/yyyy"),
        //            color = color,
        //            url = "../Home/TaskDetail?_Id=" + item.Id,
        //            NewEndDate = item.EndDate.Value.ToString("yyyy-MM-dd")
        //        };

        //        tlist.Add(obj);
        //    }

        //    return Json(tlist);
        //}

        public IActionResult AddTask(Testorder _task, string std = "", string edt = "")
        {

            Testorder t = new Testorder()
            {
                OrderTitle = _task.OrderTitle,
                OrderDescription = _task.OrderDescription,
                StartDate = _task.StartDate,
                EndDate = Convert.ToDateTime(_task.EndDate).AddDays(-1),

                CreatedAt = DateTime.Now
            };

            bool chk = new UserBL().AddTask(t, de);

            if (chk == true)
            {
                return RedirectToAction("UserProfile", "Home", new { idx = "2" });
            }
            else
            {
                return RedirectToAction("UserProfile", "Home", new { idx = "2" });
            }
        }


        public ActionResult TaskDetail(int _Id = -1)
        {
            Testorder task = new UserBL().GetTaskbyId(_Id, de);

            ViewBag.Task = task;

            return View();
        }

        [HttpPost]
        public IActionResult TaskUpdateOnDrag(int Id = -1, string Startdate = "")
        {
            Testorder ord = new UserBL().GetTaskbyId(Id, de);

            DateTime d1 = Convert.ToDateTime(ord.StartDate);
            DateTime d2 = Convert.ToDateTime(ord.EndDate);
            TimeSpan tdiffer = (d2 - d1);

            int day = Convert.ToInt32(tdiffer.TotalDays);

            int msg = -1;

            Testorder t = new Testorder()
            {
                Id = ord.Id,
                OrderTitle = ord.OrderTitle,
                OrderDescription = ord.OrderDescription,
                StartDate = Convert.ToDateTime(Startdate),
                EndDate = Convert.ToDateTime(Startdate).AddDays(day),

                CreatedAt = ord.CreatedAt
            };

            bool chk = new UserBL().UpdateTask(t, de);

            if (chk == true)
            {
                msg = 1;
            }

            return new JsonResult(msg);
        }



        [HttpPost]
        public IActionResult TaskUpdateOnResize(int Id = -1, string Enddate = "")
        {
            Testorder ord = new UserBL().GetTaskbyId(Id, de);

            int msg = -1;

            Testorder t = new Testorder()
            {
                Id = ord.Id,
                OrderTitle = ord.OrderTitle,
                OrderDescription = ord.OrderDescription,
                StartDate = ord.StartDate,//Convert.ToDateTime(Startdate),
                EndDate = Convert.ToDateTime(Enddate).AddDays(-1), //need to reove 1 day because end date is always 1 more than selected

                CreatedAt = ord.CreatedAt
            };

            bool chk = new UserBL().UpdateTask(t, de);

            if (chk == true)
            {
                msg = 1;
            }

            return Json(msg);
        }


        public ActionResult DeleteTask(int Id = -1)
        {
            Testorder task = new UserBL().GetTaskbyId(Id, de);

            Testorder t = new Testorder()
            {
                Id = task.Id,
                OrderTitle = task.OrderTitle,
                OrderDescription = task.OrderDescription,
                StartDate = task.StartDate,
                EndDate = task.EndDate,

                CreatedAt = task.CreatedAt
            };

            bool chk = new UserBL().UpdateTask(t, de);

            if (chk == true)
            {
                return RedirectToAction("Index", "Home", new { msg = "Task Deleted successfully" });
            }
            else
            {
                return RedirectToAction("Index", "Home", new { msg = "Error" });
            }
        }




        #endregion calendar



    }
}
