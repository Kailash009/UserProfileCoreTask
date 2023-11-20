using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UserProfile.BAL.Repository.Contract;
using UserProfile.DAL.Models;
using UserProfile.DAL.Models.Dtos;

namespace UserProfile.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _user;
        public AccountController(IAccountRepository user)
        {
            _user = user;
        }

        [HttpPost("register")]
        public async Task<ResponseModel> Register([FromBody] RegisterDto reg,string role)
        {
            ResponseModel response = await _user.SignUp(reg,role);
            return response;
        }
        [HttpGet("login")]
        public async Task<ResponseModel> Login([FromQuery] LoginDto logDto)
        {
            ResponseModel response = await _user.Login(logDto);
            return response;
        }

        [HttpPost("AddAddressForUsers")]
        public async Task<ResponseModel> AddAddressForUsers([FromBody] UserDto userDto)
        {
            ResponseModel response = await _user.AddAddressForUsers(userDto);
            return response;
        }
    }
}


