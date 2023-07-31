using Dapper;
using LoginFinal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LoginFinal.HelpingClasses;
using System.Security.Claims;
using Nancy.Extensions;
using LoginFinal.BL;

namespace LoginFinal.DAL
{
    public class UserDAL
    {

        //start massam 07-01-23
        public List<OrderReview> GetOrderReviewByUserId(int UserId, SqlConnection de)
        {
            // return de.OrderReviews.Where(a => a.OrdId == id && a.IsActive != 0).FirstOrDefault();
            return de.Query<OrderReview>("EXECUTE GetAllRecords OrderReviews,CommentUser," + UserId + "").ToList();
        }

        public List<OrderReview> GetInternalReviewByUserId(int UserId, SqlConnection de)
        {
            // return de.OrderReviews.Where(a => a.OrdId == id && a.IsActive != 0).FirstOrDefault();
            return de.Query<OrderReview>("EXECUTE GetAllRecords InternalReviews,customerid," + UserId + "").ToList();
        }
        //end massam 07-01-23
        public List<User> GetAllUsersList(SqlConnection de)
        {
            var record = de.Query<User>("EXECUTE GetAllRecords Users").ToList();
            return de.Query<User>("EXECUTE GetAllRecords Users").ToList();
        }

        //masam
        public List<Requesthelp> GetRequestsBySkills(string[] skills, SqlConnection de)
        {
            List<Requesthelp> Requestsbyskill = new List<Requesthelp>();
            if (skills != null)
            {
                for (int i = 0; i < skills.Length; i++)
                {
                    //var allrequests = de.Query<Requesthelp>("EXECUTE GetAllRecords Requesthelp").Where(x => x.skills.ToLower().Contains(skills[i].ToLower())).ToList();
                    //if (allrequests.Count() != 0)
                    //{
                    //    //Requestsbyskill.Add(allrequests);
                    //    Requestsbyskill.AddRange(allrequests);//list of data ko single list mein convert krta ha
                    //}
                }
                return Requestsbyskill.DistinctBy(p => p.Id).ToList();
            }
            return Requestsbyskill;
        }



        public Education GetEducationById(int id,SqlConnection de)
        {
            return de.Query<Education>("EXECUTE GetAllRecords Education,Id,"+id+"").FirstOrDefault();
        }
        public List<User> GetActiveUserList(SqlConnection de)
        {
            return de.Query<User>("EXECUTE GetAllRecords Users,Id,-1, ' Where IsActive =1 '").ToList();
        }

        //public List<User> GetButlerList(string zip,string city,SqlConnection de)
        //{
        //    return de.Query<User>("EXECUTE GetAllRecords Users,Id,-1, ' Where IsActive =1 AND Role =3 AND  ZipCode= ''"+zip+"'' OR  City= ''"+city+"''   '").ToList();
        //}

        public List<User> GetButlerList(SqlConnection de)
        {
            //var tst= de.Query<User>("EXECUTE GetAllRecords Users,Id,-1, ' Where IsActive =1 AND Role =3 '").ToList();
            return de.Query<User>("EXECUTE GetAllRecords Users,Id,-1, ' Where IsActive =1 AND Role =3 '").ToList();
        }

        public List<User> GetButlerListbyskills(string reqlangs,string zip,string city, List<string> skills /*string skills*/, int requestid, SqlConnection de,string requesttype=null)
        {
            
            //string[] arrayofskills = skills.Split(",").Select(a=>a.Trim()).ToArray();
            string[] arrayofskills = skills.ToArray();

            List<Skills> butlerskills = new List<Skills>();

            for (int i=0; i < arrayofskills.Length ;i++)
            {
                //var Allbutlerbyskill = de.Query<Skills>("EXECUTE GetAllRecords Skills").Where(x => x.SkillName.ToLower().Contains(arrayofskills[i].ToLower())).FirstOrDefault();
                var Allbutlerbyskill = de.Query<Skills>("EXECUTE GetAllRecords Skills").Where(x => x.SkillName.ToLower().Contains(arrayofskills[i].ToLower())).ToList();

                if (Allbutlerbyskill.Count()!=0)
                {
                    butlerskills.AddRange(Allbutlerbyskill);

                }
            }

            butlerskills = butlerskills.DistinctBy(x=>x.Id).ToList();
            //var butlerbyskill = de.Query<Skills>("EXECUTE GetAllRecords Skills,Id,-1, ' Where IsActive =1  AND  SkillName like '%' + skills + '%'  '").ToList();
            //var butlerbyskill = de.Query<Skills>("EXECUTE GetAllRecords Skills,Id,-1, ' Where IsActive =1  AND  SkillName = ''"+ skills +"''  '").ToList();

            //string s = "a,b, b, c";


            //var Alluserbyskills = new List<User>();

            //var alluserbylanguage = new List<User>();

            var Allusers = new List<User>();
            var Alluserbyskills = new List<User>();


            foreach (var u in butlerskills)
            {
                //var uSkills = u.SkillName.Split(',');
                int id = 0;
                if(u!=null  )
                {
                     id = u.UserId;

                }


                var userbyskill= GetActiveUserById(id, de);

                //if(reqlangs!="")
                //{


                //}
                //var lngsplit = reqlangs.Split(","); languages pending
                //var userlanguages = new List<UserLanguage>();
                //foreach (var lng in lngsplit)
                //{
                //    var userlanguage = Getuserlanguagebyid(id, de).Where(x => x.language.ToLower().Contains(lng.ToLower())).FirstOrDefault();
                //    if(userlanguage!=null)
                //    {
                //        userlanguages.Add(userlanguage);
                //    }
                    
                //}
                //var userlanguage = Getuserlanguagebyid(id, de).Where(x => x.language.ToLower().Contains(reqlangs.ToLower())).ToList();

                if(userbyskill!=null && userbyskill.Role==3)
                {
                    //var ucount = userlanguages.Count();
                    var already = Alluserbyskills.Where(x => x.Id == userbyskill.Id).FirstOrDefault();

                    if (already == null)
                    {

                        Alluserbyskills.Add(userbyskill);
                    }
                }
                

            }

            Allusers = Alluserbyskills;
            //string[] arrayofzip = zip.Split(" ");


            if(requesttype== "Livenow")
            {
                Alluserbyskills = Alluserbyskills.Where(x => (x.ZipCode.ToLower().Contains(zip.ToLower()) || x.City.ToLower().Contains(city.ToLower())) && x.Availability == 1 && x.Status == 1).ToList();

            }
            else
            {
                var requestprefertimes = Getrequestprefertime(requestid, de);
                //var butlerprefertime = Getbutlerprefertime(requestid, de);



                Alluserbyskills = Alluserbyskills.Where(x => x.ZipCode.ToLower().Contains(zip.ToLower()) || x.City.ToLower().Contains(city.ToLower())).ToList();
                
                foreach(var item in Alluserbyskills)
                {
                    item.butlerprefertimes = Getbutlerprefertimelist(item.Id, de);
                }

                foreach (var item in Alluserbyskills)
                {
                    //start massam 09-01-23
                    if (item.butlerprefertimes != null && item.butlerprefertimes[0].PreferTime == requestprefertimes[0])
                    //end massam 09-01-23
                    {
                        Alluserbyskills = Alluserbyskills.OrderByDescending(a => a.butlerprefertimes[0].PreferTime).ToList();
                        break;
                    }
                }

                if (Alluserbyskills.Count() == 0)
                {
                    Alluserbyskills = Allusers;
                }

            }

            //if (Alluserbyskills.Count() == 0)
            //{
            //    Alluserbyskills = Allusers;
            //}




            return Alluserbyskills;
        }

