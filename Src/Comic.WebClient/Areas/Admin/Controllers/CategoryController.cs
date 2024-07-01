using Microsoft.AspNetCore.Mvc;

namespace Comic.WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("{area}/{controller}/")]
    public class CategoryController : Controller
    {
        [HttpGet("")]
        public IActionResult CategoryList()
        {
            return View();
        }
    }
}
