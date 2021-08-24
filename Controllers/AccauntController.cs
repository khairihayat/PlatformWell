using System.Collections.Generic;
using AutoMapper;
using PlatformWell.Data;
using PlatformWell.Dtos;
using PlatformWell.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using PlatformWell.Helpers;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PlatformWell.Controllers
{
    public class AccauntController : BaseController
    {

        private readonly IPlatformRepository _repository;
        private readonly IMapper _mapper;
        private IConfiguration _config;

        private readonly ApplicationContext _context;


        public AccauntController(ApplicationContext context, IConfiguration config, IPlatformRepository repository, IMapper mapper)
        {
            _context = context;
            _config = config;
            _repository = repository;
            _mapper = mapper;
        }

        private string GenerateJSONWebToken(LoginModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        private async Task<LoginModel> AuthenticateUser(LoginModel login)
        {
            LoginModel user = null;

            if (login.UserName == "user@aemenersol.com")
            {
                user = new LoginModel { UserName = "user@aemenersol.com", Password = "Test@123" };
            }
            return user;
        }
        
        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] LoginModel data)
        {
            IActionResult response = Unauthorized();
            var user = await AuthenticateUser(data);
            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { Token = tokenString, Message = "Success" });

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
                else
                    return NotFound();
            }
            return response;
        }

        
        [HttpGet(nameof(Get))]
        public async Task<IEnumerable<string>> Get()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            return new string[] { accessToken };
        }


    }

    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}