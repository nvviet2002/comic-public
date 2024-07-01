using Comic.Application.IServices;
using Comic.Application.Services;
using Comic.Domain.Contrants;
using Comic.Domain.Entities;
using Comic.Domain.Enums;
using Comic.Domain.RequestModels.CategoryModel;
using Comic.Domain.RequestModels.DatatableModel;
using Comic.Domain.RequestModels.PaginateModel;
using Comic.Domain.RequestModels.StoryModel;
using Comic.Domain.RequestModels.UserModel;
using Comic.Domain.ResponseModels.DatatableModel;
using Comic.Domain.ResponseModels.PaginateModel;
using Comic.Domain.ResponseModels.RoleModel;
using Comic.Domain.ResponseModels.StoryModel;
using Comic.Domain.ResponseModels.UserModel;
using Comic.WebAPI.Common;
using Comic.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Comic.WebAPI.Controllers
{
    [Route("api/story")]
    [ApiController]
    public class StoryController : Controller
    {
        private readonly IStoryService _storyService;
        private readonly ILogger<StoryController> _logger;

        public StoryController(IStoryService storyService, ILogger<StoryController> logger)
        {
            _storyService = storyService;
            _logger = logger;
        }
        #region Admin

        [HttpPost("list-datatable")]
        [Authorization(PermissionNames.EditStory)]
        public async Task<IActionResult> ListDatatableAsync([FromBody] DatatableReq datatableReq)
        {
            var stories = await _storyService.GetDatatableAsync(datatableReq);

            return Ok(new ApiResponse<DatatableRes<StoryItemRes>>(StatusCodes.Status200OK, "Lấy danh sách truyện thành công!", stories));
        }

        [HttpPost("create")]
        [Authorization(PermissionNames.EditStory)]
        public async Task<IActionResult> CreateAsync([FromForm] StoryReq storyReq)
        {
            storyReq.Name = storyReq.Name ?? string.Empty;
            storyReq.OtherName = storyReq.OtherName ?? string.Empty;
            storyReq.Description = storyReq.Description ?? string.Empty;
            storyReq.MetaDescription = storyReq.MetaDescription ?? string.Empty;
            storyReq.MetaKeyword = storyReq.MetaKeyword ?? string.Empty;
            storyReq.Status = storyReq.Status ?? StoryStatus.New;
            storyReq.HotFlag = storyReq.HotFlag ?? true;
            storyReq.IsActived = storyReq.IsActived ?? true;
            storyReq.CategoryIds = storyReq.CategoryIds ?? new List<string>();
            await _storyService.CreateAsync(storyReq);
            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Thêm tuyện mới thành công!", null));
        }

        [HttpDelete("delete/{id}")]
        [Authorization(PermissionNames.EditStory)]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            await _storyService.DeleteAsync(id);
            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Xóa truyện thành công!", null));
        }

        [HttpGet("list")]
        [Authorization(PermissionNames.EditStory)]
        public async Task<IActionResult> ListAsync()
        {
            var stories = await _storyService.GetListAsync();

            return Ok(new ApiResponse<ICollection<Story>>(StatusCodes.Status200OK, "Lấy danh sách truyện thành công!", stories));
        }

        [HttpGet("get/{id}")]
        [Authorization(PermissionNames.EditStory)]
        public async Task<IActionResult> GetAsync([FromRoute] string id)
        {
            var storyRes = await _storyService.GetAsync(id);
            return Ok(new ApiResponse<StoryRes>(StatusCodes.Status200OK, "Lấy thông tin truyện thành công!", storyRes));
        }

        [HttpPut("update/{id}")]
        [Authorization(PermissionNames.EditStory)]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromForm] StoryReq storyReq)
        {
            storyReq.Name = storyReq.Name ?? string.Empty;
            storyReq.OtherName = storyReq.OtherName ?? string.Empty;
            storyReq.MetaDescription = storyReq.MetaDescription ?? string.Empty;
            storyReq.MetaKeyword = storyReq.MetaKeyword ?? string.Empty;
            storyReq.Description = storyReq.Description ?? string.Empty;
            storyReq.Status = storyReq.Status ?? StoryStatus.Updating;
            storyReq.CategoryIds = storyReq.CategoryIds ?? new List<string>();
            storyReq.HotFlag = storyReq.HotFlag ?? false;
            storyReq.IsActived = storyReq.IsActived ?? true;            
            await _storyService.UpdateAsync(id, storyReq);
            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Cập nhật truyện thành công!", null));
        }

        #endregion
        #region Client
        [HttpGet("list-recommend")]
        public async Task<IActionResult> ListRecommendAsync([FromQuery] PaginateReq paginateReq)
        {
            var stories = await _storyService.GetListPagingAsync(paginateReq
                ,q=>q.IsActived == true && q.HotFlag == true,q=>q.View,false);

            return Ok(new ApiResponse<PaginateRes<StoryRes>>(StatusCodes.Status200OK, "Lấy danh sách truyện đề cử thành công!", stories));
        }
        [HttpGet("list-paging")]
        public async Task<IActionResult> ListPagingAsync([FromQuery] PaginateReq paginateReq)
        {
            var stories = await _storyService.GetListPagingAsync(paginateReq
                , q => q.IsActived == true, q => q.Chapters.FirstOrDefault().UpdatedAt, false);

            return Ok(new ApiResponse<PaginateRes<StoryRes>>(StatusCodes.Status200OK, "Lấy danh sách truyện thành công!", stories));
        }
        [HttpGet("get-detail/{id}")]
        public async Task<IActionResult> GetDetailAsync(string id)
        {
            var story = await _storyService.GetDetailAsync(id);

            return Ok(new ApiResponse<StoryRes>(StatusCodes.Status200OK, "Lấy danh sách truyện đề cử thành công!", story));
        }

        [HttpGet("search-paging")]
        public async Task<IActionResult> SearchPagingAsync([FromQuery] StorySearchReq storySearchReq)
        {
            Expression<Func<Story, bool>> filter;
            if(!string.IsNullOrEmpty(storySearchReq.CategoryId))
            {
                filter = q=> q.IsActived == true && (q.StoryCategories
                .FirstOrDefault(sc => sc.CategoryId == storySearchReq.CategoryId) != null);
            }else if (!string.IsNullOrEmpty(storySearchReq.PaginateReq.SearchTerm))
            {
                filter = q => q.IsActived == true && 
                (q.Name.Contains(storySearchReq.PaginateReq.SearchTerm));
            }
            else if(storySearchReq.Status != null)
            {
                filter = q => q.IsActived == true && (q.Status == storySearchReq.Status);
            }
            else
            {
                filter = q => q.IsActived == true;
            }
            //sort option
            Expression<Func<Story, dynamic>> sorter = q => q.UpdatedAt;
            if (storySearchReq.SortOption == SortOption.UpdatedAt)
            {
                sorter = q => q.Chapters.FirstOrDefault().UpdatedAt;
            }
            else if(storySearchReq.SortOption == SortOption.TopAll)
            {
                sorter = q => q.View;
            }
            else if (storySearchReq.SortOption == SortOption.TopDay)
            {
                sorter = q => q.View; // not done
            }
            else if (storySearchReq.SortOption == SortOption.TopMonth)
            {
                sorter = q => q.View; // not done
            }
            else if (storySearchReq.SortOption == SortOption.TopWeek)
            {
                sorter = q => q.View; // not done
            }
            else if (storySearchReq.SortOption == SortOption.Follow)
            {
                sorter = q => q.Follow; 
            }
            else if (storySearchReq.SortOption == SortOption.Comment)
            {
                sorter = q => q.Comment;
            }
            else if (storySearchReq.SortOption == SortOption.Chapter)
            {
                sorter = q => q.Chapters.Where(q=>q.IsActived).Count();
            }
            else
            {
                sorter = q => q.UpdatedAt;
            }

            var stories = await _storyService.SearchPagingAsync(storySearchReq.PaginateReq
                , filter, sorter, false);

            return Ok(new ApiResponse<PaginateRes<StoryRes>>(StatusCodes.Status200OK, "Lấy danh sách truyện thành công!", stories));
        }

        [HttpGet("get-actived-all")]
        public async Task<IActionResult> GetActivedAllAsync()
        {
            var stories = await _storyService.GetAllAsync(q=>q.IsActived, q=>q.CreatedAt,false);

            return Ok(new ApiResponse<ICollection<StoryRes>>(StatusCodes.Status200OK, "Lấy danh sách truyện thành công!", stories));
        }
        #endregion

    }
}
