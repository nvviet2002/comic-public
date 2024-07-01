using Comic.Application.IServices;
using Comic.Application.Services;
using Comic.Domain.Contrants;
using Comic.Domain.Entities;
using Comic.Domain.Enums;
using Comic.Domain.RequestModels.CategoryModel;
using Comic.Domain.RequestModels.DatatableModel;
using Comic.Domain.RequestModels.UserModel;
using Comic.Domain.ResponseModels.DatatableModel;
using Comic.Domain.ResponseModels.RoleModel;
using Comic.Domain.ResponseModels.User;
using Comic.Domain.ResponseModels.UserModel;
using Comic.WebAPI.Common;
using Comic.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Comic.WebAPI.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpPost("list-datatable")]
        [Authorization(PermissionNames.EditCategory)]
        public async Task<IActionResult> ListDatatableAsync([FromBody] DatatableReq datatableReq)
        {
            var categories = await _categoryService.GetDatatableAsync(datatableReq);

            return Ok(new ApiResponse<DatatableRes<Category>>(StatusCodes.Status200OK, "Lấy danh sách danh mục thành công!", categories));
        }

        [HttpGet("list")]
        [Authorization(PermissionNames.EditCategory)]
        public async Task<IActionResult> ListAsync()
        {
            var categories = await _categoryService.GetListAsync();
            return Ok(new ApiResponse<ICollection<Category>>(StatusCodes.Status200OK, "Lấy danh sách danh mục thành công!", categories));
        }

        [HttpPost("create")]
        [Authorization(PermissionNames.EditCategory)]
        public async Task<IActionResult> CreateAsync([FromForm] CategoryReq categoryReq)
        {
            categoryReq.Name = categoryReq.Name ?? string.Empty;
            categoryReq.Description = categoryReq.Description ?? string.Empty;
            categoryReq.MetaDescription = categoryReq.MetaDescription ?? string.Empty;
            categoryReq.MetaKeyword = categoryReq.MetaKeyword ?? string.Empty;
            categoryReq.Type = categoryReq.Type ?? CategoryType.Category;
            categoryReq.IsActived = categoryReq.IsActived ?? true;
            

            await _categoryService.CreateAsync(categoryReq);
            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Thêm danh mục mới thành công!", null));
        }

        [HttpDelete("delete/{id}")]
        [Authorization(PermissionNames.EditCategory)]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            await _categoryService.DeleteAsync(id);
            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Xóa danh mục thành công!", null));
        }

        [HttpGet("get/{id}")]
        [Authorization(PermissionNames.EditCategory)]
        public async Task<IActionResult> GetAsync([FromRoute] string id)
        {
            var category = await _categoryService.GetAsync(id);
            return Ok(new ApiResponse<Category>(StatusCodes.Status200OK, "Lấy thông tin người dùng thành công!", category));
        }

        [HttpPut("update/{id}")]
        [Authorization(PermissionNames.EditCategory)]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromForm] CategoryReq categoryReq)
        {
            categoryReq.Name = categoryReq.Name ?? string.Empty;
            categoryReq.Description = categoryReq.Description ?? string.Empty;
            categoryReq.MetaDescription = categoryReq.MetaDescription ?? string.Empty;
            categoryReq.MetaKeyword = categoryReq.MetaKeyword ?? string.Empty;
            categoryReq.Type = categoryReq.Type ?? CategoryType.Category;
            categoryReq.IsActived = categoryReq.IsActived ?? true;
            await _categoryService.UpdateAsync(id, categoryReq);
            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Cập nhật danh mục thành công!", null));
        }
        #region Client
        [HttpGet("get-actived-all")]
        public async Task<IActionResult> GetActivedAllAsync()
        {
            var categories = await _categoryService.GetActivedAllAsync();
            return Ok(new ApiResponse<ICollection<Category>>(StatusCodes.Status200OK, "Lấy danh sách danh mục thành công!", categories));
        }
        #endregion

    }
}
