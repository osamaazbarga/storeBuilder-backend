using Azure.Core;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using MimeKit.Text;
using MimeKit;
using Newtonsoft.Json.Linq;
using superecommere.Models.DTO;
using MailKit.Net.Smtp;


namespace superecommere.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        //private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public EmailService(IConfiguration config/*, Microsoft.AspNetCore.Hosting.IHostingEnvironment env*/)
        {
            _config = config;
            //_env = env;
        }

        public bool SendEmail(EmailSendDto emailSend)
        {





            //MailMessage msg = new MailMessage("osamaazbarga@gmail.com", "osamaazbarga@gmail.com");
            //AlternateView plainview = AlternateView.CreateAlternateViewFromString("some plaintext", Encoding.UTF8, "text/plain");
            //// we have something to show in real old mail clients.
            //msg.AlternateViews.Add(plainview);
            //string htmltext = "the <b>fancy</b> part.";
            //AlternateView htmlview =
            //     AlternateView.CreateAlternateViewFromString("<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n    <title>Welcome Email from TCP</title>\r\n</head>\r\n\r\n<body>\r\n    <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n        <tr>\r\n            <td align=\"center\" valign=\"top\" bgcolor=\"#ffe77b\" style=\"background-color:#ffe77b;\">\r\n                <br>\r\n                <br>\r\n                <table width=\"600\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n                    <tr>\r\n                        <td height=\"70\" align=\"left\" valign=\"middle\"></td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td align=\"left\" valign=\"top\"><img src=\"http://localhost:2131/Templates/EmailTemplate/images/top.png\" width=\"600\" height=\"13\" style=\"display:block;\"></td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td align=\"left\" valign=\"top\" bgcolor=\"#564319\" style=\"background-color:#564319; font-family:Arial, Helvetica, sans-serif; padding:10px;\">\r\n                            <div style=\"font-size:36px; color:#ffffff;\">\r\n                                <b>{0}</b>\r\n                            </div>\r\n                            <div style=\"font-size:13px; color:#a29881;\">\r\n                                <b>{1} : ASP.NET Core Demp App</b>\r\n                            </div>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td align=\"left\" valign=\"top\" bgcolor=\"#ffffff\" style=\"background-color:#ffffff;\">\r\n                            <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n                                <tr>\r\n                                    <td align=\"center\" valign=\"middle\" style=\"padding:10px; color:#564319; font-size:28px; font-family:Georgia, 'Times New Roman', Times, serif;\">\r\n                                        Congratulations! <small>You are registered.</small>\r\n                                    </td>\r\n                                </tr>\r\n                            </table>\r\n                            <table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">\r\n                                <tr>\r\n                                    <td width=\"40%\" align=\"center\" valign=\"middle\" style=\"padding:10px;\">\r\n                                        <img src=\"http://localhost:2131/Templates/EmailTemplate/images/Weak_Password.gif\" width=\"169\" height=\"187\" style=\"display:block\">\r\n                                    </td>\r\n                                    <td align=\"left\" valign=\"middle\" style=\"color:#525252; font-family:Arial, Helvetica, sans-serif; padding:10px;\">\r\n                                        <div style=\"font-size:16px;\">\r\n                                            Dear {2},\r\n                                        </div>\r\n                                        <div style=\"font-size:12px;\">\r\n                                            Thank you for showing your interest  in  our website.\r\n                                            All you need to do is click the button below (it only takes a few seconds).\r\n                                            You won’t be asked to log in to your account – we're simply verifying ownership of this email address.\r\n                                            <hr>\r\n                                            <center>\r\n\r\n                                                <button type=\"button\" title=\"Confirm Account Registration\" style=\"background: #1b97f1\">\r\n                                                    <a href=\"{6}\" style=\"font-size:22px; padding: 10px; color: #ffffff\">\r\n                                                        Confirm Email Now\r\n                                                    </a>\r\n                                                </button>\r\n\r\n                                            </center>\r\n                                        </div>\r\n                                    </td>\r\n                                </tr>\r\n                            </table>\r\n                            <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n                                <tr>\r\n                                    <td align=\"center\" valign=\"middle\" style=\"padding:5px;\">\r\n                                        <img src=\"http://localhost:2131/Templates/EmailTemplate/images/divider.gif\" width=\"566\" height=\"30\">\r\n                                    </td>\r\n                                </tr>\r\n                            </table>\r\n                            <table width=\"100%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" style=\"margin-bottom:15px;\">\r\n                                <tr>\r\n                                    <td align=\"left\" valign=\"middle\" style=\"padding:15px; font-family:Arial, Helvetica, sans-serif;\">\r\n                                        <div style=\"font-size:20px; color:#564319;\">\r\n                                            <b>Please keep your credentials confidential for future use. </b>\r\n                                        </div>\r\n                                        <div style=\"font-size:16px; color:#525252;\">\r\n                                            <b>Email         :</b> {2}\r\n                                            <br />\r\n                                            <b>Username :</b> {3}\r\n                                            <br />\r\n                                            <b>Password :</b> {4}\r\n                                        </div>\r\n                                    </td>\r\n                                </tr>\r\n                            </table>\r\n                            <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-bottom:10px;\">\r\n                                <tr>\r\n                                    <td align=\"left\" valign=\"middle\" style=\"padding:15px; background-color:#564319; font-family:Arial, Helvetica, sans-serif;\">\r\n                                        <div style=\"font-size:20px; color:#fff;\">\r\n                                            <b>Update your password now.</b>\r\n                                        </div>\r\n                                        <div style=\"font-size:13px; color:#ffe77b;\">\r\n                                            Weak passwords get stolen and lead to hacked accounts. Celebrate World Password Day with a new, strong password.\r\n                                            <br>\r\n                                            <br>\r\n                                            <a href=\"#\" style=\"color:#ffe77b; text-decoration:underline;\">CLICK HERE</a> TO CHANGE PASSOWORD\r\n                                        </div>\r\n                                    </td>\r\n                                </tr>\r\n                            </table>\r\n                            <table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">\r\n                                <tr>\r\n                                    <td width=\"50%\" align=\"left\" valign=\"middle\" style=\"padding:10px;\">\r\n                                        <table width=\"75%\" border=\"0\" cellspacing=\"0\" cellpadding=\"4\">\r\n                                            <tr>\r\n                                                <td align=\"left\" valign=\"top\" style=\"font-family:Verdana, Geneva, sans-serif; font-size:14px; color:#000000;\">\r\n                                                    <b>Follow Us On</b>\r\n                                                </td>\r\n                                            </tr>\r\n                                            <tr>\r\n                                                <td align=\"left\" valign=\"top\" style=\"font-family:Verdana, Geneva, sans-serif; font-size:12px; color:#000000;\">\r\n                                                    <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n                                                        <tr>\r\n                                                            <td width=\"33%\" align=\"left\" valign=\"middle\">\r\n                                                                <a href=\"https://twitter.com\" title=\"Facebook\">\r\n                                                                    <img src=\"http://localhost:2131/Templates/EmailTemplate/images/tweet48.png\" width=\"48\" height=\"48\">\r\n                                                                </a>\r\n                                                            </td>\r\n                                                            <td width=\"34%\" align=\"left\" valign=\"middle\">\r\n                                                                <a href=\"https://linkedin.com\" title=\"Linkedin\">\r\n                                                                    <img src=\"http://localhost:2131/Templates/EmailTemplate/images/in48.png\" width=\"48\" height=\"48\">\r\n                                                                </a>\r\n                                                            </td>\r\n                                                            <td width=\"33%\" align=\"left\" valign=\"middle\">\r\n                                                                <a href=\"https://facebook.com\" title=\"Facebook\">\r\n                                                                    <img src=\"http://localhost:2131/Templates/EmailTemplate/images/face48.png\" width=\"48\" height=\"48\">\r\n                                                                </a>\r\n                                                            </td>\r\n                                                        </tr>\r\n                                                    </table>\r\n                                                </td>\r\n                                            </tr>\r\n                                        </table>\r\n                                    </td>\r\n                                    <td width=\"50%\" align=\"left\" valign=\"middle\" style=\"color:#564319; font-size:11px; font-family:Arial, Helvetica, sans-serif; padding:10px;\">\r\n                                        <b>Hours:</b> Mon-Fri 9:30-5:30, Sat. 9:30-3:00, Sun. Closed <br>\r\n                                        <b>Customer Support:</b> <a href=\"mailto:name@yourcompanyname.com\" style=\"color:#564319; text-decoration:none;\">name@yourcompanyname.com</a><br>\r\n                                        <br>\r\n                                        <b>Company Address</b><br>\r\n                                        Company URL: <a href=\"http://www.yourcompanyname.com\" target=\"_blank\" style=\"color:#564319; text-decoration:none;\">http://www.yourcompanyname.com</a>\r\n                                    </td>\r\n                                </tr>\r\n                            </table>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td align=\"left\" valign=\"top\"><img src=\"http://localhost:2131/Templates/EmailTemplate/images/bot.png\" width=\"600\" height=\"37\" style=\"display:block;\"></td>\r\n                    </tr>\r\n                </table>\r\n                <br>\r\n                <br>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n</html>   ", Encoding.UTF8, "text/html");
            //msg.AlternateViews.Add(htmlview); // and a html attachment to make sure.
            //msg.Body = htmltext;  // but the basis is the html body
            //msg.IsBodyHtml = true; // but the basis is the html body\
            //msg.d
            var email = new MimeMessage();
            //var webRoot = _env.WebRootPath;
            //var pathToFile = _env.WebRootPath
            //                + Path.DirectorySeparatorChar.ToString()
            //                + "Templates"
            //                + Path.DirectorySeparatorChar.ToString()
            //                + "EmailTemplate"
            //                + Path.DirectorySeparatorChar.ToString()
            //                + "Confirm_Account_Registration.html";
            email.From.Add(new MailboxAddress(_config["Email:ApplicationName"], "osamaazbarga@gmail.com"));
            email.To.Add(MailboxAddress.Parse(emailSend.To));
            email.Subject = emailSend.Subject;
            var builder=new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText("C:\\Users\\Osama Azbarga\\Documents\\projects\\projectEcommere\\Ecommere\\SuperEcommereWebAPI7.0\\superecommere\\superecommere\\Templates\\EmailTemplate\\ConfirmAccountRegistration.html"))
            {
                
                builder.HtmlBody = File.ReadAllText("C:\\Users\\Osama Azbarga\\Documents\\projects\\projectEcommere\\Ecommere\\SuperEcommereWebAPI7.0\\superecommere\\superecommere\\Templates\\EmailTemplate\\ConfirmAccountRegistration.html");

            }
            //for design mail open this and close the mail register send for test
            builder.HtmlBody = emailSend.Body;
            email.Body=builder.ToMessageBody(); 



            //email.Body = new TextPart("plain")
            //{
            //    Text = emailSend.Body
            //};

            using var smtp = new SmtpClient();
            try
            {
                smtp.Connect("smtp.gmail.com", 465, true);
                smtp.Authenticate("osamaazbarga@gmail.com", "sxxt ffdh itjk mhvd");
                smtp.Send(email);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                smtp.Disconnect(true);
                smtp.Dispose();
            }

            //var email =new TransactionalEmailBuilder()
            //    .WithFrom(new SendContact(_config["Email:From"], _config["Email:ApplicationName"]))
            //    .WithSubject(emailSend.Subject)
            //    .WithHtmlPart(emailSend.Body)
            //    .WithTo(new SendContact(emailSend.To))
            //    .Build();
            //MailjetRequest request = new MailjetRequest
            //{
            //    Resource = Send.Resource,
            //}
            // .Property(Send.FromEmail, "o.s.2@hotmail.com")
            // .Property(Send.FromName, "Mailjet Pilot")
            // .Property(Send.Subject, "Your email flight plan!")
            // .Property(Send.TextPart, "Dear passenger, welcome to Mailjet! May the delivery force be with you!")
            // .Property(Send.HtmlPart, "<h3>Dear passenger, welcome to <a href=\"https://www.mailjet.com/\">Mailjet</a>!<br />May the delivery force be with you!")
            // .Property(Send.To, "Name2 <osamaazbarga@gmail.com>")


            //MailjetResponse resopnse = await client.PostAsync(request);
            //var resopnse = await client.SendTransactionalEmailAsync(email);

        }
    }
}
