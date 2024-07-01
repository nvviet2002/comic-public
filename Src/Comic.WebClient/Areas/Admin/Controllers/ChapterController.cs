using Microsoft.AspNetCore.Mvc;

namespace Comic.WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("{area}/story/{storyId}/{controller}/")]
    public class ChapterController : Controller
    {
        [HttpGet("")]
        public IActionResult ChapterList([FromRoute] string storyId)
        {
            ViewBag.StoryId = storyId;
            return View();
        }
    }
}
