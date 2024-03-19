using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Explorer.Tours.Core.Domain.Tours;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Xml.Linq;

namespace Explorer.API.Controllers.Author
{
    [Route("api/tour")]
    public class TourController : BaseApiController
    {
        private readonly ITourService _tourService;
        private readonly ICheckpointService _checkpointService;

        public TourController(ITourService tourService, ICheckpointService checkpointService)
        {
            _tourService = tourService;
            _checkpointService = checkpointService;
        }

        [HttpGet]
        public ActionResult<PagedResult<TourDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }


        [HttpGet("authortours")]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResult<TourStringDto>>> GetAllAuthorTours([FromQuery] int authorId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "http://localhost:8081/api/tours/for_author/" + authorId;
                try
                {
                    var response = await client.GetAsync(url);
                 
                    if (response.IsSuccessStatusCode)
                    {
                         
                        var responseData = await response.Content.ReadFromJsonAsync<List<TourStringDto>>();
                        var pagedResult = new PagedResult<TourStringDto>(responseData, responseData.Count);

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

        [HttpGet("singletour")]
        public ActionResult<TourDto> GetById([FromQuery] int tourId) {
            var result = _tourService.Get(tourId);
            return CreateResponse(result);
        }

        [HttpGet("singletour/{tourId}")]
        public async Task<ActionResult<TourDto>> GetTour(string tourId)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "http://localhost:8081/api/tours/"+tourId;
                try
                {
                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {

                        var responseData = await response.Content.ReadFromJsonAsync<TourStringDto>();
                        //var pagedResult = new PagedResult<TourStringDto>(responseData, responseData.Count);

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

        [HttpPut("archiveTour/{tourId}")]
        public async Task<ActionResult<TourStringDto>> ArchiveTour(string tourId)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "http://localhost:8081/api/tours/archive/" + tourId;
                    TourStringDto prenos = new TourStringDto();
                    prenos.Id = tourId;
                    string jsonString = JsonConvert.SerializeObject(prenos);
                    var response = await client.PutAsJsonAsync(url, prenos);

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

        [HttpPut("updatetour")]
        public async Task<ActionResult<TourStringDto>> Update([FromBody] TourStringDto tourDto)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "http://localhost:8081/api/tours/" + tourDto.Id;
                    string jsonString = JsonConvert.SerializeObject(tourDto);
                    var response = await client.PutAsJsonAsync(url, tourDto);

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

        [HttpPost]
        public async Task<ActionResult<TourStringDto>> Create([FromBody] TourDto tour)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "http://localhost:8081/api/tours";
                    string jsonString = JsonConvert.SerializeObject(tour);
                    var response = await client.PostAsJsonAsync(url, tour);

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

        [HttpPost("addCheckpoint")]
        public async Task<ActionResult<CheckpointStringDto>> AddCheckpointOnTour([FromBody] CheckpointTourStringDto checkpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "http://localhost:8081/api/checkpoints";
                    string jsonString = JsonConvert.SerializeObject(checkpoint);
                    var response = await client.PostAsJsonAsync(url, checkpoint);

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


        //[HttpPost("addCheckpoint")]
        //public ActionResult<CheckpointDto> AddCheckpointOnTour([FromBody] CheckpointDto checkpointDto)
        //{
        //    var result = _checkpointService.Create(checkpointDto);
        //    return CreateResponse(result);
        //}

        [HttpPatch("addCheckpoint/{tourId:int}")]

        public void AddCheckPointOnTour(int? tourId, [FromBody] CheckpointDto checkpointDto)
        {
            _checkpointService.Create(checkpointDto);
        }

        //ovo treba
        //[HttpGet("getCheckpoints/{tourId:int}")]

        //public ActionResult<PagedResult<CheckpointDto>> GetAllByTourId([FromQuery] int page, [FromQuery] int pageSize, int tourId)
        //{
        //    var result = _checkpointService.GetAllByTourId(page, pageSize, tourId);
        //    return CreateResponse(result);
        //}

        [HttpGet("toursCheckpoints")]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResult<CheckpointStringDto>>> GetAllToursCheckpoints([FromQuery] string tourId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "http://localhost:8081/api/checkpoints/for_tour/" + tourId;
                try
                {
                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {

                        var responseData = await response.Content.ReadFromJsonAsync<List<CheckpointStringDto>>();
                        var pagedResult = new PagedResult<CheckpointStringDto>(responseData, responseData.Count);

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

        //ovo treba
        //[HttpPut("updateCheckpoint")]
        //public ActionResult<CheckpointDto> UpdateCheckpoint([FromBody] CheckpointDto checkpointDto)
        //{
        //    var result = _checkpointService.Update(checkpointDto);
        //    return CreateResponse(result);
        //}

        [HttpPut("updateCheckpoint")]
        public async Task<ActionResult<CheckpointStringDto>> Update([FromBody] CheckpointStringDto checkpointDto)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "http://localhost:8081/api/checkpoints/" + checkpointDto.Id;
                    string jsonString = JsonConvert.SerializeObject(checkpointDto);
                    var response = await client.PutAsJsonAsync(url, checkpointDto);

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

        //ovo treba
        //[HttpDelete("deleteCheckpoint/{checkpointId:int}")]
        //public ActionResult<CheckpointDto> DeleteCheckpoint(int checkpointId)
        //{
        //    var result = _checkpointService.Delete(checkpointId);
        //    return CreateResponse(result);
        //}

        [HttpDelete("deleteCheckpoint/{id}")]
        public async void Delete(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "http://localhost:8081/api/checkpoints/" + id;
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

        [HttpGet("tourist/checkpoints")]
        public ActionResult<PagedResult<CheckpointDto>> GetTouristCheckpoints([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _checkpointService.GetAllPublicCheckpoints(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPost("acceptRequest/{checkpointId:long}")]
        public ActionResult<CheckpointDto> AcceptRequest(int checkpointId)
        {
            var result = _checkpointService.AcceptCheckpoint(checkpointId);
            return CreateResponse(result);
        }

        [HttpPost("declineRequest/{checkpointId:int}")]
        public ActionResult<CheckpointDto> DeclineRequest(int checkpointId, [FromBody] string comment)
        {
            var result = _checkpointService.RejectCheckpoint(checkpointId, comment);
            return CreateResponse(result);
        }

        //ovo treba
        [HttpPut("publishTour")]
        public ActionResult<TourDto> PublishTour([FromBody] int tourId)
        {
            if (_checkpointService.CheckPointsAreValidForPublish(0, 0, tourId))
            {
                _tourService.PublishTour(tourId, DateTime.Now.ToUniversalTime());
                var tour = _tourService.Get(tourId);
                return CreateResponse(tour);
            }
            else
            {
                throw new Exception("Tour doesn't have enough checkpoints to be published ");
            }
        }

        //[HttpPut("archiveTour")]
        //public ActionResult<TourDto> ArchiveTour([FromBody] int tourId)
        //{
        //    _tourService.ArchiveTour(tourId, DateTime.Now.ToUniversalTime());
        //    var tour = _tourService.Get(tourId);
        //    return CreateResponse(tour);
        //}

        [HttpGet("shopping/{userId:int}")]
        public ActionResult<PagedResult<TourPreviewDto>> GetAllAvailableTours([FromQuery] int page, [FromQuery] int pageSize, int userId)
        {
            var result = _tourService.GetAllAvailableTours(page, pageSize, userId);
            return CreateResponse(result);
        }
        
        [HttpGet("touristTours/{userId:int}")]
        public ActionResult<PagedResult<TourPreviewDto>> GetTouristTours([FromQuery] int page, [FromQuery] int pageSize, int userId)
        {
            var result = _tourService.GetTouristTours(page, pageSize, userId);
            return CreateResponse(result);

        }

        [HttpGet("publishedauthortours")]
        public ActionResult<PagedResult<TourDto>> GetPublishedAuthorTours([FromQuery] int authorId, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourService.GetPublishedAuthorTours(authorId, page, pageSize);
            return CreateResponse(result);
        }
        
        [HttpGet("administrator/checkpoints")]
        [AllowAnonymous]
        public ActionResult<PagedResult<CheckpointDto>> GetAdministratorCheckpoints([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _checkpointService.GetAllAdministratorCheckpoints(page, pageSize);
            return CreateResponse(result);
        }
        
        [HttpGet("suggestedTours")]
        [AllowAnonymous]
        public ActionResult<PagedResult<TourDto>> GetSuggestedTours([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] long[] ids)
        {
            var result = _tourService.GetSuggestions(page, pageSize, ids);
            return CreateResponse(result);
        }
        
        [HttpPost("{tourId:int}/checkpoints")]
        [AllowAnonymous]
        public void AddCheckpointsOnTour(int tourId, [FromBody] List<int> checkpoints)
        {
            _tourService.AddCheckpoints(checkpoints, tourId);
        }
        
        [HttpPost("{tourId:int}/favorite")]
        [AllowAnonymous]
        public void AddFavoriteTour(int tourId)
        {
            var userId = ClaimsPrincipalExtensions.UserId(User);
            _tourService.AddFavoriteTour(userId, tourId);
        }
        
        [HttpDelete("{tourId:int}/favorite")]
        [AllowAnonymous]
        public void RemoveFavoriteTour(int tourId)
        {
            var userId = ClaimsPrincipalExtensions.UserId(User);
            _tourService.RemoveFavoriteTour(userId, tourId);
        }
    }
}
