using Azure.Messaging.ServiceBus;
using eCom.Services.RewardAPI.Message;
using eCom.Services.RewardAPI.Service;
using Newtonsoft.Json;
using System.Text;

namespace eCom.Services.RewardAPI.Messaging
{
	public class AzureServiceBusConsumer : IAzureServiceBusConsumer
	{
		private readonly string serviceBusConnectionString;
		private readonly string orderCreatedTopic;
		private readonly string orderCreatedRewardSubcription;

		private readonly IConfiguration _configuration;
		private readonly RewardService _rewardService;	
		private ServiceBusProcessor _rewardProcessor;

		public AzureServiceBusConsumer(IConfiguration configuration, RewardService rewardService)
		{
            _rewardService = rewardService;
			_configuration = configuration;
			serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            orderCreatedTopic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            orderCreatedRewardSubcription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Rewards_Subcription");
			var client = new ServiceBusClient(serviceBusConnectionString);
            _rewardProcessor = client.CreateProcessor(orderCreatedTopic, orderCreatedRewardSubcription);
		}

		public async Task Start()
		{
            _rewardProcessor.ProcessMessageAsync += OnNewOrderRewardsRequestReceived;
            _rewardProcessor.ProcessErrorAsync += ErrorHandler;
			await _rewardProcessor.StartProcessingAsync();


		}

		public async Task Stop()
		{
            _rewardProcessor.StopProcessingAsync();
            _rewardProcessor.DisposeAsync();

		}
		private Task ErrorHandler(ProcessErrorEventArgs args)
		{
			Console.WriteLine(args.Exception.ToString());	
			return Task.CompletedTask;
		}

		private async Task OnNewOrderRewardsRequestReceived(ProcessMessageEventArgs args)
		{
			var message = args.Message;
			var body = Encoding.UTF8.GetString(message.Body);	
			RewardsMessage objMessage = JsonConvert.DeserializeObject<RewardsMessage>(body);
			try
			{
				await _rewardService.UpdateRewards(objMessage);	
				await args.CompleteMessageAsync(args.Message);
			}catch (Exception ex)
			{
				throw;
			}

		}

	}
}
