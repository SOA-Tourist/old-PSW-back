using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.UseCases.Administration;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos.Execution;
using Explorer.Tours.Core.Domain.Tours;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Explorer.API.Controllers.Tourist.Execution
{
    [Authorize(Policy = "touristPolicy")]
    [Microsoft.AspNetCore.Mvc.Route("api/tourist/execution/encounter")]
    public class EncounterExecutionController : BaseApiController
    {
        private readonly IEncounterExecutionService _encounterExecutionService;
        private readonly IEncounterService _encounterService;

        public EncounterExecutionController(IEncounterExecutionService encounterExecutionService, IEncounterService encounterService)
        {
            _encounterExecutionService = encounterExecutionService;
            _encounterService = encounterService;
        }

        [HttpGet("statistics/user/{userId:long}")]
        public ActionResult<EncounterStatisticsDto> GetStatisticsForUser(long userId)
        {
            var result = _encounterExecutionService.GetStatisticsForUser(userId);
            return CreateResponse(result);
        }

        //OVO MENJAM
        [HttpGet("allEncounters")]
        public async Task<ActionResult<EncounterStringDto>> GetAllActive()
        {
            //   var result = _encounterService.GetAllActive();
            //    return CreateResponse(result);
            using (HttpClient client = new HttpClient())
            {
                string url = "http://localhost:8083/api/encounters/activate";
                try
                {
                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadFromJsonAsync<List<EncounterStringDto>>();
                        var pagedResult = new PagedResult<EncounterStringDto>(responseData, responseData.Count);

                        return Ok(pagedResult);
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, "Error calling the Spring microservice");
                    }
                }
                catch (HttpRequestException ex)
                {
                    return StatusCode(500, $"Request to microservice failed: {ex.Message}");
                }
            }
        }
        //OVO MENJAM


        //OVO MENJAM
        [HttpPost("{encounterId}")]
        public async Task<ActionResult<EncounterExecutionStringDto>> Activate(string encounterId, [FromBody] EncounterCoordinateDto currentPosition)
        {
             //   var result = _encounterExecutionService.Activate(encounterId, ClaimsPrincipalExtensions.PersonId(User), currentPosition);
            //    return CreateResponse(result);
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "http://localhost:8083/api/encounter/executions/" + encounterId;
                    string jsonString = JsonConvert.SerializeObject(currentPosition);
                    var response = await client.PostAsJsonAsync(url, currentPosition);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Response from server: " + responseContent);
                        return CreateResponse(Result.Ok(responseContent));
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                        return CreateResponse(Result.Fail("An error occurred"));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                    return CreateResponse(Result.Fail("An error occurred").WithError(ex.Message));
                }
            }
        }
        //OVO MENJAM

        [HttpPatch("{executionId:long}")]
        public ActionResult<EncounterExecutionDto> CheckIfCompleted(long executionId, [FromBody] EncounterCoordinateDto currentPosition)
        {
            var result = _encounterExecutionService.CheckIfCompleted(executionId, ClaimsPrincipalExtensions.PersonId(User), currentPosition);
            return CreateResponse(result);
        }

        //OVO MENJAM
        [HttpPatch("completeMiscEncounter/{executionId}")]
        public async Task<ActionResult<EncounterExecutionDto>> CompleteMiscEnctounter(string executionId)
        {
        //    var result = _encounterExecutionService.CompleteMiscEncounter(executionId, ClaimsPrincipalExtensions.PersonId(User));
        //    return CreateResponse(result);
            var httpClient = new HttpClient();
            var resourceUrl = "http://localhost:8083/api/encounter/executions/completeMiscEncounter/" + executionId; // Replace with your Spring Boot resource URL

            var myResource = new
            {
                // Properties to update
                PropertyName = "New Value"
            };

            var json = JsonConvert.SerializeObject(myResource);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Adding a header if needed, for example, Authorization
            // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "your_token");

            var response = await httpClient.PatchAsync(resourceUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Resource updated successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to update resource. Status code: {response.StatusCode}");
            }
            return CreateResponse(Result.Ok(content));
        }
        //OVO MENJAM

        //OVO MENJAM
        [HttpPatch("abandon/{executionId}")]
        public async Task<ActionResult<EncounterExecutionDto>> Abandon(string executionId) //([FromBody] string executionId)
        {
           // var result = _encounterExecutionService.Abandon(executionId, ClaimsPrincipalExtensions.PersonId(User));
           // return CreateResponse(result);

            var httpClient = new HttpClient();
            var resourceUrl = "http://localhost:8083/api/encounter/executions/" + executionId; // Replace with your Spring Boot resource URL

            var myResource = new
            {
                // Properties to update
                PropertyName = "New Value"
            };

            var json = JsonConvert.SerializeObject(myResource);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Adding a header if needed, for example, Authorization
            // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "your_token");

            var response = await httpClient.PatchAsync(resourceUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Resource updated successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to update resource. Status code: {response.StatusCode}");
            }
            return CreateResponse(Result.Ok(content));
        }
        //OVO MENJAM
    }
}
