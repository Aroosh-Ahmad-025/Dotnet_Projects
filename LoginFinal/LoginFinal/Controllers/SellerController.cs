using LoginFinal.BL;
using LoginFinal.DataHub;
using LoginFinal.Filters;
using LoginFinal.HelpingClasses;
using LoginFinal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using RestSharp;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static LoginFinal.HelpingClasses.ProjectVariables;

namespace LoginFinal.Controllers
{
    [Authorize]
    [ExceptionFilter]
    public class SellerController : Controller
    {

        private readonly AppDbContext db;
        private readonly SqlConnection de;
        private readonly IHttpContextAccessor haccess;
        private readonly IConfiguration confg;

        private readonly IHubContext<ChatHub> HubContext;

        public SellerController(SqlConnection de, IConfiguration confg, IHttpContextAccessor haccess,IHubContext<ChatHub> HubContext, AppDbContext db)
        {
            //this.de = new SqlConnection(confg.GetConnectionString("Default"));
            this.de = de;
            this.confg = confg;
            var request = haccess.HttpContext.Request;
            this.haccess = haccess;
            baseUrl = $"{request.Scheme}://{request.Host}";
            this.HubContext = HubContext;
            this.db = db;
        }


        #region Manage Seller

        [Route("Account")]
       
        public IActionResult Account(string msg = "",int verify_id=-1)
        {
            try
            {


                //get logged in user details
                var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
                ViewBag.userid = Userid;

                var GetUser = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);

                //check role
                if (GetUser.Role == 3 || GetUser.Role == 4)
                {

                    //get user details
                    //var GetSkills = new UserBL().GetAllSkills(de).Where(a => a.UserId == Convert.ToInt32(Userid) && a.IsActive == 1).FirstOrDefault();
                    //var GetSkills = new UserBL().GetAllSkills(de).Where(a => a.UserId == Convert.ToInt32(Userid) && a.IsActive == 1).ToList();//massam (14-12-22)
                    var GetSkills = new UserBL().GetAllSkills(de).Where(a => a.UserId == Convert.ToInt32(Userid) && a.IsActive == 1).Select(a => a.SkillName.ToUpper()).ToList();//massam (14-12-22)


                    //var GetTags = new UserBL().GetAllTags(de).Where(a => a.UserId == Convert.ToInt32(Userid) && a.IsActive == 1).FirstOrDefault();

                    var GetTags = new UserBL().GetAllTags(de).Where(a => a.UserId == Convert.ToInt32(Userid) && a.IsActive == 1).ToList();
                    var GetEducation = new UserBL().GetAllEducations(de).Where(a => a.UserId == Convert.ToInt32(Userid) && a.IsActive == 1).ToList();
                    var getuserexperience = new UserBL().Getexperiencebyuserid(Convert.ToInt32(Userid), de);
                    //var check_connectivity = new GeneralPurpose(de, haccess).CheckInternet();
                    var Getbutlerprefertime = new UserBL().Getbutlerprefertime(Convert.ToInt32(Userid) , de);
                    ViewBag.butlerprefertime = Getbutlerprefertime;

                    var userlanguage = new UserBL().Getuserlanguagebyuserid(Convert.ToInt32(Userid), de).Select(a => a.language).ToList();
                    ViewBag.userlanguages = null;

                    if (userlanguage.Count > 0)
                    {
                        var languageString = string.Join(",", userlanguage);
                        ViewBag.userlanguages = languageString;

                    }


                    ViewBag.userexperience = getuserexperience;
                    if (GetUser != null)
                    {
                       
                        if (msg== "Account Verification Success" && verify_id == GetUser.Id)
                        {
                            GetUser.Is_Verified = 1;
                            var updateUser = new UserBL().UpdateUser(GetUser, de);
                        }

                        if (GetUser.Role == 3 || GetUser.Role == 4)
                        {
                            //massam (14-12-22)
                            //var skills = "";
                            //for (int i = 0; i < GetSkills.Count; i++)
                            //{
                            //    skills += string.Join("", GetSkills[i].SkillName);
                            //    if (i >= 0 && i < GetSkills.Count - 1)
                            //    {
                            //        skills += ",";
                            //    }
                            //}

                            var tags = "";
                            for (int i = 0; i < GetTags.Count; i++)
                            {
                                tags += string.Join("", GetTags[i].TagName);
                                if (i >= 0 && i < GetTags.Count - 1)
                                {
                                    tags += ",";
                                }
                            }

                            //storing all records to pass in view
                            ViewBag.User = GetUser;
                            ViewBag.Skills = String.Join(",", GetSkills); //mssm 7jan23
                            //ViewBag.Tags = GetTags;
                            ViewBag.Tags = tags;
                            ViewBag.Education = GetEducation;
                            ViewBag.msg = msg;
                          

                            return View();
                        }

                    }

                    return RedirectToAction("Index", "Home");
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


        public IActionResult Earnings()
        {

            //get logged in user details
            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            var GetUser = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);

            var GetAllOrders = new UserBL().GetOrdersByUserId(GetUser.Id, de).Where(a=>a.IsDelivered==2).ToList();

            long balance_available = 0, balance_pending = 0;

            if (GetUser != null && GetUser.Role==3 && GetUser.StripeId!=null)
            {
                var requestOptions = new RequestOptions();

                requestOptions.StripeAccount = GetUser.StripeId;

                var service = new BalanceService();

                Balance balance = service.Get(requestOptions);
                
                var pendinglist = balance.Pending;
                var balancelist = balance.Available;

                foreach (var x in balancelist)
                {
                    balance_available = balance_available + x.Amount;
                }
                foreach (var x in pendinglist)
                {
                    balance_pending = balance_pending + x.Amount;
                }
                balance_pending = balance_pending / 100;
                balance_available = balance_available / 100;
            }
            ViewBag.Balance_Pending = balance_pending.ToString();
            ViewBag.Balance_Available = balance_available.ToString();

            ViewBag.UserId = GetUser.Id;

            ViewBag.Orders = GetAllOrders;
            return View();
        }

        public async Task<IActionResult> PostUpdatePassword(string oldPassword = "", string newPassword = "", string confirmPassword = "")
        {

            var Userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            var GetUser = new UserBL().GetActiveUserById(Convert.ToInt32(Userid), de);

            if (newPassword != confirmPassword)
            {
                return RedirectToAction("UpdatePasswordUser", "Auth", new { msg = "New password and Confirm password did not match!", color = "red" });
            }

            User u = GetUser;

            if (StringCipher.Decrypt(u.Password) != oldPassword)
            {
                return RedirectToAction("UpdatePasswordUser", "Auth", new { msg = "Old password did not match the current password!", color = "red" });
            }

            u.Password = StringCipher.Encrypt(newPassword);

            bool chk = await new UserBL().UpdateUser(u, de);

            if (chk)
            {
                return RedirectToAction("UpdatePasswordUser", "Auth", new { msg = "Password updated successfully!", color = "green" });
            }
            else
            {
                return RedirectToAction("UpdatePasswordUser", "Auth", new { msg = "Somthings' wrong!", color = "red" });
            }
        }

        #endregion


        #region Orders

        [Route("Orders")]
        public IActionResult Orders()
         {
            var LoggedinUserId2= User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            if (LoggedinUserId2 != null)
            {
                var LoggedinUserId = Convert.ToInt32(LoggedinUserId2);
                var getAllOrders = new UserBL().GetOrdersByUserId(LoggedinUserId, de);
                ViewBag.Orders = getAllOrders;
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        //[Route("OrderDetails")]
        //public IActionResult OrderDetails(string id2="")
        //{
        //    var id = StringCipher.DecryptId(id2);
        //    var LoggedinUserId2 = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        //    if (LoggedinUserId2 != null)
        //    {
        //        if (id != -1)
        //        {
        //            var getorder = new UserBL().GetOrderById(id,de);
        //            ViewBag.OrderDetails = getorder;
        //            // start massam (28-12-22)
        //            var getcustomerreview = new InternalReview();
        //            getcustomerreview = new UserBL().getInternalReviewByUserIds(getorder.BuyerId, Convert.ToInt32(LoggedinUserId2), de);
        //            ViewBag.getCustomerCurrentReview = new UserBL().getInternalReviewByOrderId(id, de);
        //            // end massam (28-12-22)
        //            ViewBag.internalreview = getcustomerreview.Sellerreview;

        //            }
        //            else
        //            {
        //                ViewBag.internalreview = null;
        //            }



        //            var getmessages = new ChatBL(confg).GetAllChats().Where(a => a.SenderId == Convert.ToInt32(LoggedinUserId2) || a.RecieverId == Convert.ToInt32(LoggedinUserId2)).Where(p=>p.OrderId==id).ToList();
        //            var getsortedmsg = getmessages.OrderBy(a => a.CreatedAt).ToList();
        //            ViewBag.Messages = getsortedmsg;

        //            if(Convert.ToInt32(LoggedinUserId2)== getorder.BuyerId)
        //            {
        //                ViewBag.SenderId = getorder.BuyerId;
        //                ViewBag.RecieverId = getorder.SellerId;
        //            }
        //            else
        //            {
        //                ViewBag.SenderId = getorder.SellerId;
        //                ViewBag.RecieverId = getorder.BuyerId;
        //            }
        //            if (getorder.EndDate.Value.AddDays(3).ToLongDateString()== DateTime.UtcNow.Date.ToLongDateString())
        //            {
        //                getorder.IsDelivered = 2;
        //                var update = new UserBL().UpdateOrder(getorder, de);
        //            }
        //            var seller_Review= new UserBL().GetAllReviews(de).Where(a=>a.CommentId==getorder.SellerId && a.OrdId== getorder.Id).ToList();
        //            ViewBag.SellerReview= seller_Review;
        //            ViewBag.ReviewCount = seller_Review.Count();
        //            return View();
        //        }
        //    }
        //    return RedirectToAction("Order","Seller");
        //}


        [Route("OrderDetails")]
        public IActionResult OrderDetails(string id2 = "")
        {
            var id = StringCipher.DecryptId(id2);
            var LoggedinUserId2 = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
            if (LoggedinUserId2 != null)
            {
                if (id != -1)
                {
                    var getorder = new UserBL().GetOrderById(id, de);
                    ViewBag.OrderDetails = getorder;
                    // start massam (28-12-22)
                    var getcustomerreview = new InternalReview();
                    getcustomerreview = new UserBL().getInternalReviewByUserIds(getorder.BuyerId, Convert.ToInt32(LoggedinUserId2), de);
                    ViewBag.getCustomerCurrentReview = new UserBL().getInternalReviewByOrderId(id, de);
                    // end massam (28-12-22)
                    if (getcustomerreview != null)
                    {
                        ViewBag.internalreview = getcustomerreview;
                    }
                    else
                    {
                        ViewBag.internalreview = null;
                    }
                    //ViewBag.CurrentOrderInternalReview
                    var getmessages = new ChatBL(confg).GetAllChats().Where(a => a.SenderId == Convert.ToInt32(LoggedinUserId2) || a.RecieverId == Convert.ToInt32(LoggedinUserId2)).Where(p => p.OrderId == id).ToList();
                    var getsortedmsg = getmessages.OrderBy(a => a.CreatedAt).ToList();
                    ViewBag.Messages = getsortedmsg;
                    if (Convert.ToInt32(LoggedinUserId2) == getorder.BuyerId)
                    {
                        ViewBag.SenderId = getorder.BuyerId;
                        ViewBag.RecieverId = getorder.SellerId;
                    }
                    else
                    {
                        ViewBag.SenderId = getorder.SellerId;
                        ViewBag.RecieverId = getorder.BuyerId;
                    }
                    if (getorder.EndDate.Value.AddDays(3).ToLongDateString() == DateTime.UtcNow.Date.ToLongDateString())
                    {
                        getorder.IsDelivered = 2;
                        var update = new UserBL().UpdateOrder(getorder, de);
                    }
                    var seller_Review = new UserBL().GetAllReviews(de).Where(a => a.CommentId == getorder.SellerId && a.OrdId == getorder.Id).ToList();
                    ViewBag.SellerReview = seller_Review;
                    ViewBag.ReviewCount = seller_Review.Count();
                    return View();
                }
            }
            return RedirectToAction("Order", "Seller");
        }


        //[HttpPost]
        //public async Task<IActionResult> Charge(string stripeEmail, string stripeToken,string stripeDescription, int buyer=-1, int seller=-1, int order=-1, int stripeAmount=-1)
        //{

        //    var ajax = new AjaxController(confg, haccess,HubContext);

        //    var ChargePayment = ajax.Charge(stripeEmail, stripeToken,stripeAmount, stripeDescription,order);

        //    if (ChargePayment==true)
        //    {
        //        var PlaceOrder = ajax.PlaceOrder(buyer, seller, order);

        //        if (PlaceOrder == true)
        //        {
        //            await HubContext.Clients.All.SendAsync("PlaceOrder", buyer.ToString(), seller.ToString(), order.ToString());

        //            return RedirectToAction("Orders", "Seller", new { msg = "Order Placed Successfully" });
        //        }
        //        else
        //            return RedirectToAction("Messages", "Home", new { msg = "Something Went Wrong ! Please Try Again! " });
        //    }
        //    else
        //    {
        //        return RedirectToAction("Messages","Home", new { msg = "Something Went Wrong ! Please Try Again! " });
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> Charge(string stripeEmail, string stripeToken, CheckOutDTO Data)
        {
            string msgtxt = "";
            string imgt = "";
            string msgid = "";
            string filen = "d_order";
            if (Data.orderId == null)
            {
                // massam 7jan23
                var butlerRec = new UserBL().GetUserById(Data.butlerId, de);
                //end massam 7jan23

                var NewOrder = new Order()
                {
                    OrderTitle = Data.title,
                    OrderDescription = Data.desc,
                    StartDate = Data.FromDateTime,
                    EndDate = Data.ToDateTime,
                    BuyerId = Data.customerId,
                    SellerId = Data.butlerId,
                    IsAccepted = 1,
                    IsActive = 1,
                    CreatedAt = DateTime.UtcNow,
                    OrderPrice = 0
                };
                if (stripeEmail == null && stripeToken == null)
                {
                    NewOrder.requestedSessions = Int32.Parse(Data.wHours);
                    var subscriptions = new OrderBL().GetSubscriptionsAgainstSpecificButler(Data.customerId, Data.butlerId, de);
                    var totalSessionCount = subscriptions.Sum(a => a.SessionCount);
                    var subscriptionChargeIds = new List<string>();
                    var sessionCharged = false;
                    if (totalSessionCount > 0 && totalSessionCount > Int32.Parse(Data.wHours))
                    {
                        foreach (var sub in subscriptions)
                        {
                            if (Data.wHours == "0")
                            {
                                break;
                            }
                            if (sub.SubscriptionEnd > DateTime.UtcNow)
                            {
                                if (sub.SessionCount > 0)
                                {
                                    if (Math.Ceiling(float.Parse(Data.wHours)) > sub.SessionCount)
                                    {
                                        sub.subtractedSessions = sub.SessionCount;
                                        Data.wHours = Math.Ceiling(float.Parse(Data.wHours) - sub.SessionCount).ToString();
                                        NewOrder.OrderPrice = NewOrder.OrderPrice + (13 * sub.SessionCount);
                                        sub.SessionCount = 0;
                                    }
                                    else
                                    {
                                        sub.subtractedSessions = sub.SessionCount;
                                        sub.SessionCount = (int)Math.Ceiling(sub.SessionCount - float.Parse(Data.wHours));
                                        sub.subtractedSessions = sub.subtractedSessions - sub.SessionCount;
                                        NewOrder.OrderPrice = NewOrder.OrderPrice + (13 * sub.subtractedSessions);
                                        Data.wHours = "0";
                                    }
                                    subscriptionChargeIds.Add(sub.Id.ToString());
                                    sessionCharged = new OrderBL().UpdateSubscription(sub, de);
                                }
                            }
                        }
                    }
                    else
                    {
                        return RedirectToAction("CheckOut", "Home", new { msg = "Insufficient Sessions! " });
                    }
                    if (sessionCharged == true)
                    {
                        NewOrder.Charge = string.Join(",", subscriptionChargeIds);
                        NewOrder.paidBy = 2;
                        Data.orderId = new OrderBL().AddOrder(NewOrder, de);

                        await HubContext.Clients.All.SendAsync("newMessage", NewOrder.BuyerId.ToString(), NewOrder.BuyerId.ToString(),msgtxt,NewOrder.SellerId.ToString(),imgt,msgid,Data.orderId.Value.ToString(),filen);
                        // massam 7jan23
                        MailSender.OrderPlacing(butlerRec.Email);
                        //end massam 7jan23
                        return RedirectToAction("Orders", "Seller", new { msg = "Order Placed Successfully" });
                    }
                    else
                    {
                        return RedirectToAction("Orders", "Seller", new { msg = "Something Went Wrong ! Please Try Again! " });
                    }
                }
                else
                {
                    NewOrder.OrderPrice = (long)Data.workCharges;
                    NewOrder.paidBy = 1;
                    Data.orderId = new OrderBL().AddOrder(NewOrder, de);
                }
                var ajax = new AjaxController(de, confg, haccess, HubContext);
                var ChargePayment = ajax.Charge(stripeEmail, stripeToken, (long)Data.workCharges, Data.desc, Data.orderId.Value.ToString());
                if (ChargePayment == true)
                {
                    //await HubContext.Clients.All.SendAsync("newMessage", NewOrder.BuyerId.ToString(), NewOrder.BuyerId.ToString(), "", NewOrder.SellerId.ToString(), "", "", Data.orderId.ToString(), "");
                    await HubContext.Clients.All.SendAsync("newMessage", NewOrder.BuyerId.ToString(), NewOrder.BuyerId.ToString(), msgtxt, NewOrder.SellerId.ToString(), imgt, msgid, Data.orderId.Value.ToString(), filen);

                    // massam 7jan23
                    MailSender.OrderPlacing(butlerRec.Email);
                    //end massam 7jan23
                    return RedirectToAction("Orders", "Seller", new { msg = "Order Placed Successfully" });
                }
                else
                {

                    return RedirectToAction("Messages", "Home", new { msg = "Something Went Wrong ! Please Try Again! " });
                }
            }
            else
            {
                // massam 7jan23
                var oredrRec = new OrderBL().GetOrderById((int)Data.orderId, de);
                var butlerRec = new UserBL().GetUserById(oredrRec.SellerId, de);
                oredrRec.IsAccepted = 1;
                new OrderBL().UpdateOrder(oredrRec, de);
                //end massam 7jan23

                var ajax = new AjaxController(de, confg, haccess, HubContext);
                var ChargePayment = ajax.Charge(stripeEmail, stripeToken, (long)Data.workCharges, Data.desc, Data.orderId.Value.ToString());
                if (ChargePayment == true)
                {
                    var PlaceOrder = ajax.PlaceOrder(Data.customerId, Data.butlerId, Data.orderId.Value);
                    if (PlaceOrder == true)
                    {
                        await HubContext.Clients.All.SendAsync("PlaceOrder", Data.customerId.ToString(), Data.butlerId.ToString(), Data.orderId.Value.ToString());

                        // massam 7jan23
                        MailSender.OrderPlacing(butlerRec.Email);

                        //end massam 7jan23

                        return RedirectToAction("Orders", "Seller", new { msg = "Order Placed Successfully" });
                    }
                    else
                        return RedirectToAction("Messages", "Home", new { msg = "Something Went Wrong ! Please Try Again! " });
                }
                return RedirectToAction("Messages", "Home", new { msg = "Something Went Wrong ! Please Try Again! " });
            }
        }

        public async Task<IActionResult> SubscriptonCharge(string stripeEmail, string stripeToken, CheckOutDTO Data)
        {
            var NewSubscription = new Models.Subscription()
            {
                BuyerId = Data.customerId,
                SellerId = Data.butlerId,
                IsSubscribed = 1,
                SubscriptionStart = DateTime.UtcNow,
                IsActive = 1,
                CreatedAt = DateTime.UtcNow,
                SubscriptionPrice = (long)Data.subscribtionCharges
            };
            if (Data.subscribtionCharges == 100)
            {
                NewSubscription.SessionCount = 6;
                NewSubscription.subscriptionType = 1;
                NewSubscription.SubscriptionEnd = DateTime.UtcNow.AddYears(1);
            }
            else
            {
                NewSubscription.SessionCount = 12;
                NewSubscription.subscriptionType = 2;
                NewSubscription.SubscriptionEnd = DateTime.UtcNow.AddYears(2);
            }
            //Data.orderId = new OrderBL().AddOrder(NewOrder, de);
            Data.subscriptionId = new OrderBL().AddSubscription(NewSubscription, de);
            var ajax = new AjaxController(de, confg, haccess, HubContext);
            //var ChargePayment = ajax.Charge(stripeEmail, stripeToken, (long)Data.workCharges, Data.desc, Data.orderId);
            var ChargePayment = ajax.SubscriptionCharge(stripeEmail, stripeToken, (long)Data.subscribtionCharges, Data.desc, Data.subscriptionId);
            if (ChargePayment == true)
            {
                return RedirectToAction("UserProfile", "Home", new { msg = "Subscribed Successfully", idx = StringCipher.EncryptId(Data.butlerId) });
            }
            else
            {
                return RedirectToAction("Messages", "Home", new { msg = "Something Went Wrong ! Please Try Again! " });
            }
        }


        public IActionResult DeleteUser(string id2="")
        {
            var id = StringCipher.DecryptId(id2);
            if(id!=-1)
            {
                var delete = new UserBL().DeleteUser(id, de);
                if (delete.Result == true)
                {
                    var signOut= new AuthController(de, confg,haccess,HubContext).SignOut();
                    return RedirectToAction("Index", "Home");
                }
                
            }
            return RedirectToAction("UpdatePasswordUser", "Auth", new {id=id, msg = "Something Went Wrong ! Please Try Again" });
        }

        #endregion

    }
}