        public List<User> GetButlerListbyTags(string zip, string city, /*string Tags,*/List<string> Tags, SqlConnection de,string requesttype)
        {
            var Alluserbytag = new List<User>();
            //if(!string.IsNullOrEmpty(Tags))
            if(Tags.Count()!=0)
            {
                //string[] arrayoftags = Tags.Split(",").Select(a => a.Trim()).ToArray();
                string[] arrayoftags = Tags.ToArray();
                List<Tags> butlertags = new List<Tags>();

                for (int i = 0; i < arrayoftags.Length; i++)
                {
                    //var Allbutlerbyskill = de.Query<Skills>("EXECUTE GetAllRecords Skills").Where(x => x.SkillName.ToLower().Contains(arrayofskills[i].ToLower())).FirstOrDefault();
                    var butlerbytagss = de.Query<Tags>("EXECUTE GetAllRecords Tags").Where(x => x.IsActive == 1 && x.TagName.ToLower().Contains(arrayoftags[i].ToLower())).ToList();

                    if (butlertags != null)
                    {
                        butlertags.AddRange(butlerbytagss);

                    }
                }

                butlertags=butlertags.DistinctBy(x=>x.Id).ToList();
                var Allusers = new List<User>();

                foreach (var u in butlertags)
                {
                    int id = 0;

                    if (u != null)
                    {
                        id = u.UserId;

                    }

                    var userbytag = GetActiveUserById(id, de);

                    if (userbytag != null)
                    {
                        var already = Alluserbytag.Where(x => x.Id == userbytag.Id).FirstOrDefault();

                        if (already == null)
                        {

                            Alluserbytag.Add(userbytag);
                        }
                    }


                }

                Allusers = Alluserbytag;

                if(requesttype=="Livenow")
                {
                    Alluserbytag = Alluserbytag.Where(x => (x.ZipCode.ToLower().Contains(zip.ToLower()) || x.City.ToLower().Contains(city.ToLower())) && x.IsActive == 1 && x.Availability == 1 && x.Status == 1).ToList();

                }
                else
                {
                    Alluserbytag = Alluserbytag.Where(x => (x.ZipCode.ToLower().Contains(zip.ToLower()) || x.City.ToLower().Contains(city.ToLower())) && x.IsActive == 1).ToList();

                    if (Alluserbytag.Count() == 0)
                    {
                        Alluserbytag = Allusers;
                    }
                }
                




            }
            


            return Alluserbytag;
        }
        public List<Skills> GetAllSkills(SqlConnection de)
        {
            return de.Query<Skills>("EXECUTE GetAllRecords Skills").ToList();
        }

        public List<Tags> GetAllTags(SqlConnection de)
        {
            return de.Query<Tags>("EXECUTE GetAllRecords Tags").ToList();
        }

        public List<Order> GetOrdersById(int id , SqlConnection de)
        {
            //return de.Orders.Where(a => (a.BuyerId == id || a.SellerId == id) && a.IsActive == 1 && a.IsAccepted != 0).Include(a => a.Buyer).Include(a => a.Seller).ToList();
            //var getOrdersbyId = de.Query<Order>("EXECUTE GetAllRecords Orders,Id,-1, ' Where BuyerId=" + id + " OR SellerId=" + id + " AND IsAccepted <> 0 '").ToList();
            var getOrdersbyId = de.Query<Order>("EXECUTE GetAllRecords Orders,Id,-1, ' Where (BuyerId=" + id + " OR SellerId=" + id + ") AND IsAccepted <> 0 AND IsActive <> 0 '").OrderByDescending(x => x.Id).ToList();
            if(getOrdersbyId.Count()!=0)
            {
                foreach (var x in getOrdersbyId)
                {
                    var getbuyer = GetActiveUserById(x.BuyerId, de);
                    var getSeller = GetActiveUserById(x.SellerId, de);
                    x.Buyer = getbuyer;
                    x.Seller = getSeller;
                }

            }
            return getOrdersbyId;
        }


        public List<Order> GetAllOrders(SqlConnection de)
        {
            return de.Query<Order>("EXECUTE GetAllRecords Orders").Where(x=>x.IsActive == 1).ToList();
        }

        public List<Requesthelp> GetRequestsbyuserid(int userid,SqlConnection de)
        {
            return de.Query<Requesthelp>("EXECUTE GetAllRecords Requesthelp,userid,"+userid+" ").ToList();
        }

        public Requesthelp getLastrequestbyuserid(int userid, SqlConnection de)
        {
            return de.Query<Requesthelp>("EXECUTE GetAllRecords Requesthelp,userid," + userid + " ").OrderByDescending(x=>x.Id).FirstOrDefault();
        }

        public List<Requestkills> getrequestskillsbyrequestid(int id, SqlConnection de)
        {

            return de.Query<Requestkills>("EXECUTE GetAllRecords RequestSkills,Requestid," + id + " ").ToList();
        }
        public List<Requesthelp> GetRequestsbyuserskill(int userid, SqlConnection de)
        {
            var getsellerskill = GetSkillsById(userid, de);
            List<Requesthelp> Requestsbyskill = new List<Requesthelp>();
            if (getsellerskill != null)
            {
                //string[] arrayofskills = getsellerskill.SkillName.Split(",").Select(a=>a.TrimStart()).Select(a => a.TrimEnd()).ToArray();
                //List<Requesthelp> requestskill = new List<Requesthelp>();
                for (int i = 0; i < getsellerskill.Count; i++)
                {
                    //var allrequests = de.Query<Requesthelp>("EXECUTE GetAllRecords Requesthelp").Where(x => x.skills.ToLower().Contains(arrayofskills[i].ToLower())).ToList();
                    var allrequests = de.Query<Requesthelp>("EXECUTE GetAllRecords Requesthelp").Where(x => x.skills.ToLower().Contains(getsellerskill[i].SkillName.ToLower())).ToList();
                    if (allrequests.Count() != 0)
                    {
                        //Requestsbyskill.Add(allrequests);
                        Requestsbyskill.AddRange(allrequests);//list of data ko single list mein convert krta ha
                    }
                }
                return Requestsbyskill.DistinctBy(p => p.Id).ToList();
            }
            //var record = de.Query<Requesthelp>("EXECUTE GetAllRecords Requesthelp,skills,'''"+getsellerskill.SkillName + "''' ");
            //if (getsellerskill != null)
            //{
            //    return de.Query<Requesthelp>("EXECUTE GetAllRecords Requesthelp,skills,'''" + getsellerskill.SkillName + "''' ").ToList();
            //}
            return Requestsbyskill;
        }

        public List<UserLanguage> Getuserlanguagebyid(int userid, SqlConnection de)
        {
            return de.Query<UserLanguage>("EXECUTE GetAllRecords UserLanguage,userid," + userid + " ").ToList();
        }
        //public List<Requesthelp> GetRequestsbyuserTags(int userid, SqlConnection de)
        //{
        //    var getsellertags = GetTagsById(userid, de);
        //    List<Requesthelp> Requestsbytags = new List<Requesthelp>();

