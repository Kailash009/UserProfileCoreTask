using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserProfile.BAL.Repository.Contract;
using UserProfile.DAL.Models;
using UserProfile.DAL.Models.Dtos;

namespace UserProfile.API.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAccountRepository _user;
        public UserController(IAccountRepository user)
        {
            _user = user;
        }
        [HttpGet("GetUserProfileWithAddress")]
        public async Task<ResponseModel> GetUserProfileWithAddress([FromQuery] int Uid)
        {
            ResponseModel response = await _user.GetUserProfileWithAddress(Uid);
            return response;
        }
    }
}
