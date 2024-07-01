using Microsoft.AspNetCore.Mvc;

namespace Comic.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("{area}/{controller}/")]
    public class UserController : Controller
    {
        [HttpGet("")]
        public IActionResult UserList()
        {
            return View();
        }
    }
}
