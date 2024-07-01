using Microsoft.AspNetCore.Mvc;

namespace Comic.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("{area}/{controller}/")]
    public class AdminController : Controller
    {
        [HttpGet("error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
