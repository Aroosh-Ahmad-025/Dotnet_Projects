using LoginFinal.DAL;
using LoginFinal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginFinal.BL
{
    public class UserBL
    {

        //start massam 07-01-23
        public List<OrderReview> GetOrderReviewByUserId(int UserId, SqlConnection de)
        {
            return new UserDAL().GetOrderReviewByUserId(UserId, de);
        }

        public List<OrderReview> GetInternalReviewByUserId(int UserId, SqlConnection de)
        {
            return new UserDAL().GetInternalReviewByUserId(UserId, de);
        }
        //end massam 07-01-23
        public List<User> GetAllUsersList(SqlConnection de)
        {
            return new UserDAL().GetAllUsersList(de);
        }

        //Massam
        public List<Requesthelp> GetRequestsBySkills(string[] skills, SqlConnection de)
        {
            return new UserDAL().GetRequestsBySkills(skills, de);
        }
        public Education GetEducationById(int id, SqlConnection de)
        {
            return new UserDAL().GetEducationById(id, de);
        }
            public bool DeleteEducationById(int id, SqlConnection de)
        {
            return new UserDAL().DeleteEducationById(id, de);
        }

        public bool DeleteExperiencebyid(int id, SqlConnection de)
        {
            return new UserDAL().DeleteExperienceById(id, de);
        }
        public List<Logging> GetAllLogs(SqlConnection de)
        {
            return new UserDAL().GetAllLogs(de);
        }

        public List<Tags> GetAllTags(SqlConnection de)
        {
            return new UserDAL().GetAllTags(de);
        }
        public List<Skills> GetAllSkills(SqlConnection de)
        {
            return new UserDAL().GetAllSkills(de);

        }

        public List<Order> GetOrdersByUserId(int id, SqlConnection de)
        {
            return new UserDAL().GetOrdersById(id,de);
        }
        public List<Order> GetAllOrders(SqlConnection de)
        {
            return new UserDAL().GetAllOrders(de);
        }
        public List<Requesthelp> GetRequestsbyuserid(int userid, SqlConnection de)
        {
            return new UserDAL().GetRequestsbyuserid(userid,de);
        }
        public Requesthelp getLastrequestbyuserid(int userid, SqlConnection de)
        {
            return new UserDAL().getLastrequestbyuserid(userid,de);
        }

        public List<Requesthelp> GetRequestsbyuserskills(int userid, SqlConnection de)
        {

            return new UserDAL().GetRequestsbyuserskill(userid, de);

        }

        public List<Requestkills> GetRequestskillsbyrequestid(int id, SqlConnection de)
        {

            return new UserDAL().getrequestskillsbyrequestid(id, de);

        }

        public List<Requesthelp> GetRequestsbyuserTags(int userid, SqlConnection de)
        {


            return new UserDAL().GetRequestsbyuserTags(userid, de);



        }

        public List<Requesthelp> GetAllRequestss(SqlConnection de)
        {
            return new UserDAL().GetAllRequests(de);
        }
        public List<User> GetActiveUserList(SqlConnection de)
        {
            return new UserDAL().GetActiveUserList(de);
        }

        //public List<User> GetButlersList(string zip,string city,SqlConnection de)
        //{
        //    return new UserDAL().GetButlerList(zip,city,de);
        //}
        public List<User> GetButlersList(SqlConnection de)
        {
            return new UserDAL().GetButlerList(de);
        }

        public List<User> GetButlersListbyskill(string langs,string zip,string city, List<string> skills /*string skills*/, int requestid, SqlConnection de,string requesttype=null)
        {
            return new UserDAL().GetButlerListbyskills(langs,zip,city,skills, requestid, de, requesttype);
        }
        public List<User> GetButlersListbyTag(string zip, string city,/*string Tags,*/List<string> Tags, SqlConnection de,string requesttype)

        {
            return new UserDAL().GetButlerListbyTags(zip,city,Tags, de, requesttype);
        }
        public Requesthelp Getrequestbyrequestid(int id, SqlConnection de)
        {
            return new UserDAL().GetRequestsbyRequestid(id, de);
        }

        public User GetUserById(int id, SqlConnection de)
        {
            return new UserDAL().GetUserById(id, de);
        }

        public User GetActiveUserById(int id, SqlConnection de)
        {
            return new UserDAL().GetActiveUserById(id, de);
        }

        public List<Notification> Getnotificationcount(int id, SqlConnection de)
        {
            return new UserDAL().GetAllnotifications(id, de);
        }


        public async Task<bool> AddUser(User _user, SqlConnection de)
        {
            if (String.IsNullOrEmpty(_user.Username) || String.IsNullOrEmpty(_user.Email) || String.IsNullOrEmpty(_user.Password))
                return false;

            return await new UserDAL().AddUser(_user, de);
        }

        #region Requesthelp
        
            public async Task<bool> Addnotification(Notification _notif, SqlConnection de)
        {
            if (_notif == null)
            {
                return false;
            }


            return await new UserDAL().Addnotification(_notif, de);
        }
        //public async Task<bool> AddRequest(Requesthelp _req, SqlConnection de)
        public int AddRequest(Requesthelp _req, SqlConnection de)

        {
            if (_req==null)
            {
                return 0;
            }
                

            return new UserDAL().AddRequest(_req, de);
        }

        public Experience Getexperiencebyid(int Id, SqlConnection de)
        {
            return new UserDAL().Getexperiencebyid(Id, de);
        }
        public Requesthelp GetRequestbyid(int Id, SqlConnection de)
        {
            return new UserDAL().GetRequestsbyRequestid(Id, de);
        }
        public async Task<bool> DeleteRequest(int id, SqlConnection de)
        {
            return await new UserDAL().DeleteRequest(id,de);
        }

        public async Task<bool> DeleteRequestNotification(int reqid, SqlConnection de)
        {
            return await new UserDAL().DeleteRequestNotifications(reqid, de);
        }
        public async Task<bool> updaterequest(Requesthelp updrequest,List<string> skills,string Tags,string langs,List<string> preferservicetime, SqlConnection de)
        {
            return await new UserDAL().UpdateRequesthelp(updrequest, skills, Tags,langs,preferservicetime, de);
        }

        //massam
        public async Task<bool> UpdateRequestForEmailSend(Requesthelp updrequest, SqlConnection de)
        {
            return await new UserDAL().UpdateRequestForEmailSend(updrequest, de);
        }

        public Experience update_experience(Experience exp, SqlConnection de)
        {
            var upexp= new UserDAL().UpdateUserexperience(exp, de);
            return upexp;  
        }
        #endregion end request  help

        public bool AddExperience(Experience _exp, SqlConnection de)
        {
            if (_exp == null)
            {
                return false;
            }


            return new UserDAL().Adduserexperience(_exp, de);
        }

        public bool Addinternalreview(InternalReview _review, SqlConnection de)
        {
            if (_review == null)
            {
                return false;
            }


            return new UserDAL().addInternalReview(_review, de);
        }

        public InternalReview getInternalReviewbyId(int customerid, /*int orderid,*/ int userid, SqlConnection de)
        {
           
            return new UserDAL().getInternalReview(customerid, userid, de);
        }
        public bool AddRequestlanguage(UserLanguage _lang, SqlConnection de)
        {
            if (_lang == null)
            {
                return false;
            }


            return new UserDAL().AddRequestlanguages(_lang, de);
        }

        //public bool Addbutlerprefertime(butlerprefertime _pref, SqlConnection de)
        //{
        //    if (_pref == null)
        //    {
        //        return false;
        //    }


        //    return new UserDAL().Addbutlerprefertime(_pref, de);
        //}
        public bool AddUpdateButlerPreferTime(List<string> _pref,int userid, SqlConnection de)
        {
            if (_pref == null)
            {
                return false;
            }


            return new UserDAL().addUpdateButlerPreferTime(_pref, userid, de);
        }
        public bool AddUpdateButlerlanguage(List<string> _lngs, int userid, SqlConnection de)
        {
            if (_lngs == null)
            {
                return false;
            }


            return new UserDAL().addUpdateButlerlanguage(_lngs, userid, de);
        }

        public bool AddRequestprefertime(preferservicetime _pref, SqlConnection de)
        {
            if (_pref == null)
            {
                return false;
            }


            return new UserDAL().AddRequestservicetime(_pref, de);
        }

        //public Skills GetSkillsById(int Id , SqlConnection de)
        //{
        //    return new UserDAL().GetSkillsById(Id, de);
        //}

        public List<Skills> GetSkillsById(int Id, SqlConnection de) // masam changes
        {
            return new UserDAL().GetSkillsById(Id, de);
        }
        public List<Education> GetAllEducations(SqlConnection de)
        {
            return new UserDAL().GetAllEducations(de);
        }
        public List<UserLanguage> Getuserlanguagebyuserid(int userid,SqlConnection de)
        {
            return new UserDAL().Getuserlanguagebyid(userid,de);
        }

        public string Getbutlerprefertime(int userid,SqlConnection de)
        {
            return new UserDAL().Getbutlerprefertime(userid, de);
        }

        public List<Requestkills> Getallrequestskillbyid(int reqid,SqlConnection de)
        {
            return new UserDAL().Getallrequestskillbyid(reqid,de);
        }
        public List<RequestTags> GetallrequestTagsbyid(int reqid, SqlConnection de)
        {
            return new UserDAL().GetallrequestTagsbyid(reqid, de);
        }

        //public Tags GetTagsById(int Id, SqlConnection de)
        //{
        //    return new UserDAL().GetTagsById(Id, de);
        //}
        public List<Tags> GetTagsById(int Id, SqlConnection de)
        {
            return new UserDAL().GetTagsById(Id, de);
        }
        public List<Experience> Getexperiencebyuserid(int Id, SqlConnection de)
        {
            return new UserDAL().Getuserexperiencebyid(Id, de);
        }

        public async Task<bool> UpdateNotification(int id,int userid,int isdelete, SqlConnection de)
        {
            return await new UserDAL().UpdateNotifications(id, userid, isdelete, de);
        }
        public async Task<bool> UpdateUser(User _user, SqlConnection de)
        {
            return await new UserDAL().UpdateUser(_user, de);
        }

        public async Task<bool> DeleteUser(int id, SqlConnection de)
        {
            return await new UserDAL().DeleteUser(id, de);
        }


        public bool AddLog(Logging l,SqlConnection de)
        {
            return new UserDAL().AddLog(l,de);
        }

        public bool AddRefferal(Refferals l, SqlConnection de)
        {
            return new UserDAL().AddRefferal(l, de);
        }

        public int AddOrder(Order newOrder, SqlConnection de)
        {
            return new UserDAL().AddOrder(newOrder, de);
        }

        public bool AddFeedback(Feedback model, SqlConnection de)
        {
            return new UserDAL().AddFeedback(model, de);
        }
        public bool AddEducation(Education edu, SqlConnection de)
        {
            return new UserDAL().AddEducation(edu, de);
        }

        public bool AddSkills(Skills skills, SqlConnection de)
        {
            return new UserDAL().AddSkills(skills, de);
           
        }

        public bool AddRequestSkills(Requestkills skills, SqlConnection de)
        {
            return new UserDAL().AddRequestSkills(skills, de);

        }
        public bool AddRequestTags(RequestTags userTags, SqlConnection de)
        {
            return new UserDAL().AddRequestTags(userTags, de);
        }

        public bool AddTags(Tags userTags, SqlConnection de)
        {
            return new UserDAL().AddTags(userTags, de);
        }


        public Order GetOrderById(int id, SqlConnection de)
        {
            return new UserDAL().GetOrderById(id, de);
        }
        public bool UpdateOrder(Order order, SqlConnection de)
        {
            return new UserDAL().UpdateOrder(order, de);
        }

        public bool AddReview(OrderReview review, SqlConnection de)
        {
            return new UserDAL().AddReview(review, de);
        }

        public Task<string> UploadImage(IFormFile ImagePath)
        {
            return new UserDAL().FileUpload(ImagePath);
        }

        public OrderReview GetReviewById(int id, SqlConnection de)
        {
            return new UserDAL().GetReviewById(id,de);
        }

        public bool UpdateReview(OrderReview getReview, SqlConnection de)
        {
            return new UserDAL().UpdateReview(getReview, de);
        }

        public List<OrderReview> GetAllReviews(SqlConnection de)
        {
            return new UserDAL().GetAllReviews(de);
        }

        public bool UpdateEducation(Education n, SqlConnection de)
        {
            return new UserDAL().UpdateEducation(n,de);
        }

        public List<Refferals> GetAllReferrals(SqlConnection de)
        {
            return new UserDAL().GetAllRefferals( de);
        }

        public List<Feedback> GetAllFeedBacks(SqlConnection de)
        {
            return new UserDAL().GetAllFeedBacks(de);
        }


        #region calendar 

        public List<Testorder> GetTaskList(SqlConnection de)
        {
            return new UserDAL().GetTaskList(de);
        }

        public List<Testorder> GetActiveTaskList(SqlConnection de)
        {
            return new UserDAL().GetActiveTaskList(de);
        }

        //public List<order> GetInactiveTaskList()
        //{
        //    return new UserDAL().GetInactiveTaskList();
        //}

        public Testorder GetTaskbyId(int _id, SqlConnection de)
        {
            return new UserDAL().GetTaskById(_id, de);
        }

        public bool AddTask(Testorder _Task, SqlConnection de)
        {
            if (_Task.OrderTitle == null || _Task.OrderDescription == null || _Task.StartDate == null ||
                _Task.EndDate == null)
            {
                return false;
            }

            return new UserDAL().AddTask(_Task, de);
        }

        public bool UpdateTask(Testorder _Task, SqlConnection de)
        {
            if (_Task.Id == -1 || _Task.OrderTitle == null || _Task.OrderDescription == null || _Task.StartDate == null ||
                _Task.EndDate == null)
            {
                return false;
            }

            return new UserDAL().UpdateTask(_Task, de);
        }



        #endregion calendar



        // Start massam (28-12-22)
        public InternalReview getInternalReviewByUserIds(int customerid, /*int orderid,*/ int userid, SqlConnection de)
        {
            return new UserDAL().getInternalReviewByUserIds(customerid, userid, de);
        }
        public InternalReview getInternalReviewByOrderId(int oredrId, SqlConnection de)
        {
            return new UserDAL().getInternalReviewByOrderId(oredrId, de);
        }
        // End massam (28-12-22)
        //massam (26-12-22)
        public Refferals GetReferralByButlerId(int butlerId, SqlConnection de)
        {
            return new UserDAL().GetReferralByButlerId(butlerId, de);
        }
        public User GetUserReccordByReferralCode(string referralCode, SqlConnection de)
        {
            return new UserDAL().GetUserReccordByReferralCode(referralCode, de);
        }
        public List<Refferals> GetButlersReferralReccordByReferralCode(string referralCode, SqlConnection de)
        {
            return new UserDAL().GetButlersReferralReccordByReferralCode(referralCode, de);
        }
        //End massam (26-12-22)



    }
}