        //    if (getsellertags!= null)
        //    {
        //        if (!string.IsNullOrEmpty(getsellertags.TagName))
        //        {


        //            string[] arrayoftags = getsellertags.TagName.Split(",").Select(a=>a.TrimStart()).Select(x=>x.TrimEnd()).ToArray();
        //            //List<Requesthelp> Requestsbytags = new List<Requesthelp>();

        //            for (int i = 0; i < arrayoftags.Length; i++)
        //            {
        //                var allrequests = de.Query<Requesthelp>("EXECUTE GetAllRecords Requesthelp").Where(x => x.Tags.ToLower().Contains(arrayoftags[i].ToLower())).ToList();

        //                if (allrequests != null)
        //                {
        //                    Requestsbytags.AddRange(allrequests);

        //                }
        //            }

        //            return Requestsbytags.DistinctBy(x => x.Id).ToList();
        //        }
        //    }

        //    return Requestsbytags;

        //    //var record = de.Query<Requesthelp>("EXECUTE GetAllRecords Requesthelp,skills,'''"+getsellerskill.SkillName + "''' ");


        //    //return de.Query<Requesthelp>("EXECUTE GetAllRecords Requesthelp,Tags,'''" + getsellertags.TagName + "''' ").ToList();


        //}

        public List<Requesthelp> GetRequestsbyuserTags(int userid, SqlConnection de)
        {
            var getsellertags = GetTagsById(userid, de).ToArray();
            List<Requesthelp> Requestsbytags = new List<Requesthelp>();
            if (getsellertags != null)
            {
                //if (!string.IsNullOrEmpty(getsellertags.TagName))
                //{
                //string[] arrayoftags = getsellertags.TagName.Split(",").Select(a=>a.TrimStart()).Select(x=>x.TrimEnd()).ToArray();
                //List<Requesthelp> Requestsbytags = new List<Requesthelp>();
                for (int i = 0; i < getsellertags.Count(); i++)
                {
                    var allrequests = de.Query<Requesthelp>("EXECUTE GetAllRecords Requesthelp").Where(x => x.Tags.ToLower().Contains(getsellertags[i].TagName.ToLower())).ToList();
                    if (allrequests != null)
                    {
                        Requestsbytags.AddRange(allrequests);
                    }
                }
                return Requestsbytags.DistinctBy(x => x.Id).ToList();
                //}
            }
            return Requestsbytags;
            //var record = de.Query<Requesthelp>("EXECUTE GetAllRecords Requesthelp,skills,'''"+getsellerskill.SkillName + "''' ");
            //return de.Query<Requesthelp>("EXECUTE GetAllRecords Requesthelp,Tags,'''" + getsellertags.TagName + "''' ").ToList();
        }
        public List<Requesthelp> GetAllRequests(SqlConnection de)
        {
            return de.Query<Requesthelp>("EXECUTE GetAllRecords Requesthelp").ToList();
        }

        //public List<UserLanguage> Getuserlanguagebyid(int userid, SqlConnection de)
        //{
            
        //    return de.Query<UserLanguage>("EXECUTE GetAllRecords UserLanguage,userid," + userid + " ").ToList();
        //}
        //public List<Requesthelp> GetRequestsbyRequestid(int reqid, SqlConnection de)
        //{
        //    return de.Query<Requesthelp>("EXECUTE GetAllRecords Requesthelp,userid," + reqid + " ").FirstOrDefault();
        //}

        public Experience UpdateUserexperience(Experience updrequest, SqlConnection de)
        {
            try
            {
                int ids = 0;

                int id = updrequest.Id;
                Experience exp = Getexperiencebyid(id, de);
                exp.Id = updrequest.Id;
                exp.ReferalContact = updrequest.ReferalContact;
                exp.userid = updrequest.userid;
                exp.IsActive = 1;
                exp.updatedat = updrequest.updatedat;
                exp.Designation = updrequest.Designation;
                exp.FromDate = updrequest.FromDate;
                exp.ToDate = updrequest.ToDate;
                exp.Organization = updrequest.Organization;
                exp.Organization_Reference = updrequest.Organization_Reference;
                


                var getPropandVal = GetUpdatePropandVal(exp);
                var Updaterequest = de.Query("EXECUTE InsertOrUpdate " + exp.Id + ",Experience,'" + getPropandVal + "'");
                return exp;
            }
            catch
            {
                return null;
            }
        }
        public Experience Getexperiencebyid(int Id, SqlConnection de)
        {
            return de.Query<Experience>("EXECUTE GetAllRecords Experience,Id," + Id + "").FirstOrDefault();
        }
        public Requesthelp GetRequestsbyRequestid(int Id, SqlConnection de)
        {
            //var tstreq= de.Query<Requesthelp>("EXECUTE GetAllRecords Requesthelp,Id," + Id + "").FirstOrDefault();
            return de.Query<Requesthelp>("EXECUTE GetAllRecords Requesthelp,Id," + Id + "").FirstOrDefault();
        }

