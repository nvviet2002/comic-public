using Microsoft.AspNetCore.Mvc;

namespace Comic.WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("{area}/{controller}/")]
    public class StoryController : Controller
    {
        [HttpGet("")]
        public IActionResult StoryList()
        {
            return View();
        }

    }
}
