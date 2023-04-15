using Azure.Messaging.ServiceBus;
using Common;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AzureServiceBusConsumer
{
    public class Worker : BackgroundService
    {
        private ServiceBusProcessor SBProcessor { get; set; }
        private ServiceBusReceiver SBReceiver { get; set; }
        private ILogger<Worker> Logger { get; set; }
        private IConfiguration Config { get; set; }

        public Worker(IConfiguration config, ILogger<Worker> logger)
        {
            Logger = logger;
            Config = config;

            string connectionString = $"Endpoint={config["AzureServiceBus:Endpoint"]};" +
                                      $"SharedAccessKeyName={config["AzureServiceBus:SharedAccessKeyName"]};" +
                                      $"SharedAccessKey={config["AzureServiceBus:SharedAccessKey"]};" +
                                      $"EntityPath={config["AzureServiceBus:QueueName"]}";

            var client = new ServiceBusClient(connectionString);

            SBProcessor = client.CreateProcessor(config["AzureServiceBus:QueueName"], new ServiceBusProcessorOptions());
            SBProcessor.ProcessMessageAsync += MessageHandler;
            SBProcessor.ProcessErrorAsync += ErrorHandler;

            SBReceiver = client.CreateReceiver(Config["AzureServiceBus:QueueName"]);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await SBProcessor.StartProcessingAsync(stoppingToken);
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            await SBReceiver.ReceiveMessageAsync();
            await SendSms(JsonConvert.DeserializeObject<UserModel>(args.Message.Body.ToString()));
            await SendEmail(JsonConvert.DeserializeObject<UserModel>(args.Message.Body.ToString()));
            Logger.LogWarning(args.Message.Body.ToString());
            await args.CompleteMessageAsync(args.Message);
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Logger.LogError(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task SendSms(UserModel user)
        {
            Logger.LogWarning("SendSms Called.");
            HttpClient client = new()
            {
                BaseAddress = new Uri("http://localhost:5006/Sms/")
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("SendSms", data);
        }

        private async Task SendEmail(UserModel user)
        {
            Logger.LogWarning("SendEmail Called.");
            HttpClient client = new()
            {
                BaseAddress = new Uri("http://localhost:5005/Mail/")
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("SendMail", data);
        }
    }
}