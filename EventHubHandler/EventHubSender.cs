using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.EventHubs;
using System.Text;

namespace EventHubHandler
{
    public static class EventHubSender
    {
        [FunctionName("EventHubSender")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Event")] HttpRequest req,
            [EventHub("testsample", Connection = "EventHubConnection")]
            IAsyncCollector<EventData> outputMessages,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<EventModel.EventModel>(requestBody);
            await outputMessages.AddAsync(new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data))));

            return new OkObjectResult("Hello!");
            //return name != null
            //    ? (ActionResult)new OkObjectResult($"Hello, {name}")
            //    : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
