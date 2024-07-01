using Microsoft.AspNetCore.Mvc;

namespace Comic.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("{area}/{controller}/")]
    public class DashboardController : Controller
    {
        [HttpGet("")]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
