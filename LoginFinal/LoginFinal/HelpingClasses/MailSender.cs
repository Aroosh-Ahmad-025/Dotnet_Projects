using Microsoft.AspNetCore.Http;
using RestSharp;
using RestSharp.Authenticators;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;
using Microsoft.CodeAnalysis.Differencing;
using System.Drawing;

namespace LoginFinal.HelpingClasses
{
    public class MailSender
    {

        //Install-Package RestSharp -Version 106.11.7
        public static bool SendForgotPasswordEmail(string email, int id, string BaseUrl = "")
        {
            try
            {
                //string MailBody = "<html>" +
                //    "<head></head>" +
                //    "<body>" +
                //    "<center>" + "<div> <h1 class='text-center' style='color:#000000'> Password Reset </h1> " +
                //    "<p class='text-center' style='color:#000000'> " +
                //          "You are Getting this Email Because You Requested To Reset Your Account Password.<br>Click the Button Below To Change Your Password" +
                //    " </p>" +
                //    "<p style='color:#000000' class='text-center'>" +
                //            "If you did not request a password reset, Please Ignore This Email" +
                //    "</p>" +
                //    "<h3 style='color:#000000'>" + "Thanks" + "</h3>" +
                //    "<br/>" +
                //    "<button style='background-color: #CE2029; padding:12px 16px; border:1px solid #CE2029; border-radius:3px;'>" +
                //            "<a href='" + ProjectVariables.baseUrl + "/Auth/ResetPassword?encId=" + StringCipher.EncryptId(id) + "&t=" + GeneralPurpose.DateTimeNow().Ticks + "' style='text-decoration:none; font-size:15px; color:white;'> Reset Password </a>" +
                //    "</button>" +
                //    "<p style='color:#FF0000'>Link will Expire after Date Change.<br>" +
                //    "Link will not work in spam. Please move this mail into your inbox.</p>" +
                //    "</div>" + "</center>" +
                //            "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";




                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot/EmailTemp/emailTemp.txt");
                string MailBody = System.IO.File.ReadAllText(path) +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                    "<tr>" +
                                      "<td height='50'></td>" +
                                    "<tr>" +
                                      "<td align='center' style='line-height: 0px;'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/butlerlogo.png' alt='img' width='180px'/></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                      "</table>" +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#ECEFF3'>" +
                        //"<tr>" +
                        //  "<td height='70'></td >" +
                        //"</tr>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width ='520' align='center'>" +
                                  "<table class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                      "<tr>" +
                                        "<td height='50' align='center' bgcolor='#6EC8C7' style='border-top-left-radius:6px; border-top-right-radius: 6px;font-family: 'Open sans', Arial, sans-serif; color:#FFFFFF; font-size:20px;font-weight: bold;' > I.T Butler Notification</td>" +
                                      "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                    "</table>" +
                    "<table class='full' align='center' bgcolor='#ECEFF3' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table align='center' class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                    "<tr>" +
                                      "<td style='border-bottom-left-radius:6px;border-bottom-right-radius:6px; box-shadow:0px 3px 0px #E0E5EB;' bgcolor='#FFFFFF' align='center'>" +
                                        "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                          "<tr>" +
                                            "<td height='35'></td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#3b3b3b; font-size:22px;font-weight: bold; line-height: 28px;'>Password Reset</td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td height='4'></td>" +
                                          "</tr>" +
                                          "<tr><td><hr style='border: 0.5px solid #6ec8c7b0;'></td></tr>" +
                                          "<tr>" +
                                            "<td style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:14px; line-height: 28px; margin-top: 10px;'>" +
                                                "<p>Dear User,</p>" +
                                                "<p>You are Getting this Email Because You Requested To Reset Your Account Password.<br>Click the Button Below To Change Your Password</p>" +
                                                "<p style='color:#FF0000'>Button link will Expire after Date Change.<br>" +
                                                "Button will not work in spam. Please move this mail into your inbox.</p>" +
                                                "<p style='color:#000000' class='text-center'>" +
                                                    "If you did not request a password reset, please ignore this email" +
                                                "</p>" +
                                                "Thanks,<br>" +
                                                "The IT Butler Team." +
                                            "</td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='30'></td>" +
                                         "</tr>" +



                                         "<tr>" +
                                            "<td align='center'>" +
                                              "<table class='textbutton' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                                "<tr>" +
                                                  "<td bgcolor='#6ec8c7' style='border-radius:6px;' align='center'>" +
                                                    "<table border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                                      "<tr>" +
                                                        "<td class='btn-link' height='45' align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#FFFFFF; font-size:16px;padding-left: 35px;padding-right: 35px; font-weight: bold;'><a href='" + ProjectVariables.baseUrl + "/Auth/ResetPassword?encId=" + StringCipher.EncryptId(id) + "&t=" + GeneralPurpose.DateTimeNow().Ticks + "'>" + "Reset Password" + "</a></td>" +
                                                      "</tr>" +
                                                      "<tr>" +
                                                        "<td height='0'></td>" +
                                                      "</tr>" +
                                                    "</table>" +
                                                  "</td>" +
                                                "</tr>" +
                                              "</table>" +
                                            "</td>" +
                                          "</tr>" +


                                         "<tr>" +
                                            "<td height='5'></td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='10'></td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#a7aeb3; font-size:13px; line-height: 20px;font-style: italic;'> *This email was intended for Usman, because you signed up for IT Butler | The links in this email will always direct to <a href='https://Itbutler.com/' target='_blank'> https://Itbutler.com/</a>." +
                                              "<p>© IT Butler International Ltd. 2023 </p></td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td height='5'></td>" +
                                          "</tr>" +
                                        "</table>" +
                                      "</td>" +
                                    "</tr>" +
                                  "</table>" +
                                  "<table align='center' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                    "<tr>" +
                                      "<td height='20'></td>" +
                                    "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                      //"<tr>" +
                      //  "<td height='50'></td>" +
                      //"</tr>" +
                      "</table>" +

                      "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table align='center' class='table-inner' width='90%' border='0' cellspacing='0' cellpadding='0'>" +
                                    "<tr>" +
                                      "<td align='center'>" +
                                        "<table border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                          "<tr>" +
                                            "<td align='center' style='line-height: 0px;'>" +
                                              "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/fb.png' alt='img'/> </a>" +
                                            "</td>" +
                                            "<td width='15'></td>" +
                                            "<td align='center' style='line-height: 0px;'>" +
                                              "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/tw.png' alt='img'/></a>" +
                                            "</td>" +
                                            "<td width='15'></td>" +
                                            "<td align='center' style='line-height: 0px;'>" +
                                              "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/gg.png' alt='img'/></a>" +
                                            "</td>" +
                                            "<td width='15'></td>" +
                                            "<td align='center' style='line-height: 0px;'>" +
                                              "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/in.png' alt='img'/></a>" +
                                            "</td>" +
                                          "</tr>" +
                                          "</table>" +
                                      "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='0'></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:12px; line-height: 28px;'><a href='#'>Webversion</a><span>&nbsp;&nbsp;|&nbsp;&nbsp;</span><a href='#'>Download App</a></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='55'></td>" +
                                    "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                      "</table>" +
                "</body>" +
            "</html>";







                RestClient client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator = new HttpBasicAuthenticator("api", "496e6c6979cb786921579085c5b07222-8d821f0c-bde767e8");

                RestRequest request = new RestRequest();
                request.AddParameter("domain", "usmandev.ca", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", ProjectVariables.FromEmail);
                request.AddParameter("to", email);
                request.AddParameter("subject", "IT Butler | Password Reset");

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
                //string MailBody = "<html>" +
                //    "<head></head>" +
                //    "<body>" +
                //    "<center>" + "<div> <h1 class='text-center' style='color:#000000'> Account Activation </h1> " +
                //    "<p class='text-center' style='color:#000000'> " +
                //          "You are Getting this Email Because You Have Created New Account On Our Platform.<br>Click the Button Below To Verify Your Email" +
                //    " </p>" +
                //    "<p style='color:#000000' class='text-center'>" +
                //            "If you did not did this, Please Ignore This Email" +
                //    "</p>" +
                //    "<h3 style='color:#000000'>" + "Thanks" + "</h3>" +
                //    "<br/>" +
                //    "<button style='background-color:green;padding:12px 16px; border:1px solid green; border-radius:3px;'>" +
                //            "<a href='" + ProjectVariables.baseUrl + "/Auth/AccountAcctivate?e=" + StringCipher.Base64Encode(email) + "&t=" + GeneralPurpose.DateTimeNow().Ticks + "' style='text-decoration:none; font-size:15px; background:green; color:white;'> Activate Account </a>" +
                //    "</button>" +
                //    "<p style='color:#FF0000'>Link will Expire after Date Change.<br>" +
                //    "Link will not work in spam. Please move this mail into your inbox.</p>" +
                //    "</div>" + "</center>" +
                //            "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";


                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot/EmailTemp/emailTemp.txt");
                string MailBody = System.IO.File.ReadAllText(path) +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                    "<tr>" +
                                      "<td height='50'></td>" +
                                    "<tr>" +
                                      "<td align='center' style='line-height: 0px;'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/butlerlogo.png' alt='img' width='180px'/></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                      "</table>" +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#ECEFF3'>" +
                        //"<tr>" +
                        //  "<td height='70'></td >" +
                        //"</tr>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width ='520' align='center'>" +
                                  "<table class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                      "<tr>" +
                                        "<td height='50' align='center' bgcolor='#6EC8C7' style='border-top-left-radius:6px; border-top-right-radius: 6px;font-family: 'Open sans', Arial, sans-serif; color:#FFFFFF; font-size:20px;font-weight: bold;' > I.T Butler Notification</td>" +
                                      "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                    "</table>" +
                    "<table class='full' align='center' bgcolor='#ECEFF3' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table align='center' class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                    "<tr>" +
                                      "<td style='border-bottom-left-radius:6px;border-bottom-right-radius:6px; box-shadow:0px 3px 0px #E0E5EB;' bgcolor='#FFFFFF' align='center'>" +
                                        "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                          "<tr>" +
                                            "<td height='35'></td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#3b3b3b; font-size:22px;font-weight: bold; line-height: 28px;'>Account Activation</td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td height='4'></td>" +
                                          "</tr>" +
                                          "<tr><td><hr style='border: 0.5px solid #6ec8c7b0;'></td></tr>" +
                                          "<tr>" +
                                            "<td style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:14px; line-height: 28px; margin-top: 10px;'>" +
                                                "<p>Dear User,</p>" +
                                                "<p>You are Getting this Email Because You Have Created New Account On Our Platform.<br>Click the Button Below To Verify Your Email</p>" +
                                                "<p style='color:#FF0000'>Button link will Expire after Date Change.<br>" +
                                                "Button will not work in spam. Please move this mail into your inbox.</p>" +
                                                "<p style='color:#000000' class='text-center'>" +
                                                    "If you did not did this, please ignore this email" +
                                                "</p>" +
                                                "Thanks,<br>" +
                                                "The IT Butler Team." +
                                            "</td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='30'></td>" +
                                         "</tr>" +



                                         "<tr>" +
                                            "<td align='center'>" +
                                              "<table class='textbutton' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                                "<tr>" +
                                                  "<td bgcolor='#6ec8c7' style='border-radius:6px;' align='center'>" +
                                                    "<table border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                                      "<tr>" +
                                                        "<td class='btn-link' height='45' align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#FFFFFF; font-size:16px;padding-left: 35px;padding-right: 35px; font-weight: bold;'><a href='" + ProjectVariables.baseUrl + "/Auth/AccountAcctivate?e=" + StringCipher.Base64Encode(email) +"&t=" + GeneralPurpose.DateTimeNow().Ticks + "'>" + "Activate Account" + "</a></td>" +
                                                      "</tr>" +
                                                      "<tr>" +
                                                        "<td height='0'></td>" +
                                                      "</tr>" +
                                                    "</table>" +
                                                  "</td>" +
                                                "</tr>" +
                                              "</table>" +
                                            "</td>" +
                                          "</tr>" +


                                         "<tr>" +
                                            "<td height='5'></td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='10'></td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#a7aeb3; font-size:13px; line-height: 20px;font-style: italic;'> *This email was intended for Usman, because you signed up for IT Butler | The links in this email will always direct to <a href='https://Itbutler.com/' target='_blank'> https://Itbutler.com/</a>." +
                                              "<p>© IT Butler International Ltd. 2023 </p></td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td height='5'></td>" +
                                          "</tr>" +
                                        "</table>" +
                                      "</td>" +
                                    "</tr>" +
                                  "</table>" +
                                  "<table align='center' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                    "<tr>" +
                                      "<td height='20'></td>" +
                                    "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                      //"<tr>" +
                      //  "<td height='50'></td>" +
                      //"</tr>" +
                      "</table>" +

                      "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table align='center' class='table-inner' width='90%' border='0' cellspacing='0' cellpadding='0'>" +
                                    "<tr>" +
                                      "<td align='center'>" +
                                        "<table border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                          "<tr>" +
                                            "<td align='center' style='line-height: 0px;'>" +
                                              "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/fb.png' alt='img'/> </a>" +
                                            "</td>" +
                                            "<td width='15'></td>" +
                                            "<td align='center' style='line-height: 0px;'>" +
                                              "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/tw.png' alt='img'/></a>" +
                                            "</td>" +
                                            "<td width='15'></td>" +
                                            "<td align='center' style='line-height: 0px;'>" +
                                              "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/gg.png' alt='img'/></a>" +
                                            "</td>" +
                                            "<td width='15'></td>" +
                                            "<td align='center' style='line-height: 0px;'>" +
                                              "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/in.png' alt='img'/></a>" +
                                            "</td>" +
                                          "</tr>" +
                                          "</table>" +
                                      "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='0'></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:12px; line-height: 28px;'><a href='#'>Webversion</a><span>&nbsp;&nbsp;|&nbsp;&nbsp;</span><a href='#'>Download App</a></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='55'></td>" +
                                    "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                      "</table>" +
                "</body>" +
            "</html>";




                RestClient client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator = new HttpBasicAuthenticator("api", "496e6c6979cb786921579085c5b07222-8d821f0c-bde767e8");

                RestRequest request = new RestRequest();
                request.AddParameter("domain", "usmandev.ca", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", ProjectVariables.FromEmail);
                request.AddParameter("to", email);
                request.AddParameter("subject", "IT Butler | Account Activation");

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
                //string MailBody = "<html>" +
                //    "<head></head>" +
                //    "<body>" +
                //    "<center>" + "<div> <h1 class='text-center' style='color:#000000'> Congratulations! Your Account has been Approved by the Admin </h1> " +
                //    "</center>" + "<center>"+
                //    "<a role ='button' href='" + ProjectVariables.baseUrl + "/Home/Index" +"' style='text-decoration:none; background-color: green; padding:12px 16px; border-radius:3px;margin-bottom:47px; color:white'>"+"Go To Home"+ "</a>" +
                //      "</center>" + "<br/>"+

                //            "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";



                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot/EmailTemp/emailTemp.txt");
                string MailBody = System.IO.File.ReadAllText(path) +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                    "<tr>" +
                                      "<td height='50'></td>" +
                                    "<tr>" +
                                      "<td align='center' style='line-height: 0px;'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/butlerlogo.png' alt='img' width='180px'/></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                      "</table>" +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#ECEFF3'>" +
                        //"<tr>" +
                        //  "<td height='70'></td >" +
                        //"</tr>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width ='520' align='center'>" +
                                  "<table class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                      "<tr>" +
                                        "<td height='50' align='center' bgcolor='#6EC8C7' style='border-top-left-radius:6px; border-top-right-radius: 6px;font-family: 'Open sans', Arial, sans-serif; color:#FFFFFF; font-size:20px;font-weight: bold;' > I.T Butler Notification</td>" +
                                      "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                    "</table>" +
                    "<table class='full' align='center' bgcolor='#ECEFF3' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table align='center' class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                    "<tr>" +
                                      "<td style='border-bottom-left-radius:6px;border-bottom-right-radius:6px; box-shadow:0px 3px 0px #E0E5EB;' bgcolor='#FFFFFF' align='center'>" +
                                        "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                          "<tr>" +
                                            "<td height='35'></td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#3b3b3b; font-size:22px;font-weight: bold; line-height: 28px;'>Account Approval</td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td height='4'></td>" +
                                          "</tr>" +
                                          "<tr><td><hr style='border: 0.5px solid #6ec8c7b0;'></td></tr>" +
                                          "<tr>" +
                                            "<td style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:14px; line-height: 28px; margin-top: 10px;'>" +
                                                "<p>Dear User,</p>" +
                                                "<p>Your profile is matched against the Customer request, reach him out asap to avail the offer.</p>" +
                                                "Thanks,<br>" +
                                                "The IT Butler Team." +
                                            "</td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='30'></td>" +
                                         "</tr>" +



                                         "<tr>" +
                                            "<td align='center'>" +
                                              "<table class='textbutton' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                                "<tr>" +
                                                  "<td bgcolor='#6ec8c7' style='border-radius:6px;' align='center'>" +
                                                    "<table border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                                      "<tr>" +
                                                        "<td class='btn-link' height='45' align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#FFFFFF; font-size:16px;padding-left: 35px;padding-right: 35px; font-weight: bold;'><a href='\" + ProjectVariables.baseUrl + \"/Home/Index\" +\"'>" + "Go To Home" + "</a></td>" +
                                                      "</tr>" +
                                                      "<tr>" +
                                                        "<td height='0'></td>" +
                                                      "</tr>" +
                                                    "</table>" +
                                                  "</td>" +
                                                "</tr>" +
                                              "</table>" +
                                            "</td>" +
                                          "</tr>" +


                                         "<tr>" +
                                            "<td height='5'></td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='10'></td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#a7aeb3; font-size:13px; line-height: 20px;font-style: italic;'> *This email was intended for Usman, because you signed up for IT Butler | The links in this email will always direct to <a href='https://Itbutler.com/' target='_blank'> https://Itbutler.com/</a>." +
                                              "<p>© IT Butler International Ltd. 2023 </p></td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td height='5'></td>" +
                                          "</tr>" +
                                        "</table>" +
                                      "</td>" +
                                    "</tr>" +
                                  "</table>" +
                                  "<table align='center' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                    "<tr>" +
                                      "<td height='20'></td>" +
                                    "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                        //"<tr>" +
                        //  "<td height='50'></td>" +
                        //"</tr>" +
                      "</table>" +

                      "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table align='center' class='table-inner' width='90%' border='0' cellspacing='0' cellpadding='0'>" +
                                    "<tr>" +
                                      "<td align='center'>" +
                                        "<table border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                          "<tr>" +
                                            "<td align='center' style='line-height: 0px;'>" +
                                              "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/fb.png' alt='img'/> </a>" +
                                            "</td>" +
                                            "<td width='15'></td>" +
                                            "<td align='center' style='line-height: 0px;'>" +
                                              "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/tw.png' alt='img'/></a>" +
                                            "</td>" +
                                            "<td width='15'></td>" +
                                            "<td align='center' style='line-height: 0px;'>" +
                                              "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/gg.png' alt='img'/></a>" +
                                            "</td>" +
                                            "<td width='15'></td>" +
                                            "<td align='center' style='line-height: 0px;'>" +
                                              "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/in.png' alt='img'/></a>" +
                                            "</td>" +
                                          "</tr>" +
                                          "</table>" +
                                      "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='0'></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:12px; line-height: 28px;'><a href='#'>Webversion</a><span>&nbsp;&nbsp;|&nbsp;&nbsp;</span><a href='#'>Download App</a></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='55'></td>" +
                                    "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                      "</table>" +
                "</body>" +
            "</html>";



                RestClient client = new RestClient(new Uri("https://api.mailgun.net/v3"));
                client.Authenticator = new HttpBasicAuthenticator("api", "496e6c6979cb786921579085c5b07222-8d821f0c-bde767e8");

                RestRequest request = new RestRequest();
                request.AddParameter("domain", "usmandev.ca", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", ProjectVariables.FromEmail);
                request.AddParameter("to", email);
                request.AddParameter("subject", "IT Butler | Account Approval");

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

        //by nmasam
        public static bool CustomerRequestMailToButlers(string email)
        {
            try
            {
                //string MailBody = "<html>" +
                //    "<head></head>" +
                //    "<body>" +
                //    "<center>" + "<div> <h1 class='text-center' style='color:#000000'> Your profile is matched against the Customer request, reach him out asap to avail the offer. </h1> " +
                //    "</center>" + "<center>" +
                //      //"<a role ='button' href='" + ProjectVariables.baseUrl + "/Home/CustomerRequests?userrole=3" + "' style='text-decoration:none; background-color: green; padding:12px 16px; border-radius:3px;margin-bottom:47px; color:white'>" + "Go To Home" + "</a>" +
                //      "</center>" + "<br/>" +
                //            "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";


                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot/EmailTemp/emailTemp.txt");
                string MailBody = System.IO.File.ReadAllText(path) +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                    "<tr>" +
                                      "<td height='50'></td>" +
                                    "<tr>" +
                                      "<td align='center' style='line-height: 0px;'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/butlerlogo.png' alt='img' width='180px'/></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                      "</table>" +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#ECEFF3'>" +
                        //"<tr>" +
                        //  "<td height='70'></td >" +
                        //"</tr>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width ='520' align='center'>" +
                                  "<table class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                      "<tr>" +
                                        "<td height='50' align='center' bgcolor='#6EC8C7' style='border-top-left-radius:6px; border-top-right-radius: 6px;font-family: 'Open sans', Arial, sans-serif; color:#FFFFFF; font-size:20px;font-weight: bold;' > I.T Butler Notification</td>" +
                                      "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                    "</table>" +
                    "<table class='full' align='center' bgcolor='#ECEFF3' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table align='center' class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                    "<tr>" +
                                      "<td style='border-bottom-left-radius:6px;border-bottom-right-radius:6px; box-shadow:0px 3px 0px #E0E5EB;' bgcolor='#FFFFFF' align='center'>" +
                                        "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                          "<tr>" +
                                            "<td height='35'></td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#3b3b3b; font-size:22px;font-weight: bold; line-height: 28px;'>Request Matched</td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td height='4'></td>" +
                                          "</tr>" +
                                          "<tr><td><hr style='border: 0.5px solid #6ec8c7b0;'></td></tr>" +
                                          "<tr>" +
                                            "<td style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:14px; line-height: 28px; margin-top: 10px;'>" +
                                                "<p>Dear User,</p>" +
                                                "<p>Your profile is matched against the Customer request, reach him out asap to avail the offer.</p>" +
                                                "Thanks,<br>" +
                                                "The IT Butler Team." +
                                            "</td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='30'></td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='5'></td>" +
                                         "</tr>" +
                                         "<tr>" +
                                    "<td height='10'></td>" +
                                  "</tr>" +
                                  "<tr>" +
                                    "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#a7aeb3; font-size:13px; line-height: 20px;font-style: italic;'> *This email was intended for Usman, because you signed up for IT Butler | The links in this email will always direct to <a href='https://Itbutler.com/' target='_blank'> https://Itbutler.com/</a>." +
                                      "<p>© IT Butler International Ltd. 2023 </p></td>" +
                                  "</tr>" +
                                  "<tr>" +
                                    "<td height='5'></td>" +
                                  "</tr>" +
                                "</table>" +
                              "</td>" +
                            "</tr>" +
                          "</table>" +
                          "<table align='center' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                            "<tr>" +
                              "<td height='20'></td>" +
                            "</tr>" +
                          "</table>" +
                        "</td>" +
                      "</tr>" +
                    "</table>" +
                  "</td>" +
                "</tr>" +
                //"<tr>" +
                //  "<td height='50'></td>" +
                //"</tr>" +
              "</table>" +

              "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                "<tr>" +
                  "<td align='center'>" +
                    "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                      "<tr>" +
                        "<td width='520' align='center'>" +
                          "<table align='center' class='table-inner' width='90%' border='0' cellspacing='0' cellpadding='0'>" +
                            "<tr>" +
                              "<td align='center'>" +
                                "<table border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                  "<tr>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/fb.png' alt='img'/> </a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/tw.png' alt='img'/></a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/gg.png' alt='img'/></a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/in.png' alt='img'/></a>" +
                                    "</td>" +
                                  "</tr>" +
                                  "</table>" +
                              "</td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td height='0'></td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:12px; line-height: 28px;'><a href='#'>Webversion</a><span>&nbsp;&nbsp;|&nbsp;&nbsp;</span><a href='#'>Download App</a></td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td height='55'></td>" +
                            "</tr>" +
                          "</table>" +
                        "</td>" +
                      "</tr>" +
                    "</table>" +
                  "</td>" +
                "</tr>" +
              "</table>" +
        "</body>" +
    "</html>";




                RestClient client = new RestClient(new Uri("https://api.mailgun.net/v3"));
                client.Authenticator = new HttpBasicAuthenticator("api", "496e6c6979cb786921579085c5b07222-8d821f0c-bde767e8");
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "usmandev.ca", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", ProjectVariables.FromEmail);
                request.AddParameter("to", email);
                request.AddParameter("subject", "IT Butler | Customer Request");
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
        public static bool MatchingButlersMailToCustomer(string email)
        {
            try
            {
                //string MailBody = "<html>" +
                //    "<head></head>" +
                //    "<body>" +
                //    "<center>" + "<div> <h1 class='text-center' style='color:#000000'> Some butlers matched against your request, view them to place an order. </h1> " +
                //    "</center>" + "<center>" +
                //      //"<a role ='button' href='" + ProjectVariables.baseUrl + "/Home/CustomerRequests?userrole=4" + "' style='text-decoration:none; background-color: green; padding:12px 16px; border-radius:3px;margin-bottom:47px; color:white'>" + "Go To Home" + "</a>" +
                //      "</center>" + "<br/>" +
                //            "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";


                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot/EmailTemp/emailTemp.txt");
                string MailBody = System.IO.File.ReadAllText(path) +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                    "<tr>" +
                                      "<td height='50'></td>" +
                                    "<tr>" +
                                      "<td align='center' style='line-height: 0px;'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/butlerlogo.png' alt='img' width='180px'/></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                      "</table>" +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#ECEFF3'>" +
                        //"<tr>" +
                        //  "<td height='70'></td >" +
                        //"</tr>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width ='520' align='center'>" +
                                  "<table class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                      "<tr>" +
                                        "<td height='50' align='center' bgcolor='#6EC8C7' style='border-top-left-radius:6px; border-top-right-radius: 6px;font-family: 'Open sans', Arial, sans-serif; color:#FFFFFF; font-size:20px;font-weight: bold;' > I.T Butler Notification</td>" +
                                      "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                    "</table>" +
                    "<table class='full' align='center' bgcolor='#ECEFF3' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table align='center' class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                    "<tr>" +
                                      "<td style='border-bottom-left-radius:6px;border-bottom-right-radius:6px; box-shadow:0px 3px 0px #E0E5EB;' bgcolor='#FFFFFF' align='center'>" +
                                        "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                          "<tr>" +
                                            "<td height='35'></td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#3b3b3b; font-size:22px;font-weight: bold; line-height: 28px;'>Request Matched</td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td height='4'></td>" +
                                          "</tr>" +
                                          "<tr><td><hr style='border: 0.5px solid #6ec8c7b0;'></td></tr>" +
                                          "<tr>" +
                                            "<td style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:14px; line-height: 28px; margin-top: 10px;'>" +
                                                "<p>Dear User,</p>" +
                                                "<p>Some butlers matched against your request.</p>" +
                                                "<p>Kindly view them to place an order.</p>" +
                                                "Thanks,<br>" +
                                                "The IT Butler Team." +
                                            "</td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='30'></td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='5'></td>" +
                                         "</tr>" +
                                         "<tr>" +
                                    "<td height='10'></td>" +
                                  "</tr>" +
                                  "<tr>" +
                                    "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#a7aeb3; font-size:13px; line-height: 20px;font-style: italic;'> *This email was intended for Usman, because you signed up for IT Butler | The links in this email will always direct to <a href='https://Itbutler.com/' target='_blank'> https://Itbutler.com/</a>." +
                                      "<p>© IT Butler International Ltd. 2023 </p></td>" +
                                  "</tr>" +
                                  "<tr>" +
                                    "<td height='5'></td>" +
                                  "</tr>" +
                                "</table>" +
                              "</td>" +
                            "</tr>" +
                          "</table>" +
                          "<table align='center' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                            "<tr>" +
                              "<td height='20'></td>" +
                            "</tr>" +
                          "</table>" +
                        "</td>" +
                      "</tr>" +
                    "</table>" +
                  "</td>" +
                "</tr>" +
                //"<tr>" +
                //  "<td height='50'></td>" +
                //"</tr>" +
              "</table>" +

              "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                "<tr>" +
                  "<td align='center'>" +
                    "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                      "<tr>" +
                        "<td width='520' align='center'>" +
                          "<table align='center' class='table-inner' width='90%' border='0' cellspacing='0' cellpadding='0'>" +
                            "<tr>" +
                              "<td align='center'>" +
                                "<table border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                  "<tr>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/fb.png' alt='img'/> </a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/tw.png' alt='img'/></a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/gg.png' alt='img'/></a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/in.png' alt='img'/></a>" +
                                    "</td>" +
                                  "</tr>" +
                                  "</table>" +
                              "</td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td height='0'></td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:12px; line-height: 28px;'><a href='#'>Webversion</a><span>&nbsp;&nbsp;|&nbsp;&nbsp;</span><a href='#'>Download App</a></td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td height='55'></td>" +
                            "</tr>" +
                          "</table>" +
                        "</td>" +
                      "</tr>" +
                    "</table>" +
                  "</td>" +
                "</tr>" +
              "</table>" +
        "</body>" +
    "</html>";


                RestClient client = new RestClient(new Uri("https://api.mailgun.net/v3"));
                client.Authenticator = new HttpBasicAuthenticator("api", "496e6c6979cb786921579085c5b07222-8d821f0c-bde767e8");
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "usmandev.ca", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", ProjectVariables.FromEmail);
                request.AddParameter("to", email);
                request.AddParameter("subject", "IT Butler | Request");
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


        public static bool OrderPlacing(string email)
        {
            try
            {
                //string MailBody = "<html>" +
                //    "<head></head>" +
                //    "<body>" +
                //    "<center>" + "<div> <h1 class='text-center' style='color:#000000'> An order is placed against your profile. </h1> " +
                //    "</center>" + "<center>" +
                //      //"<a role ='button' href='" + ProjectVariables.baseUrl + "/Home/CustomerRequests?userrole=3" + "' style='text-decoration:none; background-color: green; padding:12px 16px; border-radius:3px;margin-bottom:47px; color:white'>" + "Go To Home" + "</a>" +
                //      "</center>" + "<br/>" +
                //            "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";


                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot/EmailTemp/emailTemp.txt");
                string MailBody = System.IO.File.ReadAllText(path) +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                    "<tr>" +
                                      "<td height='50'></td>" +
                                    "<tr>" +
                                      "<td align='center' style='line-height: 0px;'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/butlerlogo.png' alt='img' width='180px'/></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                      "</table>" +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#ECEFF3'>" +
                        //"<tr>" +
                        //  "<td height='70'></td >" +
                        //"</tr>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width ='520' align='center'>" +
                                  "<table class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                      "<tr>" +
                                        "<td height='50' align='center' bgcolor='#6EC8C7' style='border-top-left-radius:6px; border-top-right-radius: 6px;font-family: 'Open sans', Arial, sans-serif; color:#FFFFFF; font-size:20px;font-weight: bold;' > I.T Butler Notification</td>" +
                                      "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                    "</table>" +
                    "<table class='full' align='center' bgcolor='#ECEFF3' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table align='center' class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                    "<tr>" +
                                      "<td style='border-bottom-left-radius:6px;border-bottom-right-radius:6px; box-shadow:0px 3px 0px #E0E5EB;' bgcolor='#FFFFFF' align='center'>" +
                                        "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                          "<tr>" +
                                            "<td height='35'></td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td align='center' style='font-family: 'Open sans',Arial,sans-serif;color:#3b3b3b;font-size:22px;font-weight:bold;line-height:28px;'>Order Placed</td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td height='4'></td>" +
                                          "</tr>" +
                                          "<tr><td><hr style='border: 0.5px solid #6ec8c7b0;'></td></tr>" +
                                          "<tr>" +
                                            "<td style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:14px; line-height: 28px; margin-top: 10px;'>" +
                                                "<p>Dear User,</p>" +
                                                "<p>Congratulations an order is placed against your profile.</p>" +
                                                "<p>Kindly review it if you need any further revision.</p>" +
                                                "Thanks,<br>" +
                                                "The IT Butler Team." +
                                            "</td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='30'></td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='5'></td>" +
                                         "</tr>" +
                                         "<tr>" +
                                    "<td height='10'></td>" +
                                  "</tr>" +
                                  "<tr>" +
                                    "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#a7aeb3; font-size:13px; line-height: 20px;font-style: italic;'> *This email was intended for Usman, because you signed up for IT Butler | The links in this email will always direct to <a href='https://Itbutler.com/' target='_blank'> https://Itbutler.com/</a>." +
                                      "<p>© IT Butler International Ltd. 2023 </p></td>" +
                                  "</tr>" +
                                  "<tr>" +
                                    "<td height='5'></td>" +
                                  "</tr>" +
                                "</table>" +
                              "</td>" +
                            "</tr>" +
                          "</table>" +
                          "<table align='center' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                            "<tr>" +
                              "<td height='20'></td>" +
                            "</tr>" +
                          "</table>" +
                        "</td>" +
                      "</tr>" +
                    "</table>" +
                  "</td>" +
                "</tr>" +
                //"<tr>" +
                //  "<td height='50'></td>" +
                //"</tr>" +
              "</table>" +

              "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                "<tr>" +
                  "<td align='center'>" +
                    "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                      "<tr>" +
                        "<td width='520' align='center'>" +
                          "<table align='center' class='table-inner' width='90%' border='0' cellspacing='0' cellpadding='0'>" +
                            "<tr>" +
                              "<td align='center'>" +
                                "<table border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                  "<tr>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/fb.png' alt='img'/> </a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/tw.png' alt='img'/></a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/gg.png' alt='img'/></a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/in.png' alt='img'/></a>" +
                                    "</td>" +
                                  "</tr>" +
                                  "</table>" +
                              "</td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td height='0'></td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:12px; line-height: 28px;'><a href='#'>Webversion</a><span>&nbsp;&nbsp;|&nbsp;&nbsp;</span><a href='#'>Download App</a></td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td height='55'></td>" +
                            "</tr>" +
                          "</table>" +
                        "</td>" +
                      "</tr>" +
                    "</table>" +
                  "</td>" +
                "</tr>" +
              "</table>" +
        "</body>" +
    "</html>";


                RestClient client = new RestClient(new Uri("https://api.mailgun.net/v3"));
                client.Authenticator = new HttpBasicAuthenticator("api", "496e6c6979cb786921579085c5b07222-8d821f0c-bde767e8");
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "usmandev.ca", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", ProjectVariables.FromEmail);
                request.AddParameter("to", email);
                request.AddParameter("subject", "IT Butler | Order Placed");
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

        public static bool OrderDelivered(string email)
        {
            try
            {
                //string MailBody = "<html>" +
                //    "<head></head>" +
                //    "<body>" +
                //    "<center>" + "<div> <h1 class='text-center' style='color:#000000'> Your order is delivred by a butler.</h1> " +
                //    "</center>" + "<center>" +
                //      //"<a role ='button' href='" + ProjectVariables.baseUrl + "/Home/CustomerRequests?userrole=3" + "' style='text-decoration:none; background-color: green; padding:12px 16px; border-radius:3px;margin-bottom:47px; color:white'>" + "Go To Home" + "</a>" +
                //      "</center>" + "<br/>" +
                //            "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";


                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot/EmailTemp/emailTemp.txt");
                string MailBody = System.IO.File.ReadAllText(path) +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                    "<tr>" +
                                      "<td height='50'></td>" +
                                    "<tr>" +
                                      "<td align='center' style='line-height: 0px;'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/butlerlogo.png' alt='img' width='180px'/></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                      "</table>" +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#ECEFF3'>" +
                        //"<tr>" +
                        //  "<td height='70'></td >" +
                        //"</tr>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width ='520' align='center'>" +
                                  "<table class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                      "<tr>" +
                                        "<td height='50' align='center' bgcolor='#6EC8C7' style='border-top-left-radius:6px; border-top-right-radius: 6px;font-family: 'Open sans', Arial, sans-serif; color:#FFFFFF; font-size:20px;font-weight: bold;' > I.T Butler Notification</td>" +
                                      "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                    "</table>" +
                    "<table class='full' align='center' bgcolor='#ECEFF3' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table align='center' class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                    "<tr>" +
                                      "<td style='border-bottom-left-radius:6px;border-bottom-right-radius:6px; box-shadow:0px 3px 0px #E0E5EB;' bgcolor='#FFFFFF' align='center'>" +
                                        "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                          "<tr>" +
                                            "<td height='35'></td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#3b3b3b; font-size:22px;font-weight: bold; line-height: 28px;'>Order Delivered</td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td height='4'></td>" +
                                          "</tr>" +
                                          "<tr><td><hr style='border: 0.5px solid #6ec8c7b0;'></td></tr>" +
                                          "<tr>" +
                                            "<td style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:14px; line-height: 28px; margin-top: 10px;'>" +
                                                "<p>Dear User,</p>" +
                                                "<p>Your order is delivred by a butler.</p>" +
                                                "<p>Kindly review it if you need any further revision.</p>" +
                                                "Thanks,<br>" +
                                                "The IT Butler Team." +
                                            "</td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='30'></td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='5'></td>" +
                                         "</tr>" +
                                         "<tr>" +
                                    "<td height='10'></td>" +
                                  "</tr>" +
                                  "<tr>" +
                                    "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#a7aeb3; font-size:13px; line-height: 20px;font-style: italic;'> *This email was intended for Usman, because you signed up for IT Butler | The links in this email will always direct to <a href='https://Itbutler.com/' target='_blank'> https://Itbutler.com/</a>." +
                                      "<p>© IT Butler International Ltd. 2023 </p></td>" +
                                  "</tr>" +
                                  "<tr>" +
                                    "<td height='5'></td>" +
                                  "</tr>" +
                                "</table>" +
                              "</td>" +
                            "</tr>" +
                          "</table>" +
                          "<table align='center' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                            "<tr>" +
                              "<td height='20'></td>" +
                            "</tr>" +
                          "</table>" +
                        "</td>" +
                      "</tr>" +
                    "</table>" +
                  "</td>" +
                "</tr>" +
                //"<tr>" +
                //  "<td height='50'></td>" +
                //"</tr>" +
              "</table>" +

              "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                "<tr>" +
                  "<td align='center'>" +
                    "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                      "<tr>" +
                        "<td width='520' align='center'>" +
                          "<table align='center' class='table-inner' width='90%' border='0' cellspacing='0' cellpadding='0'>" +
                            "<tr>" +
                              "<td align='center'>" +
                                "<table border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                  "<tr>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/fb.png' alt='img'/> </a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/tw.png' alt='img'/></a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/gg.png' alt='img'/></a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/in.png' alt='img'/></a>" +
                                    "</td>" +
                                  "</tr>" +
                                  "</table>" +
                              "</td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td height='0'></td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:12px; line-height: 28px;'><a href='#'>Webversion</a><span>&nbsp;&nbsp;|&nbsp;&nbsp;</span><a href='#'>Download App</a></td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td height='55'></td>" +
                            "</tr>" +
                          "</table>" +
                        "</td>" +
                      "</tr>" +
                    "</table>" +
                  "</td>" +
                "</tr>" +
              "</table>" +
        "</body>" +
    "</html>";

                RestClient client = new RestClient(new Uri("https://api.mailgun.net/v3"));
                client.Authenticator = new HttpBasicAuthenticator("api", "496e6c6979cb786921579085c5b07222-8d821f0c-bde767e8");
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "usmandev.ca", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", ProjectVariables.FromEmail);
                request.AddParameter("to", email);
                request.AddParameter("subject", "IT Butler | Order Delivered");
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
        public static bool OrderCompleted(string email)
        {
            try
            {
                //string MailBody = "<html>" +
                //    "<head></head>" +
                //    "<body>" +
                //    "<center>" + "<div> <h1 class='text-center' style='color:#000000'> Congratulations your order is completed.</h1> " +
                //    "</center>" + "<center>" +
                //      //"<a role ='button' href='" + ProjectVariables.baseUrl + "/Home/CustomerRequests?userrole=3" + "' style='text-decoration:none; background-color: green; padding:12px 16px; border-radius:3px;margin-bottom:47px; color:white'>" + "Go To Home" + "</a>" +
                //      "</center>" + "<br/>" +
                //            "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";


                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot/EmailTemp/emailTemp.txt");
                string MailBody = System.IO.File.ReadAllText(path) +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                    "<tr>" +
                                      "<td height='50'></td>" +
                                    "<tr>" +
                                      "<td align='center' style='line-height: 0px;'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/butlerlogo.png' alt='img' width='180px'/></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                      "</table>" +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#ECEFF3'>" +
                        //"<tr>" +
                        //  "<td height='70'></td >" +
                        //"</tr>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width ='520' align='center'>" +
                                  "<table class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                      "<tr>" +
                                        "<td height='50' align='center' bgcolor='#6EC8C7' style='border-top-left-radius:6px; border-top-right-radius: 6px;font-family: 'Open sans', Arial, sans-serif; color:#FFFFFF; font-size:20px;font-weight: bold;' > I.T Butler Notification</td>" +
                                      "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                    "</table>" +
                    "<table class='full' align='center' bgcolor='#ECEFF3' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table align='center' class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                    "<tr>" +
                                      "<td style='border-bottom-left-radius:6px;border-bottom-right-radius:6px; box-shadow:0px 3px 0px #E0E5EB;' bgcolor='#FFFFFF' align='center'>" +
                                        "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                          "<tr>" +
                                            "<td height='35'></td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#3b3b3b; font-size:22px;font-weight: bold; line-height: 28px;'>Order Completed</td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td height='4'></td>" +
                                          "</tr>" +
                                          "<tr><td><hr style='border: 0.5px solid #6ec8c7b0;'></td></tr>" +
                                          "<tr>" +
                                            "<td style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:14px; line-height: 28px; margin-top: 10px;'>" +
                                                "<p>Dear User,</p>" +
                                                "<p>Congratulations your order is completed.</p>" +
                                                "<p>So kindly go check your wallet for payment.</p>" +
                                                "Thanks,<br>" +
                                                "The IT Butler Team." +
                                            "</td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='30'></td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='5'></td>" +
                                         "</tr>" +
                                         "<tr>" +
                                    "<td height='10'></td>" +
                                  "</tr>" +
                                  "<tr>" +
                                    "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#a7aeb3; font-size:13px; line-height: 20px;font-style: italic;'> *This email was intended for Usman, because you signed up for IT Butler | The links in this email will always direct to <a href='https://Itbutler.com/' target='_blank'> https://Itbutler.com/</a>." +
                                      "<p>© IT Butler International Ltd. 2023 </p></td>" +
                                  "</tr>" +
                                  "<tr>" +
                                    "<td height='5'></td>" +
                                  "</tr>" +
                                "</table>" +
                              "</td>" +
                            "</tr>" +
                          "</table>" +
                          "<table align='center' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                            "<tr>" +
                              "<td height='20'></td>" +
                            "</tr>" +
                          "</table>" +
                        "</td>" +
                      "</tr>" +
                    "</table>" +
                  "</td>" +
                "</tr>" +
                //"<tr>" +
                //  "<td height='50'></td>" +
                //"</tr>" +
              "</table>" +

              "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                "<tr>" +
                  "<td align='center'>" +
                    "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                      "<tr>" +
                        "<td width='520' align='center'>" +
                          "<table align='center' class='table-inner' width='90%' border='0' cellspacing='0' cellpadding='0'>" +
                            "<tr>" +
                              "<td align='center'>" +
                                "<table border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                  "<tr>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/fb.png' alt='img'/> </a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/tw.png' alt='img'/></a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/gg.png' alt='img'/></a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/in.png' alt='img'/></a>" +
                                    "</td>" +
                                  "</tr>" +
                                  "</table>" +
                              "</td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td height='0'></td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:12px; line-height: 28px;'><a href='#'>Webversion</a><span>&nbsp;&nbsp;|&nbsp;&nbsp;</span><a href='#'>Download App</a></td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td height='55'></td>" +
                            "</tr>" +
                          "</table>" +
                        "</td>" +
                      "</tr>" +
                    "</table>" +
                  "</td>" +
                "</tr>" +
              "</table>" +
        "</body>" +
    "</html>";





                RestClient client = new RestClient(new Uri("https://api.mailgun.net/v3"));
                client.Authenticator = new HttpBasicAuthenticator("api", "496e6c6979cb786921579085c5b07222-8d821f0c-bde767e8");
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "usmandev.ca", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", ProjectVariables.FromEmail);
                request.AddParameter("to", email);
                request.AddParameter("subject", "IT Butler | Order Completed");
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

        public static bool AccountHoldWarning(string email)
        {
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot/EmailTemp/emailTemp.txt");
                string MailBody = System.IO.File.ReadAllText(path) +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                    "<tr>" +
                                      "<td height='50'></td>" +
                                    "<tr>" +
                                      "<td align='center' style='line-height: 0px;'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/butlerlogo.png' alt='img' width='180px'/></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                      "</table>" +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#ECEFF3'>" +
                        //"<tr>" +
                        //  "<td height='70'></td >" +
                        //"</tr>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width ='520' align='center'>" +
                                  "<table class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                      "<tr>" +


                                        //"<td height='50' align='center' bgcolor='#6EC8C7' style='border-top-left-radius:6px; border-top-right-radius: 6px;font-family: 'Open sans', Arial, sans-serif; color:#FFFFFF; font-size:20px;font-weight: bold;' > I.T Butler Notification</td>" +
                                        
                                      "<td height='50' align='center' bgcolor='#6EC8C7' style='border-top-left-radius:6px; border-top-right-radius: 6px;font-family: 'Open sans', Arial, sans-serif; color:#FFFFFF; font-size:20px;font-weight: bold;'><font color='#fff'> I.T Butler Notification</font></td>" +


                                      "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                    "</table>" +
                    "<table class='full' align='center' bgcolor='#ECEFF3' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table align='center' class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                    "<tr>" +
                                      "<td style='border-bottom-left-radius:6px;border-bottom-right-radius:6px; box-shadow:0px 3px 0px #E0E5EB;' bgcolor='#FFFFFF' align='center'>" +
                                        "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                          "<tr>" +
                                            "<td height='35'></td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#3b3b3b; font-size:22px;font-weight: bold; line-height: 28px;'>Warning!</td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td height='4'></td>" +
                                          "</tr>" +
                                          "<tr><td><hr style='border: 0.5px solid #6ec8c7b0;'></td></tr>" +
                                          "<tr>" +
                                            "<td style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:14px; line-height: 28px; margin-top: 10px;'>" +
                                                "<p>Dear User,</p>" +
                                                "<p>You have got three consecutive bad ratings, if you will get two more bad ratings then your account will go on hold.</p>" +
                                                "<p>So kindly be careful and try to improve your rating to refrain from getting your account on hold.</p>" +
                                                "Thanks,<br>" +
                                                "The IT Butler Team." +
                                            "</td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='30'></td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='5'></td>" +
                                         "</tr>" +
                                         "<tr>" +
                                    "<td height='10'></td>" +
                                  "</tr>" +
                                  "<tr>" +
                                    "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#a7aeb3; font-size:13px; line-height: 20px;font-style: italic;'> *This email was intended for Usman, because you signed up for IT Butler | The links in this email will always direct to <a href='https://Itbutler.com/' target='_blank'> https://Itbutler.com/</a>." +
                                      "<p>© IT Butler International Ltd. 2023 </p></td>" +
                                  "</tr>" +
                                  "<tr>" +
                                    "<td height='5'></td>" +
                                  "</tr>" +
                                "</table>" +
                              "</td>" +
                            "</tr>" +
                          "</table>" +
                          "<table align='center' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                            "<tr>" +
                              "<td height='20'></td>" +
                            "</tr>" +
                          "</table>" +
                        "</td>" +
                      "</tr>" +
                    "</table>" +
                  "</td>" +
                "</tr>" +
                //"<tr>" +
                //  "<td height='50'></td>" +
                //"</tr>" +
              "</table>" +

              "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                "<tr>" +
                  "<td align='center'>" +
                    "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                      "<tr>" +
                        "<td width='520' align='center'>" +
                          "<table align='center' class='table-inner' width='90%' border='0' cellspacing='0' cellpadding='0'>" +
                            "<tr>" +
                              "<td align='center'>" +
                                "<table border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                  "<tr>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/fb.png' alt='img'/> </a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/tw.png' alt='img'/></a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/gg.png' alt='img'/></a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/in.png' alt='img'/></a>" +
                                    "</td>" +
                                  "</tr>" +
                                  "</table>" +
                              "</td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td height='0'></td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:12px; line-height: 28px;'><a href='#'>Webversion</a><span>&nbsp;&nbsp;|&nbsp;&nbsp;</span><a href='#'>Download App</a></td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td height='55'></td>" +
                            "</tr>" +
                          "</table>" +
                        "</td>" +
                      "</tr>" +
                    "</table>" +
                  "</td>" +
                "</tr>" +
              "</table>" +
        "</body>" +
    "</html>";
                RestClient client = new RestClient(new Uri("https://api.mailgun.net/v3"));
                client.Authenticator = new HttpBasicAuthenticator("api", "496e6c6979cb786921579085c5b07222-8d821f0c-bde767e8");
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "usmandev.ca", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", ProjectVariables.FromEmail);
                request.AddParameter("to", email);
                request.AddParameter("subject", "IT Butler | Warning");
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

        public static bool AccountOnHold(string email)
        {
            try
            {
                //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot/EmailTemp/emailTemp.txt");
                //string MailBody = System.IO.File.ReadAllText(path) +

                string MailBody =
                    "<!DOCTYPE>" +
"<html>" +
"<head>" +
 "<meta http - equiv='Content-Type' content='text/html; charset=utf-8'/>" +
 "<meta http - equiv='X-UA-Compatible' content='IE=edge'/>" +
 "<meta name='viewport' content='width=device-width, initial-scale=1.0'/>" +
  "<title></title>" +
  "<style type='text/css'>" +
".ReadMsgBody {" +
                "width: 100 %; background-color: #ffffff;}" +
".ExternalClass {" +
                    "width: 100 %; background-color: #ffffff;}" +
".ExternalClass, .ExternalClass p, .ExternalClass span, .ExternalClass font, .ExternalClass td, .ExternalClass div {line-height: 100 %;}" +
                        "html {width: 100 %;}" +
                        "body {-webkit-text-size-adjust: none;-ms-text-size-adjust: none; margin: 0; padding: 0;}" +
                        "table {border-spacing: 0; table-layout: fixed; margin: 0 auto;}" +
                        "table table table {table-layout: auto;}" +
".yshortcuts a {border-bottom: none!important;}" +
                    "img:hover {opacity:0.9!important;}" +
                        "a {" +
                        "color: #6ec8c7; text-decoration: none;}" +
".textbutton a {font-family: 'open sans', arial, sans-serif!important;}" +
".btn-link a {" +
                "color:#FFFFFF !important;}" +

                            "@@media only screen and(max-width: 640px) {" +
                    "body {margin: 0px; width: auto!important; font-family: 'Open Sans', Arial, Sans-serif!important;}" +
".table-inner {width: 90 % !important; max-width: 90 % !important;}" +
".table-full {width: 100 % !important; max-width: 100 % !important;}" +
                "}" +
                "@@media only screen and(max-width: 479px) {" +
                    "body {width: auto!important; font-family: 'Open Sans', Arial, Sans-serif!important;}" +
".table-inner{width: 90 % !important;}" +
".table-full {width: 100 % !important; max-width: 100 % !important;}" +
"u + .body.full {width: 100 % !important; width: 100vw!important;}" +
            "}" +


            ".text_color {" +
                "color: #fff !important;}" +



"</style>" +
"</head>" +

"<body class='body'>" +



                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                    "<tr>" +
                                      "<td height='50'></td>" +
                                    "<tr>" +
                                      "<td align='center' style='line-height: 0px;'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/butlerlogo.png' alt='img' width='180px'/></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                    "<tr>" +
                                    "</tr>" +
                                    "<tr>" +
                                      "<td height='10'></td>" +
                                    "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                      "</table>" +


                    "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#ECEFF3'>" +
                        //"<tr>" +
                        //  "<td height='70'></td >" +
                        //"</tr>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width ='520' align='center'>" +
                                  "<table class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                      "<tr>" +
                                        "<td class='text_color' height='50' align='center' bgcolor='#6EC8C7' color='#fff' style='border-top-left-radius:6px; border-top-right-radius: 6px;font-family: 'Open sans', Arial, sans-serif; color:#FFFFFF; font-size:20px;font-weight: bold;' > I.T Butler Notification</td>" +
                                      "</tr>" +
                                  "</table>" +
                                "</td>" +
                              "</tr>" +
                            "</table>" +
                          "</td>" +
                        "</tr>" +
                    "</table>" +
                    "<table class='full' align='center' bgcolor='#ECEFF3' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                        "<tr>" +
                          "<td align='center'>" +
                            "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                              "<tr>" +
                                "<td width='520' align='center'>" +
                                  "<table align='center' class='table-inner' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                                    "<tr>" +
                                      "<td style='border-bottom-left-radius:6px;border-bottom-right-radius:6px; box-shadow:0px 3px 0px #E0E5EB;' bgcolor='#FFFFFF' align='center'>" +
                                        "<table width='90%' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                          "<tr>" +
                                            "<td height='35'></td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#3b3b3b; font-size:22px;font-weight: bold; line-height: 28px;'>Account On Hold</td>" +
                                          "</tr>" +
                                          "<tr>" +
                                            "<td height='4'></td>" +
                                          "</tr>" +
                                          "<tr><td><hr style='border: 0.5px solid #6ec8c7b0;'></td></tr>" +
                                          "<tr>" +
                                            "<td style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:14px; line-height: 28px; margin-top: 10px;'>" +
                                                "<p>Dear User,</p>" +
                                                "<p>Your account is under review due to consecutive bad ratings till then your account will be on hold.</p>" +
                                                "<p>For further assistance contact to support.</p>" +
                                                "Thanks,<br>" +
                                                "The IT Butler Team." +
                                            "</td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='30'></td>" +
                                         "</tr>" +
                                         "<tr>" +
                                            "<td height='5'></td>" +
                                         "</tr>" +
                                         "<tr>" +
                                    "<td height='10'></td>" +
                                  "</tr>" +
                                  "<tr>" +
                                    "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#a7aeb3; font-size:13px; line-height: 20px;font-style: italic;'> *This email was intended for Usman, because you signed up for IT Butler | The links in this email will always direct to <a href='https://Itbutler.com/' target='_blank'> https://Itbutler.com/</a>." +
                                      "<p>© IT Butler International Ltd. 2023 </p></td>" +
                                  "</tr>" +
                                  "<tr>" +
                                    "<td height='5'></td>" +
                                  "</tr>" +
                                "</table>" +
                              "</td>" +
                            "</tr>" +
                          "</table>" +
                          "<table align='center' width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                            "<tr>" +
                              "<td height='20'></td>" +
                            "</tr>" +
                          "</table>" +
                        "</td>" +
                      "</tr>" +
                    "</table>" +
                  "</td>" +
                "</tr>" +
                //"<tr>" +
                //  "<td height='50'></td>" +
                //"</tr>" +
              "</table>" +

              "<table class='full' width='100%' border='0' align='center' cellpadding='0' cellspacing='0' bgcolor='#eceff3'>" +
                "<tr>" +
                  "<td align='center'>" +
                    "<table align='center' border='0' cellpadding='0' cellspacing='0'>" +
                      "<tr>" +
                        "<td width='520' align='center'>" +
                          "<table align='center' class='table-inner' width='90%' border='0' cellspacing='0' cellpadding='0'>" +
                            "<tr>" +
                              "<td align='center'>" +
                                "<table border='0' align='center' cellpadding='0' cellspacing='0'>" +
                                  "<tr>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/fb.png' alt='img'/> </a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/tw.png' alt='img'/></a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/gg.png' alt='img'/></a>" +
                                    "</td>" +
                                    "<td width='15'></td>" +
                                    "<td align='center' style='line-height: 0px;'>" +
                                      "<a href='#'><img style='display:block; line-height:0px; font-size:0px; border:0px;' src='http://usman78056-001-site4.gtempurl.com/images/in.png' alt='img'/></a>" +
                                    "</td>" +
                                  "</tr>" +
                                  "</table>" +
                              "</td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td height='0'></td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td align='center' style='font-family: 'Open sans', Arial, sans-serif; color:#7f8c8d; font-size:12px; line-height: 28px;'><a href='#'>Webversion</a><span>&nbsp;&nbsp;|&nbsp;&nbsp;</span><a href='#'>Download App</a></td>" +
                            "</tr>" +
                            "<tr>" +
                              "<td height='55'></td>" +
                            "</tr>" +
                          "</table>" +
                        "</td>" +
                      "</tr>" +
                    "</table>" +
                  "</td>" +
                "</tr>" +
              "</table>" +
        "</body>" +
    "</html>";
                RestClient client = new RestClient(new Uri("https://api.mailgun.net/v3"));
                client.Authenticator = new HttpBasicAuthenticator("api", "496e6c6979cb786921579085c5b07222-8d821f0c-bde767e8");
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "usmandev.ca", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", ProjectVariables.FromEmail);
                request.AddParameter("to", email);
                request.AddParameter("subject", "IT Butler | Warning");
                request.AddParameter("html", MailBody);
                request.Method = Method.POST;
                string response = client.ExecuteAsync(request).ToString();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}

