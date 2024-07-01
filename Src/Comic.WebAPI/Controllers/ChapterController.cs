using Comic.Application.IServices;
using Comic.Application.Services;
using Comic.Domain.Contrants;
using Comic.Domain.Entities;
using Comic.Domain.Enums;
using Comic.Domain.RequestModels.ChapterModel;
using Comic.Domain.RequestModels.DatatableModel;
using Comic.Domain.RequestModels.StoryModel;
using Comic.Domain.ResponseModels.ChapterModel;
using Comic.Domain.ResponseModels.DatatableModel;
using Comic.Domain.ResponseModels.StoryModel;
using Comic.WebAPI.Common;
using Comic.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Comic.WebAPI.Controllers
{
    [Route("api/story/{storyId?}/chapter")]
    [ApiController]
    public class ChapterController : Controller
    {
        private readonly IChapterService _chapterService;
        private readonly IStoryService _storyService;

        private readonly ILogger<ChapterController> _logger;

        public ChapterController(IChapterService chapterService, ILogger<ChapterController> logger
            , IStoryService storyService)
        {
            _chapterService = chapterService;
            _storyService = storyService;
            _logger = logger;
        }

        [HttpPost("list-datatable")]
        [Authorization(PermissionNames.EditChapter)]
        public async Task<IActionResult> ListDatatableAsync([FromRoute]string storyId,[FromBody] DatatableReq datatableReq)
        {
            var chapters = await _chapterService.GetDatatableAsync(storyId, datatableReq);

            return Ok(new ApiResponse<DatatableRes<ChapterItemRes>>(StatusCodes.Status200OK, "Lấy danh sách chapter thành công!", chapters));
        }


        [HttpPost("create")]
        [Authorization(PermissionNames.EditChapter)]
        public async Task<IActionResult> CreateAsync([FromRoute] string storyId, [FromForm] ChapterReq chapterReq)
        {
            chapterReq.Name = chapterReq.Name ?? string.Empty;
            chapterReq.Title = chapterReq.Title ?? string.Empty;
            chapterReq.MetaDescription = chapterReq.MetaDescription ?? string.Empty;
            chapterReq.MetaKeyword = chapterReq.MetaKeyword ?? string.Empty;
            chapterReq.Status = chapterReq.Status ?? ChapterStatus.Raw;
            chapterReq.HotFlag = chapterReq.HotFlag ?? true;
            chapterReq.IsActived = chapterReq.IsActived ?? true;
            chapterReq.View = chapterReq.View ?? 0;
            chapterReq.Index = chapterReq.Index ?? 0;
            await _chapterService.CreateAsync(storyId, chapterReq);
            //refresh view of story
            await _storyService.RefreshViewAsync(storyId);
            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Thêm chapter mới thành công!", null));
        }

        [HttpDelete("delete/{id}")]
        [Authorization(PermissionNames.EditChapter)]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            await _chapterService.DeleteAsync(id);
            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Xóa chapter thành công!", null));
        }

        [HttpGet("get/{id}")]
        [Authorization(PermissionNames.EditChapter)]
        public async Task<IActionResult> GetAsync([FromRoute] string id)
        {
            var chapter = await _chapterService.GetAsync(id);
            return Ok(new ApiResponse<ChapterRes>(StatusCodes.Status200OK, "Lấy thông tin chapter thành công!", chapter));
        }

        [HttpPut("update/{id}")]
        [Authorization(PermissionNames.EditChapter)]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromForm] ChapterReq chapterReq)
        {
            chapterReq.Name = chapterReq.Name ?? string.Empty;
            chapterReq.Title = chapterReq.Title ?? string.Empty;
            chapterReq.MetaDescription = chapterReq.MetaDescription ?? string.Empty;
            chapterReq.MetaKeyword = chapterReq.MetaKeyword ?? string.Empty;
            chapterReq.Status = chapterReq.Status ?? ChapterStatus.Raw;
            chapterReq.HotFlag = chapterReq.HotFlag ?? true;
            chapterReq.IsActived = chapterReq.IsActived ?? true;
            chapterReq.View = chapterReq.View ?? 0;
            chapterReq.Index = chapterReq.Index ?? 0;
            var chapter = await _chapterService.UpdateAsync(id, chapterReq);
            //refresh view of story
            await _storyService.RefreshViewAsync(chapter.StoryId);
            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Cập nhật chapter thành công!", null));
        }

        #region Client

        [HttpGet("get-detail/{id}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] string id)
        {
            var chapter = await _chapterService.GetAsync(id);
            await _chapterService.IncreaseViewAsync(id,1);
            return Ok(new ApiResponse<ChapterRes>(StatusCodes.Status200OK, "Lấy thông tin chapter thành công!", chapter));
        }
        #endregion
    }
}
