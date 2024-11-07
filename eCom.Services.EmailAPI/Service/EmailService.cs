using eCom.Services.EmailAPI.Data;
using eCom.Services.EmailAPI.Message;
using eCom.Services.EmailAPI.Models;
using eCom.Services.EmailAPI.Models.DTO;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Text;

namespace eCom.Services.EmailAPI.Service
{
	public class EmailService : IEmailService, IEmailSender
	{
		private DbContextOptions<AppDbContext> _dbOptions;
        private readonly IEmailSender _emailSender;
        public string SendGridKey { get; set; }
        public EmailService(DbContextOptions<AppDbContext> dbOptions, IEmailSender emailSender, IConfiguration _configuration)
		{
			_dbOptions = dbOptions;
            _emailSender = emailSender;
            SendGridKey = _configuration.GetValue<string>("SendGrid:SecretKey");
        }

		public async Task EmailCartAndLog(CartDTO cartDTO)
		{
			StringBuilder message = new StringBuilder();
			message.AppendLine("<br/>Cart Email Request ");
			message.AppendLine("<br/>Total " + cartDTO.CartHeader.CartTotal);		
			message.Append("<br/>");
			message.Append("<ul>");

			foreach (var item in cartDTO.CartDetails)
			{
				message.Append("<li>");
				message.Append(item.Product.Name + " x " + item.Count);
				message.Append("</li>");
			}
			message.Append("</ul>");
			await LogAndEmail(message.ToString(), cartDTO.CartHeader.Email);
		}

        public async Task LogOrderPlaced(RewardsMessage rewardsDTO)
        {
            string message = "New Order Placed. <br/> Order ID:" + rewardsDTO.OrderId;
            await LogAndEmail(message, rewardsDTO.Email);
        }

        public async Task RegisterUserEmailAndLog(string email)
		{
			string message = "User Registeration Successful. <br/> Email:" + email;
			await LogAndEmail(message, email);
		}

        private async Task<bool> LogAndEmail(string message, string email)
		{
			try
			{
				EmailLogger emailLogger = new EmailLogger()
				{
					Email = email,	
					EmailSent = DateTime.Now,
					Message = message,

				};

				await using var _db = new AppDbContext(_dbOptions);
				await _db.EmailLoggers.AddAsync(emailLogger);
				await _db.SaveChangesAsync();
				return true;
			}catch (Exception ex)
			{
				return false;
			}
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
