using Azure.Messaging.ServiceBus;
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
		private ServiceBusProcessor _emailCartProcessor;
		private ServiceBusProcessor _registerUserProcessor;

		public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
		{
			_emailService = emailService;
			_configuration = configuration;
			serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
			emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
			registerUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:RegisterQueue");
			var client = new ServiceBusClient(serviceBusConnectionString);
			_emailCartProcessor = client.CreateProcessor(emailCartQueue);
			_registerUserProcessor = client.CreateProcessor(registerUserQueue);

		}

		public async Task Start()
		{
			_emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
			_emailCartProcessor.ProcessErrorAsync += ErrorHandler;
			await _emailCartProcessor.StartProcessingAsync();

			_registerUserProcessor.ProcessMessageAsync += OnUserRegisterRequestReceived;
			_registerUserProcessor.ProcessErrorAsync += ErrorHandler;
			await _registerUserProcessor.StartProcessingAsync();



		}

		public async Task Stop()
		{
			_emailCartProcessor.StopProcessingAsync();
			_emailCartProcessor.DisposeAsync();

			_registerUserProcessor.StopProcessingAsync();
			_registerUserProcessor.DisposeAsync();
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
			var message = args.Message;
			var body = Encoding.UTF8.GetString(message.Body);
			string email = JsonConvert.DeserializeObject<string>(body);
			try
			{
				await _emailService.RegisterUserEmailAndLog(email);
				await args.CompleteMessageAsync(args.Message);
			}
			catch (Exception ex)
			{
				throw;
			}
		}


	}
}
