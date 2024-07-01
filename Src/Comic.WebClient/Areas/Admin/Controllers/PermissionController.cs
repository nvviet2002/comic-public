using Microsoft.AspNetCore.Mvc;

namespace Comic.WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("{area}/{controller}/")]
    public class PermissionController : Controller
    {
        [HttpGet("")]
        public IActionResult PermissionList()
        {
            return View();
        }
    }
}
