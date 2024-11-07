using Azure.Messaging.ServiceBus;
using eCom.Services.EmailAPI.Message;
using eCom.Services.EmailAPI.Models.DTO;
using eCom.Services.EmailAPI.Service;
using Newtonsoft.Json;
using System.Text;

namespace eCom.Services.EmailAPI.Messaging
{
	public class AzureServiceBusConsumer : IAzureServiceBusConsumer
	{
		private readonly string serviceBusConnectionString;
		private readonly string emailCartQueue;
		private readonly string registerUserQueue;

		private readonly IConfiguration _configuration;
		private readonly EmailService _emailService;
        private readonly string orderCreated_Topic;
		private readonly string orderCreated_Email_Subscription;
        private ServiceBusProcessor _emailCartProcessor;
		private ServiceBusProcessor _emailOrderPlacedProcessor;
		private ServiceBusProcessor _registerUserProcessor;

		public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
		{
			_emailService = emailService;
			_configuration = configuration;
			serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
			emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
			registerUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:RegisterQueue");
            orderCreated_Topic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            orderCreated_Email_Subscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Email_Subcription");

			var client = new ServiceBusClient(serviceBusConnectionString);
			_emailCartProcessor = client.CreateProcessor(emailCartQueue);
			_registerUserProcessor = client.CreateProcessor(registerUserQueue);
            _emailOrderPlacedProcessor = client.CreateProcessor(orderCreated_Topic, orderCreated_Email_Subscription);

		}

		public async Task Start()
		{
			_emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
			_emailCartProcessor.ProcessErrorAsync += ErrorHandler;
			await _emailCartProcessor.StartProcessingAsync();

			_registerUserProcessor.ProcessMessageAsync += OnUserRegisterRequestReceived;
			_registerUserProcessor.ProcessErrorAsync += ErrorHandler;
			await _registerUserProcessor.StartProcessingAsync();


            _emailOrderPlacedProcessor.ProcessMessageAsync += OnOrderPlacedRequestReceived;
            _emailOrderPlacedProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailOrderPlacedProcessor.StartProcessingAsync();

        }

        public async Task Stop()
		{
			_emailCartProcessor.StopProcessingAsync();
			_emailCartProcessor.DisposeAsync();

			_registerUserProcessor.StopProcessingAsync();
			_registerUserProcessor.DisposeAsync();

			_emailOrderPlacedProcessor.StopProcessingAsync();
            _emailOrderPlacedProcessor.DisposeAsync();
		}
		private Task ErrorHandler(ProcessErrorEventArgs args)
		{
			Console.WriteLine(args.Exception.ToString());	
			return Task.CompletedTask;
		}

		private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
		{
			var message = args.Message;
			var body = Encoding.UTF8.GetString(message.Body);	
			CartDTO objMessage = JsonConvert.DeserializeObject<CartDTO>(body);
			try
			{
				await _emailService.EmailCartAndLog(objMessage);	
				await args.CompleteMessageAsync(args.Message);
			}catch (Exception ex)
			{
				throw;
			}

		}

		private async Task OnUserRegisterRequestReceived(ProcessMessageEventArgs args)
		{
            //this is the method that will be called when a message is received
            var message = args.Message;
			var body = Encoding.UTF8.GetString(message.Body);
			string email = JsonConvert.DeserializeObject<string>(body);
			try
			{
                //try to log email and send email
                await _emailService.RegisterUserEmailAndLog(email);
				await args.CompleteMessageAsync(args.Message);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

        private async Task OnOrderPlacedRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            RewardsMessage objMessage = JsonConvert.DeserializeObject<RewardsMessage>(body);
            try
            {
                await _emailService.LogOrderPlaced(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
