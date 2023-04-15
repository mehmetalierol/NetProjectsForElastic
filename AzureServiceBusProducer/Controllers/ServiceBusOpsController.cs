using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace AzureServiceBusProducer.Controllers
{
    [Route("[controller]")]
    public class ServiceBusOpsController : Controller
    {
        private IConfiguration Config;
        private ILogger<ServiceBusOpsController> Logger;

        public ServiceBusOpsController(IConfiguration config, ILogger<ServiceBusOpsController> logger)
        {
            Config = config;
            Logger = logger;
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] UserModel message)
        {
            await Send(message);
            Logger.LogWarning(JsonConvert.SerializeObject(message));
            return Ok();
        }

        private async Task Send(UserModel message)
        {
            string connectionString = $"Endpoint={Config["AzureServiceBus:Endpoint"]};" +
                                      $"SharedAccessKeyName={Config["AzureServiceBus:SharedAccessKeyName"]};" +
                                      $"SharedAccessKey={Config["AzureServiceBus:SharedAccessKey"]}";

            IQueueClient queueClient = new QueueClient(connectionString, Config["AzureServiceBus:QueueName"]);
            var orderJSON = JsonConvert.SerializeObject(message);
            var orderMessage = new Message(Encoding.UTF8.GetBytes(orderJSON))
            {
                MessageId = Guid.NewGuid().ToString(),
                ContentType = "application/json"
            };

            await queueClient.SendAsync(orderMessage).ConfigureAwait(false);
        }
    }
}