using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Service.Commons
{
    public interface IEmailSendGrid
    {
        Task Execute(string subject, string message, string email);
    }


    //Para más información visite https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-3.1&tabs=visual-studio
    public class EmailSendGrid : IEmailSendGrid
    {

        //Envio de mensaje simple
        public Task Execute(string subject, string message, string email)
        {
            string apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");

            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("jhotams.developer@gmail.com", "JhotaMS"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Deshabilitar el seguimiento de clics.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }

}
