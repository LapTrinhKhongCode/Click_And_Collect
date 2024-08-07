
using eCom.Services.RewardAPI.Message;

namespace eCom.Services.RewardAPI.Service
{
	public interface IRewardService
	{
		Task UpdateRewards(RewardsMessage rewardsMessage);
	}
}
