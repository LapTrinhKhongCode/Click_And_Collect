using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace eCom.Services.EmailAPI.Service
{
    public class EmailSender : IEmailSender
    {
        public string SendGridKey { get; set; }
        public EmailSender(IConfiguration _configuration)
        {
            SendGridKey = _configuration.GetValue<string>("SendGrid:SecretKey");
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(SendGridKey);
            var from_email = new EmailAddress("102220262@sv1.dut.udn.vn", "Click And Collect");

            var to_email = new EmailAddress(email);

            var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, "", htmlMessage);
            return client.SendEmailAsync(msg);
        }
    }

}
