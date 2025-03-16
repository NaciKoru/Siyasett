using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Siyasett.Web.Models
{
  
    public class EmailService : IEmailSender
    {
        private readonly MailSettings _mailSettings;

        public EmailService(MailSettings mailSettings)
        {
            _mailSettings = mailSettings;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            
                var mail = new MailMessage()
                {
                    From = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };

                mail.To.Add(new MailAddress(email));
                var credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);

                var client = new SmtpClient()
                {
                    Port = _mailSettings.Port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = _mailSettings.Host,
                    EnableSsl = true,
                    Credentials = credentials,
                    Timeout=30
                };


            await client.SendMailAsync(mail);
                
            

        }
    }

}
