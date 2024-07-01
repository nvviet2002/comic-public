using Comic.Application.Common;
using Comic.Application.IServices;
using Comic.Application.Services;
using Comic.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Comic.WebAPI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizationAttribute: Attribute, IAsyncAuthorizationFilter
    {
        private IAuthService _authService;

        private List<string> Permissions { get; set; } = new List<string>();
        public AuthorizationAttribute(params string[] permissions)
        {
            Permissions.AddRange(permissions);
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext filterContext)
        {

            if (filterContext != null)
            {
                _authService = filterContext.HttpContext.RequestServices.GetService<IAuthService>();

                var accessToken = filterContext.HttpContext.Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new AuthException("Không tìm thấy access token");
                }
                var jwtSecure = await AuthHelper.DecodeJwtAsync(accessToken);
                if (jwtSecure == null)
                {
                    throw new AuthException("Access token không đúng");
                }
                var userId = jwtSecure.Claims.FirstOrDefault(q => q.Type == "UserId").Value;

                var check = await _authService.CheckPermissionAsync(userId, Permissions);
                if (!check) throw new AuthException("Tài khoản không có quyền truy cập");
            }
        }
    }
}