        public List<Notification> GetRequestsNotificationsbyid(int Id, SqlConnection de)
        {
            return de.Query<Notification>("EXECUTE GetAllRecords Notifications,Requestid," + Id + "").ToList();
        }
        public async Task<bool> DeleteRequest(int id, SqlConnection de)
        {
            try
            {
                Requesthelp req = GetRequestsbyRequestid(id, de);

                req.Isactive = 0;
                req.DeletedAt = DateTime.UtcNow.ToString();
                var getPropandVal = GetUpdatePropandVal(req);
                var delete = de.Query("EXECUTE InsertOrUpdate " + req.Id + ",Requesthelp,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteRequestNotifications(int requestid, SqlConnection de)
        {
            try
            {
               List<Notification>  requestnotifications = GetRequestsNotificationsbyid(requestid, de);
                foreach(var item in requestnotifications)
                {
                    item.IsActive = 0;
                    item.DeletedAt = DateTime.UtcNow.ToString();
                    var getPropandVal = GetUpdatePropandVal(item);
                    var delete = de.Query("EXECUTE InsertOrUpdate " + item.Id + ",Notification,'" + getPropandVal + "'");

                }

                
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateRequesthelp(Requesthelp updrequest, List<string> skills, string Tags, string langs,List<string> preferservicetime, SqlConnection de)
        {
            try
            {  int id = updrequest.Id;
                int uid = updrequest.userid;
                var userdata = GetActiveUserById(uid, de);
                var updservicetime = updateservicetime(id, preferservicetime, de);
                var updrequestskills = updRequestSkills(id, skills, de);



                Requesthelp req = GetRequestsbyRequestid(id, de);
                req.Id= updrequest.Id;
                req.Title = updrequest.Title;
                //req.skills = updrequest.skills;
                //req.skills = skills;

                req.UpdatedAt = DateTime.UtcNow;
                req.budget = updrequest.budget;
                //req.City = updrequest.City;
                req.City = userdata.City;

                req.FromDateTime = updrequest.FromDateTime;
                req.ToDateTime=updrequest.ToDateTime;
                //req.Zipcodes = updrequest.Zipcodes;
                req.Zipcodes = userdata.ZipCode;

                req.userid = updrequest.userid;
                req.CreatedAt=updrequest.CreatedAt;
                req.Description = updrequest.Description;
                //req.Tags= updrequest.Tags;
                req.Tags = Tags;
                req.Language = langs;
                req.Isactive=updrequest.Isactive;
                req.Requesteduser = updrequest.Requesteduser;
                req.DeletedAt = updrequest.DeletedAt;


                var getPropandVal = GetUpdatePropandVal(req);
                var Updaterequest = de.Query("EXECUTE InsertOrUpdate " + req.Id + ",Requesthelp,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }
        //massam

        public async Task<bool> UpdateRequestForEmailSend(Requesthelp updrequest, SqlConnection de)
        {
            try
            {
               

                updrequest.UpdatedAt = DateTime.UtcNow;

                var getPropandVal = GetUpdatePropandVal(updrequest);
                var Updaterequest = de.Query("EXECUTE InsertOrUpdate " + updrequest.Id + ",Requesthelp,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> updateservicetime(int requestid, List<string> preferservicetime,SqlConnection de)
        {
            try
            {
                List<preferservicetime> pref = Getpreferservicetimebyid(requestid, de);

               for(int i = 0; i < preferservicetime.Count; i++)
                {
                    if (i > pref.Count() - 1)
                    {

                        preferservicetime pre = new preferservicetime()

                        {

                            //Id = 1,
                            IsActive = 1,
                            CreatedAt = DateTime.Now.ToLongDateString(),
                            requestid = requestid,
                            servicetime = preferservicetime[i],



                        };

                        var preftimestatus = new UserBL().AddRequestprefertime(pre, de);

                    }

                    else
                    {

                        pref[i].servicetime = preferservicetime[i];
                        pref[i].UpdatedAt = DateTime.UtcNow.ToLongDateString();

                        var getPropandVal = GetUpdatePropandVal(pref[i]);
                        var Updaterequest = de.Query("EXECUTE InsertOrUpdate " + pref[i].Id + ",preferservicetimes,'" + getPropandVal + "'");
                    }



                }
                
                return true;
            }
            catch
            {
                return false;
            }
        }
        //Start Massam 07-01-23
        public async Task<bool> updRequestSkills(int requestid, List<string> requestskills, SqlConnection de)
        {
            try
            {
                List<Requestkills> reqskills = getrequestskillsbyrequestid(requestid, de);
                int skillsLoopLength = 0;
                if (requestskills.Count > reqskills.Count)
                {
                    skillsLoopLength = requestskills.Count;
                }
                else
                {
                    skillsLoopLength = reqskills.Count;
                }
                if (requestskills.Count != 0)
                {
                    var new_skills = new Requestkills();
                    if (reqskills.Count != 0)
                    {
                        for (int i = 0; i < skillsLoopLength; i++)
                        {
                            if (i < reqskills.Count && requestskills.Count > i)
                            {
                                reqskills[i].Requested_SkillName = requestskills[i].ToLower();
                                reqskills[i].CreatedAt = DateTime.UtcNow;
                                var getPropandVal = GetUpdatePropandVal(reqskills[i]);
                                var Updaterequest = de.Query("EXECUTE InsertOrUpdate " + reqskills[i].Id + ",RequestSkills,'" + getPropandVal + "'");
                            }
                            else if (requestskills.Count > reqskills.Count)
                            {
                                new_skills.Requested_SkillName = requestskills[i].ToLower();
                                new_skills.CreatedAt = DateTime.UtcNow;
                                new_skills.IsActive = 1;
                                new_skills.Requestid = requestid;
                                var requestskillsstatus = new UserBL().AddRequestSkills(new_skills, de);
                            }
                            else
                            {
                                reqskills[i].IsActive = 0;
                                reqskills[i].UpdatedAt = DateTime.UtcNow;
                                var getPropandVal = GetUpdatePropandVal(reqskills[i]);
                                var Updaterequest = de.Query("EXECUTE InsertOrUpdate " + reqskills[i].Id + ",RequestSkills,'" + getPropandVal + "'");
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < requestskills.Count; i++)
                        {
                            new_skills.Requested_SkillName = requestskills[i].ToLower();
                            new_skills.CreatedAt = DateTime.UtcNow;
                            new_skills.IsActive = 1;
                            new_skills.Requestid = requestid;
                            var requestskillsstatus = new UserBL().AddRequestSkills(new_skills, de);
                        }
                    }
                }
                //End Massam 06-01-23
                //for (int i = 0; i < skillsLoopLength; i++)
                //{
                //    if (i > reqskills.Count() - 1)
                //    {
                //        Requestkills reqskill = new Requestkills()
                //        {
                //            //Id = 1,
                //            IsActive = 1,
                //            CreatedAt = DateTime.UtcNow,
                //            Requestid = requestid,
                //            Requested_SkillName = requestskills[i],
                //        };
                //        var requestskillsstatus = new UserBL().AddRequestSkills(reqskill, de);
                //    }
                //    else if (requestskills.Count() < reqskills.Count())
                //    {
                //        reqskills[i].IsActive = 0;
                //        reqskills[i].UpdatedAt = DateTime.UtcNow;
                //    }
                //    else
                //    {
                //        reqskills[i].Requested_SkillName = requestskills[i];
                //        reqskills[i].UpdatedAt = DateTime.UtcNow;
                //    }
                //    var getPropandVal = GetUpdatePropandVal(reqskills[i]);
                //    var Updaterequest = de.Query("EXECUTE InsertOrUpdate " + reqskills[i].Id + ",RequestSkills,'" + getPropandVal + "'");
                //}
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool addUpdateButlerPreferTime(List<string> butlerprefertimes, int userid, SqlConnection de)
        {
            try
            {
                List<butlerprefertime> userPrefTime = Getbutlerprefertimelist(userid, de);
                if(userPrefTime.Count()!=0)
                {
                    int prefTimeLoopLength = 0;
                    if (butlerprefertimes.Count > userPrefTime.Count)
                    {
                        prefTimeLoopLength = butlerprefertimes.Count;
                    }
                    else
                    {
                        prefTimeLoopLength = userPrefTime.Count;
                    }
                    if (butlerprefertimes.Count != 0)
                    {
                        var new_PrefTime = new butlerprefertime();
                        if (userPrefTime.Count != 0)
                        {
                            for (int i = 0; i < prefTimeLoopLength; i++)
                            {
                                if (i < userPrefTime.Count && butlerprefertimes.Count > i)
                                {
                                    userPrefTime[i].PreferTime = butlerprefertimes[i].ToLower();
                                    userPrefTime[i].CreatedAt = DateTime.UtcNow.ToString();
                                    var getPropandVal = GetUpdatePropandVal(userPrefTime[i]);
                                    var Updaterequest = de.Query("EXECUTE InsertOrUpdate " + userPrefTime[i].Id + ",butlerprefertime,'" + getPropandVal + "'");
                                }
                                else if (butlerprefertimes.Count > userPrefTime.Count)
                                {
                                    new_PrefTime.PreferTime = butlerprefertimes[i].ToLower();
                                    new_PrefTime.CreatedAt = DateTime.UtcNow.ToString();
                                    new_PrefTime.IsActive = 1;
                                    new_PrefTime.UserId = userid;
                                    var getPropandVal = GetPropandVal(new_PrefTime);
                                    var add = de.Query("EXECUTE InsertOrUpdate 0,butlerprefertime," + getPropandVal[0] + "," + getPropandVal[1] + "");
                                }
                                else
                                {
                                    userPrefTime[i].IsActive = 0;
                                    userPrefTime[i].UpdatedAt = DateTime.UtcNow.ToString();
                                    var getPropandVal = GetUpdatePropandVal(userPrefTime[i]);
                                    var Updaterequest = de.Query("EXECUTE InsertOrUpdate " + userPrefTime[i].Id + ",butlerprefertime,'" + getPropandVal + "'");
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < butlerprefertimes.Count; i++)
                            {
                                new_PrefTime.PreferTime = butlerprefertimes[i].ToLower();
                                new_PrefTime.CreatedAt = DateTime.UtcNow.ToString();
                                new_PrefTime.IsActive = 1;
                                new_PrefTime.UserId = userid;
                                var getPropandVal = GetPropandVal(new_PrefTime);
                                var add = de.Query("EXECUTE InsertOrUpdate 0,butlerprefertime," + getPropandVal[0] + "," + getPropandVal[1] + "");
                            }
                        }
                    }

                    
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool addUpdateButlerlanguage(List<string> butlerlanguage, int userid, SqlConnection de)
        {
            try
            {
                List<UserLanguage> userlangs = Getuserlanguagebyid(userid, de);
                int languageLoopLength = 0;
                if (butlerlanguage.Count > userlangs.Count)
                {
                    languageLoopLength = butlerlanguage.Count;
                }
                else
                {
                    languageLoopLength = userlangs.Count;
                }
                if (butlerlanguage.Count != 0)
                {
                    var new_language = new UserLanguage();
                    if (userlangs.Count != 0)
                    {
                        for (int i = 0; i < languageLoopLength; i++)
                        {
                            if (i < userlangs.Count && butlerlanguage.Count > i)
                            {
                                userlangs[i].language = butlerlanguage[i];
                                userlangs[i].CreatedAt = DateTime.UtcNow.ToString();
                                var getPropandVal = GetUpdatePropandVal(userlangs[i]);
                                var Updaterequest = de.Query("EXECUTE InsertOrUpdate " + userlangs[i].Id + ",UserLanguage,'" + getPropandVal + "'");
                            }
                            else if (butlerlanguage.Count > userlangs.Count)
                            {
                                new_language.language = butlerlanguage[i];
                                new_language.CreatedAt = DateTime.UtcNow.ToString();
                                new_language.IsActive = 1;
                                new_language.UserId = userid;
                                var getPropandVal = GetPropandVal(new_language);
                                var add = de.Query("EXECUTE InsertOrUpdate 0,UserLanguage," + getPropandVal[0] + "," + getPropandVal[1] + "");
                            }
                            else
                            {
                                userlangs[i].IsActive = 0;
                                userlangs[i].UpdatedAt = 1;
                                var getPropandVal = GetUpdatePropandVal(userlangs[i]);
                                var Updaterequest = de.Query("EXECUTE InsertOrUpdate " + userlangs[i].Id + ",UserLanguage,'" + getPropandVal + "'");
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < butlerlanguage.Count; i++)
                        {
                            new_language.language = butlerlanguage[i];
                            new_language.CreatedAt = DateTime.UtcNow.ToString();
                            new_language.IsActive = 1;
                            new_language.UserId = userid;
                            var getPropandVal = GetPropandVal(new_language);
                            var add = de.Query("EXECUTE InsertOrUpdate 0,UserLanguage," + getPropandVal[0] + "," + getPropandVal[1] + "");
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        //Start Massam 07-01-23
        public List<preferservicetime> Getpreferservicetimebyid(int Id, SqlConnection de)
        {
            return de.Query<preferservicetime>("EXECUTE GetAllRecords preferservicetimes,requestid," + Id + "").ToList();
        }

        public List<Logging> GetAllLogs(SqlConnection de)
        {
            return de.Query<Logging>("EXECUTE GetAllRecords Logging").ToList();
        }

        internal List<Education> GetAllEducations(SqlConnection de)
        {
            return de.Query<Education>("EXECUTE GetAllRecords Education").ToList();
        }

        public string Getbutlerprefertime(int userid,SqlConnection de)
        {

            string prefertimestring = "";
            var prefertimes= de.Query<butlerprefertime>("EXECUTE GetAllRecords butlerprefertime,UserId," + userid + "").Select(a=>a.PreferTime).ToList();

            if (prefertimes.Count > 0)
            {
                prefertimestring = string.Join(",", prefertimes);
            }

            return prefertimestring;


        }


        public List<string> Getrequestprefertime(int requestid, SqlConnection de)
        {

            List<string> prefertimestring = new List<string>();
            var prefertimes = de.Query<preferservicetime>("EXECUTE GetAllRecords preferservicetimes,requestid," + requestid + "").Select(a => a.servicetime).ToList();

            if (prefertimes.Count > 0)
            {
                for(int i=0;i<prefertimes.Count;i++)
                {

                    if (prefertimes[i] == "08,09,10,11,12")
                    {
                        prefertimes[i] = "Morning(8am-12pm)";
                        prefertimestring.Add(prefertimes[i]);
                    }
                    else if (prefertimes[i] == "12,13,14,15,16,17")
                    {
                        prefertimes[i] = "Afternoon(12pm-5pm)";
                        prefertimestring.Add(prefertimes[i]);
                    }
                    else if (prefertimes[i] == "17,18,19,20,21,22")
                    {
                        prefertimes[i] = "Evening(5pm-10pm)";
                        prefertimestring.Add(prefertimes[i]);
                    }
                    else if (prefertimes[i] == "22,23,00,01,02,03,04,05,06,07")
                    {
                        prefertimes[i] = "Night(10pm-7am)";
                        prefertimestring.Add(prefertimes[i]);
                    }

                }

                    

            }

            return prefertimestring;



        }

        public List<butlerprefertime> Getbutlerprefertimelist(int userid, SqlConnection de)
        {

            var prefertimes = de.Query<butlerprefertime>("EXECUTE GetAllRecords butlerprefertime,UserId," + userid + "").ToList();

            if(prefertimes.Count>0)
            {
                return prefertimes;

            }


            return null;


        }

        public List<Requestkills>Getallrequestskillbyid(int Id, SqlConnection de)
        {
            return de.Query<Requestkills>("EXECUTE GetAllRecords RequestSkills,Requestid," + Id + "").ToList();
        }

        public List<RequestTags> GetallrequestTagsbyid(int Id, SqlConnection de)
        {
            return de.Query<RequestTags>("EXECUTE GetAllRecords RequestTags,Requestid," + Id + "").ToList();
        }

        public User GetUserById(int Id, SqlConnection de)
        {
            return de.Query<User>("EXECUTE GetAllRecords Users,Id,"+Id+"").FirstOrDefault();
        }

        public Notification GetActivenotificationById(int Id, SqlConnection de)
        {
            return de.Query<Notification>("EXECUTE GetAllRecords Notifications,Id," + Id + "").FirstOrDefault();
        }
        public User GetActiveUserById(int Id, SqlConnection de)
        {
            return de.Query<User>("EXECUTE GetAllRecords Users,Id," + Id + "").FirstOrDefault();
        }
        public List<Notification>  GetAllnotifications(int Id, SqlConnection de)
        {
            return de.Query<Notification>("EXECUTE GetAllRecords Notifications,UserId," + Id + "").ToList();
        }
        //public Skills GetSkillsById(int Id, SqlConnection de)
        //{
        //     return de.Query<Skills>("EXECUTE GetAllRecords Skills,UserId,"+Id+"").FirstOrDefault();
        //}
        public List<Skills> GetSkillsById(int Id, SqlConnection de)
        {
            //return de.Query<Skills>("EXECUTE GetAllRecords Skills,UserId,"+Id+"").FirstOrDefault();
            return de.Query<Skills>("EXECUTE GetAllRecords Skills,UserId," + Id + "").ToList();
        }
        //public Tags GetTagsById(int Id , SqlConnection de)
        //{
        //    return de.Query<Tags>("EXECUTE GetAllRecords Tags,UserId," + Id + "").FirstOrDefault();
        //}

        public List<Tags> GetTagsById(int Id, SqlConnection de)
        {
            return de.Query<Tags>("EXECUTE GetAllRecords Tags,UserId," + Id + "").ToList();
        }
        public List<Experience> Getuserexperiencebyid(int Id, SqlConnection de)
        {
            return de.Query<Experience>("EXECUTE GetAllRecords Experience,UserId," + Id + "").ToList();
        }

        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        [RequestSizeLimit(209715200)]
        public async Task<string> FileUpload(IFormFile ImagePath=null)
        {
            try
            {
                string DirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot/Images/User");

                if (ImagePath != null)
                {
                    if (!Directory.Exists(DirectoryPath))
                    {
                        Directory.CreateDirectory(DirectoryPath);
                    }

                    var FileExtention = Path.GetExtension(ImagePath.FileName);

                    string FileName = "Invoice" + "_" + DateTime.UtcNow.Ticks.ToString() + "" + FileExtention;
                    var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwRoot/Images/User/",
                        FileName);

                    var setPath = "Images/User/" + FileName;
                    FileExtention = FileExtention.ToLower();
                    if (FileExtention.Contains("jpg") || FileExtention.Contains("jpeg") || FileExtention.Contains("png") || FileExtention.Contains("svg") || FileExtention.Contains("webp"))
                    {
                        using var image = Image.Load(ImagePath.OpenReadStream());
                        image.Mutate(x => x.Resize(256, 256));
                        image.Save(path, new JpegEncoder { Quality = 100 });
                    }
                    else
                    {
                        using (var stream = new FileStream(path, FileMode.Create))
                        {

                           await ImagePath.CopyToAsync(stream);
                        }
                    }
                    var img = setPath;
                    return img;
                }
                return null;
            }
            catch
            {
                return null;
            }
            
        }

        public List<Feedback> GetAllFeedBacks(SqlConnection de)
        {
            return de.Query<Feedback>("EXECUTE GetAllRecords Feedbacks").ToList();
        }

        public List<Refferals> GetAllRefferals(SqlConnection de)
        {
            return de.Query<Refferals>("EXECUTE GetAllRecords Refferals").ToList();
        }

        public bool UpdateEducation(Education n, SqlConnection de)
        {
            try
            {
                var getPropandVal = GetUpdatePropandVal(n);
                var update = de.Query("EXECUTE InsertOrUpdate " + n.Id + ",Education,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddFeedback(Feedback model, SqlConnection de)
        {
            try
            {
                var getPropandVal = GetPropandVal(model);
                var add = de.Query("EXECUTE InsertOrUpdate 0,Feedback," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateOrder(Order ed, SqlConnection de)
        {
            try
            {
                var getPropandVal = GetUpdatePropandVal(ed);
                var update = de.Query("EXECUTE InsertOrUpdate " + ed.Id + ",Orders,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<OrderReview> GetAllReviews(SqlConnection de)
        {
            return de.Query<OrderReview>("EXECUTE GetAllRecords OrderReviews").ToList();
        }

        public bool AddReview(OrderReview review, SqlConnection de)
        {
            try
            {
                var getPropandVal = GetPropandVal(review);
                var add = de.Query("EXECUTE InsertOrUpdate 0,OrderReviews," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;
            }
            catch
            {
                return false;
            }
          
        }

        public bool UpdateReview(OrderReview getReview, SqlConnection de)
        {
            try
            {
                var getPropandVal = GetUpdatePropandVal(getReview);
                var add = de.Query("EXECUTE InsertOrUpdate " + getReview.Id + ",OrderReviews,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public OrderReview GetReviewById(int id, SqlConnection de)
        {
            // return de.OrderReviews.Where(a => a.OrdId == id && a.IsActive != 0).FirstOrDefault();
            return de.Query<OrderReview>("EXECUTE GetAllRecords OrderReviews,OrdId," + id + "").FirstOrDefault();
        }

        public async Task<bool> AddUser(User _user, SqlConnection de)
        {
            try
            {
                var getPropandVal = GetPropandVal(_user);
                var add = de.Query("EXECUTE InsertOrUpdate 0,Users," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;
                
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Addnotification(Notification _notif, SqlConnection de)
        {
            try
            {
                var getPropandVal = GetPropandVal(_notif);
                var add = de.Query("EXECUTE InsertOrUpdate 0,Notifications," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

            }
            catch
            {
                return false;
            }
        }

        //public async Task<bool> AddRequest(Requesthelp _req, SqlConnection de)
        public int AddRequest(Requesthelp _req, SqlConnection de)

        {
            try
            {
                var getPropandVal = GetPropandVal(_req);
                var add = de.Query("EXECUTE InsertOrUpdate 0,Requesthelp," + getPropandVal[0] + "," + getPropandVal[1] + "").First();
                var requestid = Convert.ToInt32(add.Id);

                return requestid;
                //return true;

            }
            catch
            {
                return 0;
            }
        }

        public bool AddRequestlanguages(UserLanguage _lang, SqlConnection de)
        {
            try
            {
               
                var n = 0;
                var getPropandVal = GetPropandVal(_lang);
                var add = de.Query("EXECUTE InsertOrUpdate 0,UserLanguage," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

            }
            catch
            {
                return false;
            }
        }
        public bool Addbutlerprefertime(butlerprefertime _pref, SqlConnection de)
        {
            try
            {

                var n = 0;
                var getPropandVal = GetPropandVal(_pref);
                var add = de.Query("EXECUTE InsertOrUpdate 0,butlerprefertime," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

            }
            catch
            {
                return false;
            }
        }

        public bool AddRequestservicetime(preferservicetime _pref, SqlConnection de)
        {
            try
            {

                var n = 0;
                var getPropandVal = GetPropandVal(_pref);
                var add = de.Query("EXECUTE InsertOrUpdate 0,preferservicetimes," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

            }
            catch
            {
                return false;
            }
        }
        public bool Adduserexperience(Experience _exp, SqlConnection de)
        {
            try
            {

                var n = 0;
                var getPropandVal = GetPropandVal(_exp);
                var add = de.Query("EXECUTE InsertOrUpdate 0,Experience," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

            }
            catch
            {
                return false;
            }
        }

        public bool addInternalReview(InternalReview _review, SqlConnection de)
        {
            try
            {

                var n = 0;
                var getPropandVal = GetPropandVal(_review);
                var add = de.Query("EXECUTE InsertOrUpdate 0,InternalReviews," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

            }
            catch
            {
                return false;
            }
        }

        public InternalReview getInternalReview(int customerid/*, int orderid*/, int userid, SqlConnection de)
        {
            return de.Query<InternalReview>("EXECUTE GetAllRecords InternalReviews,Id,-1,' Where customerid=" +customerid+ " AND Sellerid="+ userid+ " ORDER BY Id DESC'").FirstOrDefault();
        }

        //start massam (28-12-22)
        public InternalReview getInternalReviewByUserIds(int customerid/*, int orderid*/, int userid, SqlConnection de)
        {
            try
            {
                //var a = de.Query<InternalReview>("EXECUTE GetAllRecords InternalReviews,Id,-1,' Where customerid=" + customerid + " AND Sellerid=" + userid + " ORDER BY Id DESC'").FirstOrDefault();
                return de.Query<InternalReview>("EXECUTE GetAllRecords InternalReviews,Id,-1,' Where customerid=" + customerid + " AND Sellerid=" + userid + " ORDER BY Id DESC'").FirstOrDefault();
                //return de.Query<InternalReview>("EXECUTE GetAllRecords InternalReviews").Where(x => x.customerid == customerid.ToString() && x.Sellerid == userid.ToString()).FirstOrDefault();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public InternalReview getInternalReviewByOrderId(int orderId, SqlConnection de)
        {
            try
            {
                return de.Query<InternalReview>("EXECUTE GetAllRecords InternalReviews,Orderid," + orderId + "").FirstOrDefault();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        //end massam (28-12-22)
        //massam (26-12-22)
        public Refferals GetReferralByButlerId(int butlerId, SqlConnection de)
        {
            try
            {
                return de.Query<Refferals>("EXECUTE GetAllRecords Refferals,RefferedId," + butlerId + "").FirstOrDefault();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public User GetUserReccordByReferralCode(string referralCode, SqlConnection de)
        {
            try
            {
                return de.Query<User>("EXECUTE GetAllRecords Users,Refferal_Code,'''" + referralCode + "'''").FirstOrDefault();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<Refferals> GetButlersReferralReccordByReferralCode(string referralCode, SqlConnection de)
        {
            try
            {
                //return de.Query<Refferals>("EXECUTE GetAllRecords Refferals,RefferalCode," +referralCode+ "").ToList();
                return de.Query<Refferals>("EXECUTE GetAllRecords Refferals,Id,-1,' Where RefferalCode=''" + referralCode + "'' AND ReferredUserType=1'").ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        //End massam (26-12-22)




        public async Task<bool> UpdateNotifications(int id,int userid,int isdelete, SqlConnection de)
        {

            try
            {
                Notification N = GetActivenotificationById(id, de);

               if(isdelete==1)
                {
                    N.IsActive = 0;
                    N.DeletedAt = DateTime.Now.ToString();
                    var getPropandVal = GetUpdatePropandVal(N);
                    var query = "EXECUTE InsertOrUpdate " + N.Id + ",Notifications,'" + getPropandVal + "'";
                    var update = de.Query("EXECUTE InsertOrUpdate " + N.Id + ",Notifications,'" + getPropandVal + "'");
                    return true;
                }
                else
                {
                    N.IsRead = 1;
                    var getPropandVal = GetUpdatePropandVal(N);
                    var query = "EXECUTE InsertOrUpdate " + N.Id + ",Notifications,'" + getPropandVal + "'";
                    var update = de.Query("EXECUTE InsertOrUpdate " + N.Id + ",Notifications,'" + getPropandVal + "'");
                    return true;

                }
                
            }
            catch
            {
                return false;
            }
        }
        public bool AddRefferal(Refferals r , SqlConnection de)
        {
            try
            {
                var getPropandVal = GetPropandVal(r);
                var add = de.Query("EXECUTE InsertOrUpdate 0,Refferals," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

               
            }
            catch
            {
                return false;
            }
        }

        public bool AddEducation(Education edu, SqlConnection de)
        {
            try
            {
                var getPropandVal = GetPropandVal(edu);
                var add = de.Query("EXECUTE InsertOrUpdate 0,Education," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddSkills(Skills skills, SqlConnection de)
        {
            try
            {
                
                if(skills.Id!=0)
                {
                    var getupdProp = GetUpdatePropandVal(skills);
                    //var qr = "EXECUTE InsertOrUpdate " + skills.Id + ",Skills,'" + getupdProp + "'";
                    var update = de.Query("EXECUTE InsertOrUpdate " + skills.Id + ",Skills,'" + getupdProp + "'");
                    return true;
                }
                var getPropandVal = GetPropandVal(skills);
                //var query = "EXECUTE InsertOrUpdate 0,Skills," + getPropandVal[0] + "," + getPropandVal[1] + "";
                var add = de.Query("EXECUTE InsertOrUpdate 0,Skills," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;
               
            }
            catch
            {
                return false;
            }
        }


        //public bool AddRequestSkills(Requestkills skills, SqlConnection de)
        //{
        //    try
        //    {

        //        if (skills.Id != 0)
        //        {
        //            var getupdProp = GetUpdatePropandVal(skills);
        //            //var qr = "EXECUTE InsertOrUpdate " + skills.Id + ",Skills,'" + getupdProp + "'";
        //            var update = de.Query("EXECUTE InsertOrUpdate " + skills.Id + ",Requestkills,'" + getupdProp + "'");
        //            return true;
        //        }
        //        var getPropandVal = GetPropandVal(skills);
        //        //var query = "EXECUTE InsertOrUpdate 0,Skills," + getPropandVal[0] + "," + getPropandVal[1] + "";
        //        var add = de.Query("EXECUTE InsertOrUpdate 0,Requestkills," + getPropandVal[0] + "," + getPropandVal[1] + "");
        //        return true;

        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public bool AddRequestSkills(Requestkills skills, SqlConnection de)
        {
            try
            {

                var n = 0;
                var getPropandVal = GetPropandVal(skills);
                var add = de.Query("EXECUTE InsertOrUpdate 0,RequestSkills," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

            }
            catch
            {
                return false;
            }
        }

        public bool AddRequestTags(RequestTags tags, SqlConnection de)
        {
            try
            {

                var n = 0;
                var getPropandVal = GetPropandVal(tags);
                var add = de.Query("EXECUTE InsertOrUpdate 0,RequestTags," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

            }
            catch
            {
                return false;
            }
        }
        public bool AddTags(Tags userTags, SqlConnection de)
        {
            try
            {
                
                if (userTags.Id != 0)
                {
                    var getupdProp = GetUpdatePropandVal(userTags);
                    // var query = "EXECUTE InsertOrUpdate " + userTags.Id + ",Tags,'" + getPropandVal + "'";
                    var update = de.Query("EXECUTE InsertOrUpdate " + userTags.Id + ",Tags,'" + getupdProp + "'");
                    return true;
                }
                var getPropandVal = GetPropandVal(userTags);
                var add = de.Query("EXECUTE InsertOrUpdate 0,Tags," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;
             
            }
            catch
            {
                return false;
            }
        }

        public Order GetOrderById(int id, SqlConnection de)
        {
            //return de.Orders.Where(a => a.Id == id && a.IsActive==1).Include(b=>b.Seller).Include(c=>c.Buyer).FirstOrDefault();
            var getOrdersbyId = de.Query<Order>("EXECUTE GetAllRecords Orders,Id,"+id+"").FirstOrDefault();
            getOrdersbyId.Buyer = GetActiveUserById(getOrdersbyId.BuyerId, de);
            getOrdersbyId.Seller = GetActiveUserById(getOrdersbyId.SellerId, de);
            return getOrdersbyId;
        }


        
        public async Task<bool> UpdateUser(User _user, SqlConnection de)
        {
            try
            {
                var getPropandVal = GetUpdatePropandVal(_user);
                //var query = "EXECUTE InsertOrUpdate " + _user.Id + ",Users,'" + getPropandVal + "'";
                var update = de.Query("EXECUTE InsertOrUpdate " + _user.Id + ",Users,'" + getPropandVal + "'");
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> DeleteUser(int id, SqlConnection de)
        {
            try
            {
                User u = GetActiveUserById(id, de);
                //var s = de.Skills.Where(a => a.UserId == id).ToList();
                //var t = de.Tags.Where(a => a.UserId == id).ToList();
                //var m = de.Messages.Where(a => a.SenderId == id || a.RecieverId==id && a.IsActive==1).ToList();
                //var r = de.Refferals.Where(x => x.RefferalCode == u.Refferal_Code).ToList();
                //if (s.Count() != 0)
                //{
                //    foreach (var x in s)
                //    {
                //        x.IsActive = 0;
                //        x.DeletedAt = DateTime.UtcNow;
                //        de.Skills.Update(x);
                //        de.SaveChanges();
                //    }
                   
                //}
                //if(m.Count()!=0)
                //{
                //    foreach(var mx in m)
                //    {
                //        mx.IsActive = 0;
                //        mx.DeletedAt = DateTime.UtcNow;
                //        de.Messages.Update(mx);
                //        de.SaveChanges();
                //    }
                //}
                //if (t.Count() != 0)
                //{
                //    foreach (var x in t)
                //    {
                //        x.IsActive = 0;
                //        x.DeletedAt = DateTime.UtcNow;
                //        de.Tags.Update(x);
                //        de.SaveChanges();
                //    }
                   
                //}

                //if (r.Count() != 0)
                //{
                //    foreach (var x in r)
                //    {
                //        x.IsActive = 0;
                //        x.DeletedAt = DateTime.UtcNow;
                //        de.Update(x);
                //        de.SaveChanges();
                //    }

                //}
                u.IsActive = 0;
                u.DeletedAt = DateTime.UtcNow;
                var getPropandVal = GetUpdatePropandVal(u);
                var delete = de.Query("EXECUTE InsertOrUpdate " + u.Id + ",Users," + getPropandVal + "");
                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool AddLog(Logging l, SqlConnection de)
        {
            try
            {
                var getPropandVal = GetPropandVal(l);
                var add = de.Query("EXECUTE InsertOrUpdate 0,Logging," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;
              
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteEducationById(int id, SqlConnection de)
        {
            try
            {
                if (id != -1)
                {
                    var useredu = GetEducationById(id, de);
                    if (useredu != null)
                    {
                        useredu.IsActive = 0;
                        useredu.DeletedAt = DateTime.UtcNow;
                        var getPropandVal = GetUpdatePropandVal(useredu);
                        var delete = de.Query("EXECUTE InsertOrUpdate " + useredu.Id + ",Education," + getPropandVal + "");
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


        public bool DeleteExperienceById(int id, SqlConnection de)
        {
            try
            {
                if (id != -1)
                {
                    var useredu = Getexperiencebyid(id, de);
                    if (useredu != null)
                    {
                        useredu.IsActive = 0;
                        useredu.Deletedat = DateTime.UtcNow;
                        var getPropandVal = GetUpdatePropandVal(useredu);
                        var delete = de.Query("EXECUTE InsertOrUpdate " + useredu.Id + ",Experience,'" + getPropandVal + "'");
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

        public int AddOrder(Order order, SqlConnection de)
        {
            try
            {
                var getPropandVal = GetPropandVal(order);
                var add = de.Query("EXECUTE InsertOrUpdate 0,Orders," + getPropandVal[0] + "," + getPropandVal[1] + "").First();
                return Convert.ToInt32(add.Id);
            }
            catch
            {
                return -1;
            }
        }

        public List<string> GetPropandVal(object obj)
        {
            try
            {
                var propAndval = new List<string>();
              
                PropertyInfo[] properties = obj.GetType().GetProperties();

                var prop = new List<string>();
                var val = new List<string>();
                
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetValue(obj) != null && !property.GetValue(obj).ToString().Contains("["))
                    {
                        prop.Add(property.Name);
                        val.Add("''"+property.GetValue(obj).ToString()+"''");
                    }
                }
                prop = prop.Skip(1).ToList();
                val = val.Skip(1).ToList();
                

                propAndval.Add("'"+String.Join(",", prop)+"'");
                propAndval.Add("'" +String.Join(",", val) + "'");


                return propAndval;
            }
            catch
            {
                return null;
            }
        }
        public string GetUpdatePropandVal(object obj)
        {
            try
            {
                 
                PropertyInfo[] properties = obj.GetType().GetProperties();

                var prop = new List<string>();
               
                
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetValue(obj)!=null && !property.GetValue(obj).ToString().Contains("[") && !property.GetValue(obj).ToString().Contains("Models"))
                    {
                        prop.Add(property.Name + " = ''" + property.GetValue(obj).ToString()+"''");
                    }
                }
                prop = prop.Skip(1).ToList();



                var getcolandval = String.Join(",", prop);

                return getcolandval;
            }
            catch
            {
                return null;
            }
        }

        #region

        public List<Testorder> GetActiveTaskList(SqlConnection de)
        {
            //List<Testorder> orders;

          return de.Query<Testorder>("EXECUTE GetAllRecords Test").ToList();


            
        }

        public List<Testorder> GetTaskList(SqlConnection de)
        {
            return de.Query<Testorder>("EXECUTE GetAllRecords Testorders").ToList();

        }


        //public List<Task> GetInactiveTaskList(Applicationdbcontext de)
        //{
        //    List<Task> Tasks;
        //    db = new DatabaseEntities();

        //    Tasks = db.Tasks.Where(x => x.IsActive == 0).ToList();

        //    return Tasks;
        //}

        public Testorder GetTaskById(int _Id, SqlConnection de)
        {

            return de.Query<Testorder>("EXECUTE GetAllRecords Testorders,Id," + _Id + "").FirstOrDefault();

        }

        public bool AddTask(Testorder _Task, SqlConnection de)
        {

            try
            {
                var getPropandVal = GetPropandVal(_Task);
                var add = de.Query("EXECUTE InsertOrUpdate 0,Testorders," + getPropandVal[0] + "," + getPropandVal[1] + "");
                return true;

            }
            catch
            {
                return false;
            }
        }

        public bool UpdateTask(Testorder _Task, SqlConnection de)
        {

            try
            {
                var getPropandVal = GetUpdatePropandVal(_Task);
                //var query = "EXECUTE InsertOrUpdate " + _user.Id + ",Users,'" + getPropandVal + "'";
                var update = de.Query("EXECUTE InsertOrUpdate " + _Task.Id + ",Testorders,'" + getPropandVal + "'");
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
