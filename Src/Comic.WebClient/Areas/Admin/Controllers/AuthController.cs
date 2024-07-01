using Microsoft.AspNetCore.Mvc;

namespace Comic.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("{area}/{controller}/")]
    public class AuthController : Controller
    {

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }
    }
}
