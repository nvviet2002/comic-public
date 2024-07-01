using Comic.Application.IServices;
using Comic.Application.Services;
using Comic.Domain.Contrants;
using Comic.Domain.Entities;
using Comic.Domain.RequestModels.DatatableModel;
using Comic.Domain.RequestModels.PermissionModel;
using Comic.Domain.RequestModels.UserModel;
using Comic.Domain.ResponseModels.DatatableModel;
using Comic.Domain.ResponseModels.User;
using Comic.Domain.ResponseModels.UserModel;
using Comic.WebAPI.Common;
using Comic.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Comic.WebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("list-datatable")]
        [Authorization(PermissionNames.EditUser)]
        public async Task<IActionResult> ListDatatableAsync([FromBody] DatatableReq datatableReq)
        {
            var users = await _userService.GetDatatableAsync(datatableReq);

            return Ok(new ApiResponse<DatatableRes<UserItemRes>>(StatusCodes.Status200OK, "Lấy danh sách người dùng thành công!", users));
        }

        [HttpPost("create")]
        [Authorization(PermissionNames.EditUser)]
        public async Task<IActionResult> CreateAsync([FromForm] CreateUserReq userReq)
        {
            userReq.BirthDay = userReq.BirthDay ?? DateTime.UtcNow;
            userReq.PhoneNumber = userReq.PhoneNumber ?? string.Empty;
            userReq.Point = userReq.Point ?? 0.0m;
            userReq.IsActived = userReq.IsActived ?? true;
            userReq.RoleIds = userReq.RoleIds ?? new List<string>();

            await _userService.CreateAsync(userReq);
            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Thêm người dùng mới thành công!", null));
        }

        [HttpDelete("delete/{id}")]
        [Authorization(PermissionNames.EditUser)]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            await _userService.DeleteAsync(id);
            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Xóa người dùng thành công!", null));
        }

        [HttpGet("get/{id}")]
        [Authorization(PermissionNames.EditUser)]
        public async Task<IActionResult> GetAsync([FromRoute] string id)
        {
            var userRes = await _userService.GetAsync(id);
            return Ok(new ApiResponse<UserRes>(StatusCodes.Status200OK, "Lấy thông tin người dùng thành công!", userRes));
        }

        [HttpPut("update/{id}")]
        [Authorization(PermissionNames.EditUser)]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromForm] UpdateUserReq userReq)
        {
            await _userService.UpdateAsync(id, userReq);
            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Cập nhật người dùng thành công!", null));
        }

        
    }
}
