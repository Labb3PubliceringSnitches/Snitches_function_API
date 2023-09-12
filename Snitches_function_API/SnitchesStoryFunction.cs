using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Snitches_function_API.Models;
using Microsoft.Azure.Cosmos;
using System.ComponentModel;

namespace Snitches_function_API
{
    public static class SnitchesStoryFunction
    {

        private static readonly string connectionString = Environment.GetEnvironmentVariable("CosmosDBConnection", EnvironmentVariableTarget.Process);
        private static CosmosClient cosmosClient = new CosmosClient(connectionString);
        private static Microsoft.Azure.Cosmos.Container container = cosmosClient.GetContainer("SnitchHereDB", "SnitchesStories");

        [FunctionName("CreateSnitchStory")]
        public static async Task<IActionResult> CreateSnitchStory(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "snitchstories")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            CreateSnitch createSnitch = JsonConvert.DeserializeObject<CreateSnitch>(requestBody);


            if (string.IsNullOrEmpty(createSnitch.SnitchStory))
            {
                log.LogError("The SnitchStory is null or empty.");
                return new BadRequestObjectResult("The SnitchStory is null or empty.");
            }

            SnitchesStory newStory = new SnitchesStory
            {
                SnitchStory = createSnitch.SnitchStory
            };


            if (string.IsNullOrEmpty(newStory.Id))
            {
                log.LogError("The story ID is null or empty.");
                return new BadRequestObjectResult("The story ID is null or empty.");
            }

            log.LogInformation($"Creating a new snitch story with ID: {newStory.Id}");
            try
            {

                ItemResponse<SnitchesStory> response = await container.CreateItemAsync(newStory, new PartitionKey(newStory.Id));
                return new OkObjectResult(response.Resource);
            }
            catch (Exception ex)
            {
                log.LogError($"An error occurred: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
