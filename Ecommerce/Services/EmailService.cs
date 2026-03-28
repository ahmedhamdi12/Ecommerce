using System.Net;
using System.Net.Mail;

namespace Ecommerce.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var fromEmail = "ahmedhbarakat123@gmail.com";
            var password = "zjmz mwfi hvpy cvin";

            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true
            };
             
            var message = new MailMessage(fromEmail, toEmail, subject, body);
            message.IsBodyHtml = true;
            await smtp.SendMailAsync(message);
        }
    }
}
