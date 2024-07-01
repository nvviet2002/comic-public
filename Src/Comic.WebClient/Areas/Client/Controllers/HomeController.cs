using Comic.Domain.RequestModels.PaginateModel;
using Comic.WebClient.ApiClient.Client;
using Comic.WebClient.Areas.Client.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Comic.WebClient.Areas.Client.Controllers
{
    [Area("Client")]
    public class HomeController : Controller
    {
        private readonly IStoryApiClient _storyApiClient;

        public HomeController(IStoryApiClient storyApiClient)
        {
            _storyApiClient = storyApiClient;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index([FromQuery]int? page)
        {
            //get recommeded stories
            var newRecommendReq = new PaginateReq()
            {
                PageNumber = 1,
                PageSize = 10,
                SearchTerm = ""
            };
            var recommendStories = await _storyApiClient.GetRecommendAsync(newRecommendReq);
            ViewBag.PaginatedStories = recommendStories;

            //get paginated stories
            var newPaginateReq = new PaginateReq()
            {
                PageNumber = page ?? 1,
                PageSize = 36,
                SearchTerm = ""
            };
            var paginatedStories = await _storyApiClient.GetPagingAsync(newPaginateReq);
            ViewBag.PaginatedStories = paginatedStories;
            ViewBag.RecommendStories = recommendStories;

            //seo
            var seoModel = new SeoModel()
            {
                CanonicalUrl = Request.GetDisplayUrl(),
            };
            ViewBag.SeoModel = seoModel;

            return View();
        }

    }
}
