using Comic.Application.IServices;
using Comic.Domain.Common;
using Comic.Domain.Contrants;
using Comic.Domain.Entities;
using Comic.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Comic.WebAPI.Common;
using Comic.Domain.ResponseModels.PaginateModel;
using Comic.Domain.ResponseModels.DatatableModel;
using Comic.Domain.RequestModels.DatatableModel;
using Comic.Domain.RequestModels.PaginateModel;
using Comic.Domain.RequestModels.PermissionModel;

namespace Comic.WebAPI.Controllers
{
    [Route("api/permission")]
    [ApiController]
    public class PermissionController : Controller
    {
        private readonly IPermissionService _permissionService;
        private readonly ILogger<PermissionController> _logger;

        public PermissionController(IPermissionService permissionService, ILogger<PermissionController> logger)
        {
            _permissionService = permissionService;
            _logger = logger;
        }

        [HttpGet("list")]
        [Authorization(PermissionNames.EditPermission)]
        public async Task<IActionResult> ListAsync()
        {
            var permissions = await _permissionService.GetListAsync();

            return Ok(new ApiResponse<ICollection<Permission>>(StatusCodes.Status200OK, "Lấy danh sách quyền thành công!", permissions));
        }

        [HttpGet("list-paginate")]
        [Authorization(PermissionNames.EditPermission)]
        public async Task<IActionResult> ListPaginateAsync([FromQuery] PaginateReq paginateReq)
        {
            paginateReq.SearchTerm = paginateReq.SearchTerm ?? string.Empty;
            var permissions = await _permissionService.GetPaginatedListAsync(paginateReq);

            return Ok(new ApiResponse<PaginateRes<Permission>>(StatusCodes.Status200OK, "Lấy danh sách quyền thành công!", permissions));
        }

        [HttpPost("list-datatable")]
        [Authorization(PermissionNames.EditPermission)]
        public async Task<IActionResult> ListDatatableAsync([FromBody]DatatableReq datatableReq)
        {
            var permissions = await _permissionService.GetDatatableAsync(datatableReq);

            return Ok(new ApiResponse<DatatableRes<Permission>>(StatusCodes.Status200OK, "Lấy danh sách quyền thành công!", permissions));
        }

        [HttpPost("create")]
        [Authorization(PermissionNames.EditPermission)]
        public async Task<IActionResult> CreateAsync([FromBody] PermissionReq permissionRequest)
        {
            await _permissionService.CreateAsync(permissionRequest.Name);
            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Thêm quyền mới thành công!", null));
        }

        [HttpDelete("delete/{id}")]
        [Authorization(PermissionNames.EditPermission)]
        public async Task<IActionResult> DeleteAsync([FromRoute]string id)
        {
            await _permissionService.DeleteAsync(id);
            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Xóa quyền thành công!", null));
        }

        [HttpGet("get/{id}")]
        [Authorization(PermissionNames.EditPermission)]
        public async Task<IActionResult> GetAsync([FromRoute] string id)
        {
            var permission = await _permissionService.GetAsync(id);
            return Ok(new ApiResponse<Permission>(StatusCodes.Status200OK, "Lấy thông tin quyền thành công!", permission));
        }

        [HttpPut("update/{id}")]
        [Authorization(PermissionNames.EditPermission)]
        public async Task<IActionResult> UpdateAsync([FromRoute]string id ,[FromBody] PermissionReq permissionRequest )
        {
            await _permissionService.UpdateAsync(id, permissionRequest.Name);
            return Ok(new ApiResponse<dynamic>(StatusCodes.Status200OK, "Thêm quyền mới thành công!", null));
        }
    }
}
