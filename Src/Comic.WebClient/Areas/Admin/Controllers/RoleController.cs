using Microsoft.AspNetCore.Mvc;

namespace Comic.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("{area}/{controller}/")]
    public class RoleController : Controller
    {
        [HttpGet("")]
        public IActionResult RoleList()
        {
            return View();
        }
    }
}
