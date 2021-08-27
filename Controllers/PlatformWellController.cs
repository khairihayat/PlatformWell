using System.Collections.Generic;
using AutoMapper;
using PlatformWell.Data;
using PlatformWell.Dtos;
using PlatformWell.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using PlatformWell.Helpers;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PlatformWell.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlatformWellController : ControllerBase
    {
        private readonly IPlatformRepository _repository;
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;

        public PlatformWellController(ApplicationContext context, IPlatformRepository repository, IMapper mapper)
        {
            _context = context;
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/platforms
        [HttpGet("AllPlatformWell", Name = "GetAllPlatformWell")]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
        {
            var platformItems = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        //GET api/platforms/{id}
        [HttpGet("{id}", Name="GetPlatformById")]
        public ActionResult <PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = _repository.GetPlatformById(id);
            if(platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }
            return NotFound();
        }

        [HttpGet("GetPlatformWellActual", Name = "GetPlatformWellActual")]
        public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetPlatformWellActual()
        {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyQGFlbWVuZXJzb2wuY29tIiwianRpIjoiYzBhMzBhNzItNTAyMi00NjhiLTllZGQtOTQwMGIyZjAxMjMyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzMzE4ZTcxMC05MzAzLTQ4ZmQtODNjNS1mYmNhOTU0MTExZWYiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiZXhwIjoxNjMyMzc1MjY3LCJpc3MiOiJodHRwOi8vdGVzdC1kZW1vLmFlbWVuZXJzb2wuY29tIiwiYXVkIjoiaHR0cDovL3Rlc3QtZGVtby5hZW1lbmVyc29sLmNvbSJ9.WUJIDR5Mf96ZnO9ndU4XRZGS0C4WcnxfAc_IxiAGg3k";
            string baseUrl = "http://test-demo.aemenersol.com";
            string action = "/api/PlatformWell/GetPlatformWellActual";
            var callApi = new CallApi(baseUrl);
            var client = callApi.getClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage callresponse = await client.GetAsync(action);
            var result = new List<Platform>();
            if (callresponse.IsSuccessStatusCode)
            {
                var stringResponse = await callresponse.Content.ReadAsStringAsync();
                result = System.Text.Json.JsonSerializer.Deserialize<List<Platform>>(stringResponse,
                    new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                foreach (var item in result)
                {
                    var platform = _context.Platforms
                                        .Include(x => x.Well)
                                        .FirstOrDefault(x => x.Id == item.Id);

                    if (platform != null)
                    {

                        platform.UniqueName = item.UniqueName;
                        platform.Latitude = item.Latitude;
                        platform.Longitude = item.Longitude;
                        platform.CreatedAt = item.CreatedAt;
                        platform.UpdatedAt = item.UpdatedAt;

                        _context.SetModified(platform);
                        _context.SaveChanges();

                        foreach (var well in item.Well)
                        {
                            var wellexist = _context.Wells.FirstOrDefault(x => x.Id == well.Id && x.PlatformId == platform.Id);

                            if (wellexist != null)
                            {
                                wellexist.UniqueName = well.UniqueName;
                                wellexist.Latitude = well.Latitude;
                                wellexist.Longitude = well.Longitude;
                                wellexist.CreatedAt = well.CreatedAt;
                                wellexist.UpdatedAt = well.UpdatedAt;

                                _context.SetModified(wellexist);
                                _context.SaveChanges();
                            }
                            else
                            {
                                var wellCreate = new Well
                                {
                                    Id = well.Id,
                                    PlatformId = well.PlatformId,
                                    UniqueName = well.UniqueName,
                                    Latitude = well.Latitude,
                                    Longitude = well.Longitude,
                                    CreatedAt = well.CreatedAt,
                                    UpdatedAt = well.UpdatedAt,
                                };
                                _context.Wells.Add(wellCreate);
                                _context.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        var platformCreate = new Platform
                        {
                            Id = item.Id,
                            UniqueName = item.UniqueName,
                            Latitude = item.Latitude,
                            Longitude = item.Longitude,
                            CreatedAt = item.CreatedAt,
                            UpdatedAt = item.UpdatedAt,
                            Well = item.Well.Select(x => new Well
                            {
                                Id = x.Id,
                                PlatformId = x.PlatformId,
                                UniqueName = x.UniqueName,
                                Latitude = x.Latitude,
                                Longitude = x.Longitude,
                                CreatedAt = x.CreatedAt,
                                UpdatedAt = x.UpdatedAt,
                            }).ToList()
                        };

                        _context.Platforms.Add(platformCreate);
                        _context.SaveChanges();
                    }
                }

                //return Ok(await callresponse.Content.ReadAsStringAsync());

            }
            var platformItems = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }


        [HttpGet("GetPlatformWellDummy", Name = "GetPlatformWellDummy")]
        public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetPlatformWellDummy()
        {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c2VyQGFlbWVuZXJzb2wuY29tIiwianRpIjoiYzBhMzBhNzItNTAyMi00NjhiLTllZGQtOTQwMGIyZjAxMjMyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzMzE4ZTcxMC05MzAzLTQ4ZmQtODNjNS1mYmNhOTU0MTExZWYiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVc2VyIiwiZXhwIjoxNjMyMzc1MjY3LCJpc3MiOiJodHRwOi8vdGVzdC1kZW1vLmFlbWVuZXJzb2wuY29tIiwiYXVkIjoiaHR0cDovL3Rlc3QtZGVtby5hZW1lbmVyc29sLmNvbSJ9.WUJIDR5Mf96ZnO9ndU4XRZGS0C4WcnxfAc_IxiAGg3k";
            string baseUrl = "http://test-demo.aemenersol.com";
            string action = "/api/PlatformWell/GetPlatformWellDummy";
            var callApi = new CallApi(baseUrl);
            var client = callApi.getClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage callresponse = await client.GetAsync(action);
            var result = new List<Platform>();
            if (callresponse.IsSuccessStatusCode)
            {
                var stringResponse = await callresponse.Content.ReadAsStringAsync();
                result = System.Text.Json.JsonSerializer.Deserialize<List<Platform>>(stringResponse,
                    new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                foreach (var item in result)
                {
                    var platform = _context.Platforms
                                        .Include(x => x.Well)
                                        .FirstOrDefault(x => x.Id == item.Id);

                    if (platform != null)
                    {

                        platform.UniqueName = item.UniqueName;
                        platform.Latitude = item.Latitude;
                        platform.Longitude = item.Longitude;
                        platform.CreatedAt = item.CreatedAt;
                        platform.UpdatedAt = item.UpdatedAt;

                        _context.SetModified(platform);
                        _context.SaveChanges();

                        foreach (var well in item.Well)
                        {
                            var wellexist = _context.Wells.FirstOrDefault(x => x.Id == well.Id && x.PlatformId == platform.Id);

                            if (wellexist != null)
                            {
                                wellexist.UniqueName = well.UniqueName;
                                wellexist.Latitude = well.Latitude;
                                wellexist.Longitude = well.Longitude;
                                wellexist.CreatedAt = well.CreatedAt;
                                wellexist.UpdatedAt = well.UpdatedAt;

                                _context.SetModified(wellexist);
                                _context.SaveChanges();
                            }
                            else
                            {
                                var wellCreate = new Well
                                {
                                    Id = well.Id,
                                    PlatformId = well.PlatformId,
                                    UniqueName = well.UniqueName,
                                    Latitude = well.Latitude,
                                    Longitude = well.Longitude,
                                    CreatedAt = well.CreatedAt,
                                    UpdatedAt = well.UpdatedAt,
                                };
                                _context.Wells.Add(wellCreate);
                                _context.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        var platformCreate = new Platform
                        {
                            Id = item.Id,
                            UniqueName = item.UniqueName,
                            Latitude = item.Latitude,
                            Longitude = item.Longitude,
                            CreatedAt = item.CreatedAt,
                            UpdatedAt = item.UpdatedAt,
                            Well = item.Well.Select(x => new Well
                            {
                                Id = x.Id,
                                PlatformId = x.PlatformId,
                                UniqueName = x.UniqueName,
                                Latitude = x.Latitude,
                                Longitude = x.Longitude,
                                CreatedAt = x.CreatedAt,
                                UpdatedAt = x.UpdatedAt,
                            }).ToList()
                        };

                        _context.Platforms.Add(platformCreate);
                        _context.SaveChanges();
                    }
                }

                //return Ok(await callresponse.Content.ReadAsStringAsync());

            }
            var platformItems = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        //PUT api/platforms/{id}
        [HttpPut("UpdatePlatformWellById/{id}", Name = "UpdatePlatformWellById")]
        public ActionResult UpdatePlatform(int id, PlatformUpdateDto platformUpdateDto)
        {
            var platformModelFromRepo = _repository.GetPlatformById(id);
            if (platformModelFromRepo == null)
            {
                return NotFound();
            }
            _mapper.Map(platformUpdateDto, platformModelFromRepo);

            _repository.UpdatePlatform(platformModelFromRepo);

            _repository.SaveChanges();

            return NoContent();
        }

        ////POST api/platforms
        //[HttpPost]
        //public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto)
        //{
        //    var platformModel = _mapper.Map<Platform>(platformCreateDto);
        //    _repository.CreatePlatform(platformModel);
        //    _repository.SaveChanges();

        //    var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

        //    return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
        //}

        ////PATCH api/platforms/{id}
        //[HttpPatch("{id}")]
        //public ActionResult PartialPlatformUpdate(int id, JsonPatchDocument<PlatformUpdateDto> patchDoc)
        //{
        //    var platformModelFromRepo = _repository.GetPlatformById(id);
        //    if (platformModelFromRepo == null)
        //    {
        //        return NotFound();
        //    }

        //    var platformToPatch = _mapper.Map<PlatformUpdateDto>(platformModelFromRepo);
        //    patchDoc.ApplyTo(platformToPatch, ModelState);

        //    if (!TryValidateModel(platformToPatch))
        //    {
        //        return ValidationProblem(ModelState);
        //    }

        //    _mapper.Map(platformToPatch, platformModelFromRepo);

        //    _repository.UpdatePlatform(platformModelFromRepo);

        //    _repository.SaveChanges();

        //    return NoContent();
        //}

        ////DELETE api/platforms/{id}
        //[HttpDelete("{id}")]
        //public ActionResult DeletePlatform(int id)
        //{
        //    var platformModelFromRepo = _repository.GetPlatformById(id);
        //    if (platformModelFromRepo == null)
        //    {
        //        return NotFound();
        //    }
        //    _repository.DeletePlatform(platformModelFromRepo);
        //    _repository.SaveChanges();

        //    return NoContent();
        //}

    }
}