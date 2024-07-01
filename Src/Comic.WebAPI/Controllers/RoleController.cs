using Comic.Application.IServices;
using Comic.Application.Services;
using Comic.Domain.Contrants;
using Comic.Domain.Entities;
using Comic.Domain.RequestModels.RoleModel;
using Comic.Domain.ResponseModels.RoleModel;
using Comic.WebAPI.Common;
using Comic.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Comic.WebAPI.Controllers
{
    [Route("api/role")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        [HttpGet("list")]
        [Authorization(PermissionNames.EditRole)]
        public async Task<IActionResult> ListAsync()
        {
            var roles = await _roleService.GetListAsync();

            return Ok(new ApiResponse<ICollection<RoleItemRes>>(StatusCodes.Status200OK, "Lấy danh sách vai trò thành công!", roles));
        }

        [HttpPost("create")]
        [Authorization(PermissionNames.EditRole)]
        public async Task<IActionResult> CreateAsync([FromBody] RoleReq roleReq)
        {
            await _roleService.CreateAsync(roleReq.Name,roleReq.PermissionIds);
            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Thêm vai trò mới thành công!", null));
        }

        [HttpDelete("delete/{id}")]
        [Authorization(PermissionNames.EditRole)]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            await _roleService.DeleteAsync(id);
            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Xóa vai trò thành công!", null));
        }

        [HttpGet("get/{id}")]
        [Authorization(PermissionNames.EditRole)]
        public async Task<IActionResult> GetAsync([FromRoute] string id)
        {
            var role = await _roleService.GetAsync(id);
            return Ok(new ApiResponse<GetRoleRes>(StatusCodes.Status200OK, "Lấy thông tin vai trò thành công!", role));
        }

        [HttpPut("update/{id}")]
        [Authorization(PermissionNames.EditPermission)]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] RoleReq roleReq)
        {
            await _roleService.UpdateAsync(id, roleReq);
            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Sửa vai trò thành công!", null));
        }

    }
}
