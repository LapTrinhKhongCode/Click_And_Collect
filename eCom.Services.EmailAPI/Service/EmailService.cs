﻿using eCom.Services.EmailAPI.Data;
using eCom.Services.EmailAPI.Message;
using eCom.Services.EmailAPI.Models;
using eCom.Services.EmailAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace eCom.Services.EmailAPI.Service
{
	public class EmailService : IEmailService
	{
		private DbContextOptions<AppDbContext> _dbOptions;

		public EmailService(DbContextOptions<AppDbContext> dbOptions)
		{
			_dbOptions = dbOptions;
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
            await LogAndEmail(message, "dovanducanh06@gmail.com");
        }

        public async Task RegisterUserEmailAndLog(string email)
		{
			string message = "User Registeration Successful. <br/> Email:" + email;
			await LogAndEmail(message, "dovanducanh06@gmail.com");
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
	}
}
