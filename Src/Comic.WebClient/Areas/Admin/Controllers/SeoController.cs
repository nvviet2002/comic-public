using AspNetCore.SEOHelper.Sitemap;
using Comic.Domain.Entities;
using Comic.Domain.ResponseModels.PaginateModel;
using Comic.WebClient.ApiClient.Client;
using Comic.WebClient.Areas.Client.Controllers;
using Comic.WebClient.Common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Comic.WebClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("{area}/seo")]
    public class SeoController : Controller
    {
        private readonly IStoryApiClient _storyApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly ILogger<SeoController> _logger;
        private readonly IWebHostEnvironment _env;

        public SeoController(IStoryApiClient storyApiClient, ICategoryApiClient categoryApiClient
            , ILogger<SeoController> logger, IWebHostEnvironment env)
        {
            _storyApiClient = storyApiClient;
            _categoryApiClient = categoryApiClient;
            _logger = logger;
            _env = env;
        }


        [HttpGet("")]
        public IActionResult SeoIndex()
        {
            return View();
        }

        [HttpGet("create-sitemap")]
        public async Task<IActionResult> CreateSitemapAsync()
        {
            try
            {
                var categories = await _categoryApiClient.GetActivedAllAsync();
                var stories = await _storyApiClient.GetActivedAllAsync();

                var sitemaps = new List<SitemapNode>();
                sitemaps.Add(
                    new SitemapNode
                    {
                        LastModified = DateTime.UtcNow,
                        Priority = 1,
                        Url = $"{Request.Scheme}://{Request.Host}",
                        Frequency = SitemapFrequency.Daily
                    });
                sitemaps.Add(
                       new SitemapNode
                       {
                           LastModified = DateTime.UtcNow,
                           Priority = 0.8,
                           Url = $"{Request.Scheme}://{Request.Host}/truyen-tranh/the-loai/tat-ca",
                           Frequency = SitemapFrequency.Daily
                       });
                foreach (var category in categories)
                {
                    sitemaps.Add(
                        new SitemapNode
                        {
                            LastModified = DateTime.UtcNow,
                            Priority = 0.8,
                            Url = $"{Request.Scheme}://{Request.Host}/truyen-tranh/the-loai/{category.Slug}?id={category.Id}",
                            Frequency = SitemapFrequency.Daily
                        });
                }

                //add story link
                foreach (var story in stories)
                {
                    sitemaps.Add(
                        new SitemapNode
                        {
                            LastModified = DateTime.UtcNow,
                            Priority = 0.7,
                            Url = $"{Request.Scheme}://{Request.Host}/truyen-tranh/{story.Slug}/{story.Id}",
                            Frequency = SitemapFrequency.Daily
                        });
                    foreach (var chapter in story.Chapters)
                    {
                        sitemaps.Add(
                       new SitemapNode
                       {
                           LastModified = DateTime.UtcNow,
                           Priority = 0.6,
                           Url = $"{Request.Scheme}://{Request.Host}/truyen-tranh/{story.Slug}/{chapter.Slug}/{chapter.Id}",
                           Frequency = SitemapFrequency.Daily
                       });
                    }
                }

                new SitemapDocument().CreateSitemapXML(sitemaps, _env.WebRootPath);
                return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Tạo sitemap thành công!", null));
            }
            catch(Exception ex)
            {
                return Ok(new ApiResponse<dynamic>(StatusCodes.Status500InternalServerError, ex.Message, null));
            }

            

        }
    }
}
