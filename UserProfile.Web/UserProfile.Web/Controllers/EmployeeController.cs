using Microsoft.AspNetCore.Mvc;

namespace UserProfile.Web.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
