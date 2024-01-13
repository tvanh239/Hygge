

using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


using System;
using System.Configuration;


namespace Hygge.Service
{
    
    public class EmailService:IEmailService
    {
        private readonly IConfiguration? Configuration;
        string? EmailHost { get; set; }

        string? EmailUserName { get; set; }
        
        string? EmailPassword { get; set; }



        public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
            EmailHost = Configuration["EmailHost"];
            EmailUserName = Configuration["EmailUserName"];
            EmailPassword = Configuration["EmailPassword"];
        }

        public bool SendMailRegister(string email, string body)
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(EmailUserName ?? "");
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

 
    }

    
}
