using eCom.Services.RewardAPI.Data;
using eCom.Services.RewardAPI.Message;
using eCom.Services.RewardAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace eCom.Services.RewardAPI.Service
{
	public class RewardService : IRewardService
	{
		private DbContextOptions<AppDbContext> _dbOptions;

		public RewardService(DbContextOptions<AppDbContext> dbOptions)
		{
			_dbOptions = dbOptions;
		}

		

        public async Task UpdateRewards(RewardsMessage rewardsMessage)
        {
			try
			{
				Rewards rewards = new()
				{
					OrderId = rewardsMessage.OrderId,	
					RewardsActivity = rewardsMessage.RewardActivity,
					UserId = rewardsMessage.UserId,	
					RewardsDate = DateTime.Now
				};


				await using var _db = new AppDbContext(_dbOptions);
				await _db.Rewards.AddAsync(rewards);
				await _db.SaveChangesAsync();

			}catch (Exception ex)
			{

			}
		}

		
	}
}
