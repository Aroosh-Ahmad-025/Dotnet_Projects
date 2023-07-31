using Microsoft.AspNetCore.Http;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilkManagementSystem.HelpingClasses
{
    public class MailSender
    {

        //Install-Package RestSharp -Version 106.11.7
        public static bool SendForgotPasswordEmail(string email, int id, string BaseUrl = "")
        {
            try
            {
                string MailBody = "<html>" +
                    "<head></head>" +
                    "<body>" +
                    "<center>" + "<div> <h1 class='text-center' style='color:#000000'> Password Reset </h1> " +
                    "<p class='text-center' style='color:#000000'> " +
                          "You are Getting this Email Because You Requested To Reset Your Account Password.<br>Click the Button Below To Change Your Password" +
                    " </p>" +
                    "<p style='color:#000000' class='text-center'>" +
                            "If you did not request a password reset, Please Ignore This Email" +
                    "</p>" +
                    "<h3 style='color:#000000'>" + "Thanks" + "</h3>" +
                    "<br/>" +
                    "<button style='background-color: #CE2029; padding:12px 16px; border:1px solid #CE2029; border-radius:3px;'>" +
                            "<a href='" + ProjectVariables.baseUrl + "/Auth/ResetPassword?encId=" + StringCipher.EncryptId(id) + "&t=" + GeneralPurpose.DateTimeNow().Ticks + "' style='text-decoration:none; font-size:15px; color:white;'> Reset Password </a>" +
                    "</button>" +
                    "<p style='color:#FF0000'>Link will Expire after Date Change.<br>" +
                    "Link will not work in spam. Please move this mail into your inbox.</p>" +
                    "</div>" + "</center>" +
                            "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";


                RestClient client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator = new HttpBasicAuthenticator("api", "496e6c6979cb786921579085c5b07222-8d821f0c-bde767e8");

                RestRequest request = new RestRequest();
                request.AddParameter("domain", "usmandev.ca", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", ProjectVariables.FromEmail);
                request.AddParameter("to", email);
                request.AddParameter("subject", "Ez CarTrade | Password Reset");

                request.AddParameter("html", MailBody);

                request.Method = Method.POST;

                string response = client.Execute(request).Content.ToString();

                if (response.ToLower().Contains("queued"))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool ActivationEmail(string email)
        {
            try
            {
                string MailBody = "<html>" +
                    "<head></head>" +
                    "<body>" +
                    "<center>" + "<div> <h1 class='text-center' style='color:#000000'> Account Activation </h1> " +
                    "<p class='text-center' style='color:#000000'> " +
                          "You are Getting this Email Because You Have Created New Account On Our Platform.<br>Click the Button Below To Verify Your Email" +
                    " </p>" +
                    "<p style='color:#000000' class='text-center'>" +
                            "If you did not did this, Please Ignore This Email" +
                    "</p>" +
                    "<h3 style='color:#000000'>" + "Thanks" + "</h3>" +
                    "<br/>" +
                    "<button style='background-color:green;padding:12px 16px; border:1px solid green; border-radius:3px;'>" +
                            "<a href='" + ProjectVariables.baseUrl + "/Auth/AccountAcctivate?e=" + StringCipher.Base64Encode(email) +"&t=" + GeneralPurpose.DateTimeNow().Ticks + "' style='text-decoration:none; font-size:15px; background:green; color:white;'> Activate Account </a>" +
                    "</button>" +
                    "<p style='color:#FF0000'>Link will Expire after Date Change.<br>" +
                    "Link will not work in spam. Please move this mail into your inbox.</p>" +
                    "</div>" + "</center>" +
                            "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";


                RestClient client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator = new HttpBasicAuthenticator("api", "496e6c6979cb786921579085c5b07222-8d821f0c-bde767e8");

                RestRequest request = new RestRequest();
                request.AddParameter("domain", "usmandev.ca", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", ProjectVariables.FromEmail);
                request.AddParameter("to", email);
                request.AddParameter("subject", "Ez CarTrade | Account Activation");

                request.AddParameter("html", MailBody);

                request.Method = Method.POST;

                string response = client.Execute(request).Content.ToString();

                if (response.ToLower().Contains("queued"))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
         public static bool SendApprovalEmail(string email)
        {
            try
            {
                string MailBody = "<html>" +
                    "<head></head>" +
                    "<body>" +
                    "<center>" + "<div> <h1 class='text-center' style='color:#000000'> Congratulations! Your Account has been Approved by the Admin </h1> " +
                    "</center>" + "<center>"+
                    "<a role ='button' href='" + ProjectVariables.baseUrl + "/Home/Index" +"' style='text-decoration:none; background-color: green; padding:12px 16px; border-radius:3px;margin-bottom:47px; color:white'>"+"Go To Home"+ "</a>" +
                      "</center>" + "<br/>"+

                            "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";


                RestClient client = new RestClient(new Uri("https://api.mailgun.net/v3"));
                client.Authenticator = new HttpBasicAuthenticator("api", "496e6c6979cb786921579085c5b07222-8d821f0c-bde767e8");

                RestRequest request = new RestRequest();
                request.AddParameter("domain", "usmandev.ca", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", ProjectVariables.FromEmail);
                request.AddParameter("to", email);
                request.AddParameter("subject", "Ez CarTrade | Inventory Approval");

                request.AddParameter("html", MailBody);

                request.Method = Method.POST;
                 
                string response = client.ExecuteAsync(request).ToString();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
