using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CarBank.WebApi.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("ysidibe85", "d@ta167m0od460");
                client.Port = 587;
                client.EnableSsl = true;
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("ysidibe85@gmail.com");
                mailMessage.To.Add(email);
                mailMessage.Body = message;
                mailMessage.Subject = subject;
                //client.Send(mailMessage);

            }
            
            return Task.CompletedTask;
        }
    }
}
