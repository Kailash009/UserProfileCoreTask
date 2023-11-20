using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserProfile.API.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        [HttpGet]
        public IActionResult Admin()
        {
            return Ok("Hello Admin!!");
        }
    }
}
