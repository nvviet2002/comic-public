using Comic.Domain.Entities;
using Comic.Domain.Enums;
using Comic.Domain.RequestModels.PaginateModel;
using Comic.Domain.RequestModels.StoryModel;
using Comic.WebClient.ApiClient.Client;
using Comic.WebClient.Areas.Client.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comic.WebClient.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("truyen-tranh")]
    public class StoryController : Controller
    {
        private readonly IStoryApiClient _storyApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        public StoryController(IStoryApiClient storyApiClient, ICategoryApiClient categoryApiClient)
        {
            _storyApiClient = storyApiClient;
            _categoryApiClient = categoryApiClient;
        }

        [HttpGet("{storySlug}/{chapterSlug}/{id}")]
        public async Task<IActionResult> DetailChapter(string storySlug,
           string chapterSlug, string id)
        {
            var chapter = await _storyApiClient.GetDetailChapterAsync(id);
            var story = await _storyApiClient.GetDetailAsync(chapter.StoryId);
            ViewBag.Chapter = chapter;
            ViewBag.Story = story;

            //seo
            var seoModel = new SeoModel()
            {
                CanonicalUrl = Request.GetDisplayUrl(),
                MetaKeywords = chapter.MetaKeyword,
                MetaDescription = chapter.MetaDescription,
            };
            ViewBag.SeoModel = seoModel;

            return View();
        }

        [HttpGet("{slug}/{id}")]
        public async Task<IActionResult> Detail(string slug, string id)
        {
            var story = await _storyApiClient.GetDetailAsync(id);
            ViewBag.Story = story;

            //seo
            var seoModel = new SeoModel()
            {
                CanonicalUrl = Request.GetDisplayUrl(),
                MetaKeywords = story.MetaKeyword,
                MetaDescription = story.MetaDescription,
            };
            ViewBag.SeoModel = seoModel;

            return View();
        }

       

        [HttpGet("the-loai/{slug}")]
        public async Task<IActionResult> Search(string slug,string? id,int? page
            , StoryStatus? status, SortOption? sort, string? searchTerm )
        {
            var searchReq = new StorySearchReq()
            {
                PaginateReq = new PaginateReq()
                {
                    PageNumber = page ?? 1,
                    PageSize = 36,
                    SearchTerm = searchTerm ?? "",
                },
                CategoryId = id,
                SortOption = sort,
                Status = status,
            };

            var categories = await _categoryApiClient.GetActivedAllAsync();
            var paginatedStories = await _storyApiClient.SearchPagingAsync(searchReq);
            ViewBag.Categories = categories;
            ViewBag.CategoryId = id;
            ViewBag.CategorySlug = slug;
            ViewBag.StoryStatus = status;
            ViewBag.StorySort = sort;
            ViewBag.PaginatedStories = paginatedStories;

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
