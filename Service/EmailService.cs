//*****************************************************************************
//* ALL RIGHTS RESERVED. COPYRIGHT (C) 2024 Hygge                             *
//*****************************************************************************
//* File Name    : EmailService.cs   　　　                        　          *
//* Function     : Service for email                                          *
//* Create       : VietAnh 2024/01/13                                         *
//*****************************************************************************.

using System.Net;
using System.Net.Mail;


namespace Hygge.Service
{
    /// <summary>The service which can send mail </summary>
    public class EmailService:IEmailService
    {
        #region properties
        /// <summary>The config in appsetting.json </summary>
        private readonly IConfiguration? Configuration;
        /// <summary>The host of the mail </summary>
        string? EmailHost { get; set; }
        /// <summary>The user of the From Mail </summary>
        string? EmailUserName { get; set; }
        /// <summary>The password of the From Mail </summary>
        string? EmailPassword { get; set; }
        #endregion

        #region functions
        /// <summary>
        /// Init function
        /// </summary>
        /// <param name="configuration">the config in appsetting.json</param>
        public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
            EmailHost = Configuration["EmailHost"];
            EmailUserName = Configuration["EmailUserName"];
            EmailPassword = Configuration["EmailPassword"];
        }
        /// <summary>
        /// Send Mail when sign up
        /// </summary>
        /// <param name="email">register email</param>
        /// <param name="body">the body email</param>
        /// <returns></returns>
        public bool SendMailRegister(string email, string body)
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(EmailUserName ?? "", "HYGGE");
                message.To.Add(new MailAddress(email) ) ;
                message.Subject = "Confirm your account at Hygge";

                string bodyMail = $"Click on <a href=\"{body}\">here</a> to confirm your account.";
                message.Body = bodyMail;
                message.IsBodyHtml = true ;
                var smtp = new SmtpClient(EmailHost) { 
                    Port = 587,
                    Credentials = new NetworkCredential(EmailUserName, EmailPassword),
                    EnableSsl = true
                };
                smtp.Send(message);
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

    }


}
