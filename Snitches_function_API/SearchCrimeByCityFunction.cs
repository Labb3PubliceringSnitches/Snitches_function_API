using System.Collections.Generic;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Snitches_function_API.Models;

namespace Snitches_function_API
{
    public class SearchCrimeByCityFunction
    {
        private readonly ILogger<SearchCrimeByCityFunction> _logger;
        private HttpClient client = new();

        public SearchCrimeByCityFunction(ILogger<SearchCrimeByCityFunction> log)
        {
            _logger = log;
        }

        [FunctionName("SearchCrimeByCityName")]
        [OpenApiOperation(operationId: "Run")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "City", In = ParameterLocation.Query, Required = true, Type = typeof(string))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "City")] HttpRequest req)
        {
            try
            {
                string searchWord = req.Query["City"]; // Takes input from user
                string apiUrl = $"http://polisen.se/api/events?locationName={searchWord}";

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                string content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"API request failed with status code: {response.StatusCode}");
                    return new StatusCodeResult((int)response.StatusCode);
                }

                List<CrimesDTO> crimeList = JsonConvert.DeserializeObject<List<CrimesDTO>>(content);

                return new OkObjectResult(crimeList);

            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }


    }
}

