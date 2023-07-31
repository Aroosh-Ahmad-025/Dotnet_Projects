using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Hangfire;
using LoginFinal.BL;
using LoginFinal.HelpingClasses;
using LoginFinal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LoginFinal.DataHub
{

    public class ChatHub : Hub
    {

        private readonly IConfiguration confg;
        private readonly SqlConnection de;

        private readonly GeneralPurpose gp;


        public ChatHub(IConfiguration confg, IHttpContextAccessor haccess)
        {
            this.de = new SqlConnection(confg.GetConnectionString("Default"));
            this.gp = new GeneralPurpose(de, haccess);
            this.confg = confg;


        }

        
        public async Task SendMessage(string username, string message2, string sndid, string recid, string ordid = "")
        {
            Message message = new Message();
            var filen = "";
            message.CreatedAt = DateTime.UtcNow;
            if(message2.Contains("file:::"))
            {
                message.FilePath = message2.Split("file:::")[1];
                message2=message2.Split("file:::")[0];
            }
            if (message.FilePath != null)
            {
                filen = message.FilePath.ToString();
            }
            message.Message_Description = message2.TrimEnd();
            message.SenderId = Convert.ToInt32(sndid);
            message.RecieverId = Convert.ToInt32(recid);
            //Massam(8-12-22)
            var userRec = new UserBL().GetActiveUserById(message.RecieverId, de);
            if (ordid == "")
            {
                userRec.ConnectionId = (Int32.Parse(userRec.ConnectionId) + 1).ToString();
                await new UserBL().UpdateUser(userRec, de);
            }
            else
            {
                var getOrder = new UserBL().GetOrderById(Int32.Parse(ordid), de);
                if (userRec.Id == getOrder.Buyer.Id)
                {
                    getOrder.BuyerCommentsCount += 1;
                }
                else
                {
                    getOrder.SellerCommentsCount += 1;
                }
                new UserBL().UpdateOrder(getOrder, de);
            }
            //End









            if (ordid != "")
            {
                message.OrderId = Convert.ToInt32(ordid);
            }
            var imgt = new UserBL().GetActiveUserById(message.SenderId, de).ImagePath;
            message.IsActive = 1;
           
            var msg_id = new ChatBL(confg).AddChat(message);
            //await Clients.User(recid).SendAsync(message2);
            
            if(message2.Contains("zoom.us"))
            {
                await PlcOrder(message.SenderId.ToString());

            }
            if (message2.Contains("box shadow-sm mb-3 rounded bg-white") && message2.Contains("Reject") && message2.Contains("Cancellation"))
            {
                //var getOrder = new UserBL().GetOrderById(message.OrderId, de);
                //BackgroundJob.Schedule(
                //    () => CancelSchedule(message.OrderId),
                //    TimeSpan.FromDays(2));
            }
            await Clients.All.SendAsync("newMessage", username,sndid, message2, recid, imgt, msg_id, ordid,filen);
          
        }

        public async Task<bool> CancelSchedule(int id)
        {
            try
            { 
                var getOrder = new UserBL().GetOrderById(id, de);
                
                    getOrder.IsAccepted = -1;
                    getOrder.Cancellation_Reason = "Did Not Respond to Cancellation Request";
                    var update = new UserBL().UpdateOrder(getOrder, de);
                    
                    return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }
        public async Task RequestMeeting(string mg, string lginid)
        {
            var usern = new UserBL().GetActiveUserById(Convert.ToInt32(mg), de);
            var recname = usern.FirstName + " " + usern.LastName;
            await Clients.All.SendAsync("meetingReq", mg, recname, lginid);
        }
        public async Task OrderRequest(string recieverid, string lginid, string ordertitle = "", string orderdes = "", string fromdatetime = "", string todatetime = "")
        {

            var usern = new UserBL().GetActiveUserById(Convert.ToInt32(lginid), de);
            var buyid = 0;
            var sellid = 0;
            //massam 28dec
            var fd = Convert.ToDateTime(fromdatetime);
            var td = Convert.ToDateTime(todatetime);
            
            var workingHours = 0.0;
            var wHours = "";
            var workCharges = 0.0;
            var orderCharges = 0.0;


            if (usern.Role == 4)
            {
                var butler = new UserBL().GetActiveUserById(Convert.ToInt32(recieverid), de);
                workingHours = Math.Ceiling(TimeSpan.FromTicks(td.Subtract(fd).Ticks).TotalHours);
                wHours = workingHours.ToString();
                workCharges = Math.Ceiling(float.Parse(butler.StartingFrom) * float.Parse(wHours));
                orderCharges = Math.Ceiling((float)workCharges);


                buyid = usern.Id;
                sellid = Convert.ToInt32(recieverid);
            }
            else
            {
                workingHours = Math.Ceiling(TimeSpan.FromTicks(td.Subtract(fd).Ticks).TotalHours);
                wHours = workingHours.ToString();
                workCharges = Math.Ceiling(float.Parse(usern.StartingFrom) * float.Parse(wHours));
                orderCharges = Math.Ceiling((float)workCharges);

                buyid = Convert.ToInt32(recieverid);
                sellid = usern.Id;
            }
            //var delivery2 = delivery.Split(" ")[0];
            var NewOrder = new Order()
            {
                OrderTitle = ordertitle,
                OrderDescription = orderdes,
                StartDate =Convert.ToDateTime(fromdatetime),
                EndDate = Convert.ToDateTime(todatetime),
                BuyerId = buyid,
                SellerId = sellid,
                IsAccepted = 0,
                IsActive = 1,
                CreatedAt = DateTime.UtcNow,
                OrderPrice = (long)orderCharges,

            };
            var newOrder = new UserBL().AddOrder(NewOrder, de);
            if (newOrder != 0)
            {



                await Clients.All.SendAsync("OrderReq", recieverid.ToString(), lginid.ToString(), ordertitle.ToString(), orderdes.ToString(), fromdatetime.ToString(), todatetime.ToString(), orderCharges.ToString(), newOrder.ToString());
            }
        }

        public async Task PlcOrder(string seller)
        {
            await Clients.All.SendAsync("OrderPlace", seller.ToString());
        }

        //public async Task GetRequestNotifcation(int Id)
        //{
        //    await Clients.All.SendAsync("customerrequestnotification", Id);
        //}


    }
}