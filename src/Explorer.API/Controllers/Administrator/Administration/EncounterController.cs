using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.UseCases.Tourist.Execution;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain.Tours;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;

namespace Explorer.API.Controllers.Administrator.Administration
{
    [Route("api/administration/encounter")]

    public class EncounterController : BaseApiController
    {
        private readonly IEncounterService _encountersService;

        public EncounterController(IEncounterService encountersService)
        {
            _encountersService = encountersService;
        }


        //OVO MENJAM
        [HttpGet]
        public async Task<ActionResult<PagedResult<EncounterStringDto>>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
        //   var result = _encountersService.GetAllCheckpointUnrelated();
        //    return CreateResponse(result);

            using (HttpClient client = new HttpClient())
            {
                string url = "http://localhost:8083/api/encounters";
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

        //OVO CU SAD DA MENJAM
        [HttpGet("statistics/{encounterId}")]
        public async  Task<ActionResult<EncounterStatisticsStringDto>> GetStatistics(string encounterId)
        {
            //   var result = _encountersService.GetStatistics(encounterId);
            //  return CreateResponse(result);
            
            using (HttpClient client = new HttpClient())
            {
                string url = "http://localhost:8083/api/encounters/statistics/" + encounterId;
                try
                {
                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadFromJsonAsync<EncounterStatisticsStringDto>();
                        
                        return Ok(responseData);
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
        //OVO CU SAD DA MENJAM

        [HttpGet("checkpoint")]
        public ActionResult<EncounterDto> GetAllForCheckpoint()
        {
            var result = _encountersService.GetAllCheckpointRelated();
            return CreateResponse(result);
        }

        [HttpGet("checkpoint/{checkpointId:long}")]
        public ActionResult<EncounterDto> GetForCheckpoint(long checkpointId)
        {
            var result = _encountersService.GetForCheckpoint(checkpointId);
            return CreateResponse(result);
        }

        // OVO MENJAM
        [HttpPost]
        public async Task<ActionResult<EncounterStringDto>> Create([FromBody] EncounterStringDto encounter)
        {
            //    var result = _encountersService.Create(encounter);
            //    return CreateResponse(result);
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "http://localhost:8083/api/encounters";
                    string jsonString = JsonConvert.SerializeObject(encounter);
                    var response = await client.PostAsJsonAsync(url, encounter);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Response from server: " + responseContent);
                        return CreateResponse(Result.Ok(response));
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
        // OVO MENJAM

        //OVO MENJAM
        [HttpPut]
        public async Task<ActionResult<EncounterStringDto>> Update([FromBody] EncounterStringDto encounter)
        {
            //    var result = _encountersService.Update(encounter);
            //    return CreateResponse(result);
            using (HttpClient client = new HttpClient())
            {
                try
                {

                    string url = "http://localhost:8083/api/encounters/update/" + encounter.Id;
                    string jsonString = JsonConvert.SerializeObject(encounter);
                    var response = await client.PutAsJsonAsync(url, encounter);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Response from server: " + responseContent);
                        return CreateResponse(Result.Ok(response));
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

        //OVO MENJAM
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
        //    var result = _encountersService.Delete(id);
        //    return CreateResponse(result);
            using (HttpClient client = new HttpClient())
            {
                string url = "http://localhost:8083/api/encounters/" + id; // Change this URL as per your Spring Boot API endpoint
                // Send a DELETE request to the specified URL
                HttpResponseMessage response = await client.DeleteAsync(url);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("DELETE request successful");
                }
                else
                {
                    Console.WriteLine($"DELETE request failed with status code {response.StatusCode}");
                }
            }
        }
        //OVO MENJAM
    }
}
