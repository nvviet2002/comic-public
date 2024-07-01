using Comic.Application.IServices;
using Comic.Domain.Common;
using Comic.Domain.Exceptions;
using Comic.Domain.RequestModels.AuthModel;
using Comic.Domain.ResponseModels.TokenModel;
using Comic.Domain.ResponseModels.User;
using Comic.WebAPI.Common;
using Comic.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Comic.WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginReq requestLogin)
        {
            var result = await _authService.LoginAsync(requestLogin.Email,requestLogin.Password);

            HttpContext.Response.Cookies.Append("AccessToken", result.AccessToken,
                new CookieOptions()
                {
                    Expires = DateTime.UtcNow.AddDays(AppSetting.JWTRTExpireTime * 2),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None,
                });
            HttpContext.Response.Cookies.Append("RefreshToken", result.RefreshToken,
                new CookieOptions()
                {
                    Expires = DateTime.UtcNow.AddDays(AppSetting.JWTRTExpireTime * 2),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None,
                });

            return Ok(new ApiResponse<TokenRes>(StatusCodes.Status200OK, "Đăng nhập thành công!", result));
        }

        [HttpGet("renew-access-token")]
        public async Task<IActionResult> RenewAccessTokenAsync()
        {
            var accessToken = HttpContext.Request.Cookies["AccessToken"];
            var refreshToken = HttpContext.Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                throw new AuthException("AccessToken or RefreshToken is missing");
            }

            var newJwt = await _authService.RenewAccessTokenAsync(accessToken,refreshToken);

            HttpContext.Response.Cookies.Append("AccessToken", newJwt,
                new CookieOptions()
                {
                    Expires = DateTime.UtcNow.AddDays(AppSetting.JWTRTExpireTime * 2),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None,
                });
            var apiResponse = new ApiResponse<dynamic>()
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Renew access token successfully",
                Data = new { AccessToken = newJwt },
            };

            return Ok(apiResponse);
        }
        [HttpGet("get-infomation")]
        public async Task<IActionResult> GetInfomationAsync()
        {
            var accessToken = HttpContext.Request.Cookies["AccessToken"];
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new AuthException("AccessToken is missing");
            }
            var userInfo = await _authService.GetInfomationAsync(accessToken);
            var apiResponse = new ApiResponse<UserInfoRes>()
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Get infomation successfully",
                Data = userInfo,
            };
            return Ok(apiResponse);
        }

        [HttpGet("test")]
        [Authorization("Edit role", "Edit user")]
        public async Task<IActionResult> TestAsync()
        {
            
            var apiResponse = new ApiResponse<UserInfoRes>()
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Test successfully",
                Data = null,
            };
            return Ok(apiResponse);
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> SignUpAsync(SignUpParam model)
        //{
        //    var result = await accountRepository.SignUpAsync(model);
        //    if (!result.Succeeded)
        //    {
        //        return Unauthorized();
        //    }
        //    return Ok(new ApiResponse(
        //        StatusCodes.Status200OK
        //        , "Tạo tài khoản thành công!"
        //        , null));
        //}


        //[HttpGet("get-information")]
        //[Authorize]
        //public async Task<IActionResult> GetInformationAsync()
        //{
        //    var accessToken = HttpContext.Request.Cookies["AccessToken"];
        //    if (accessToken.IsNullOrEmpty())
        //    {
        //        return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized
        //            , "You are not logged in!"
        //            , null));
        //    }

        //    var result = await accountRepository.GetInformationAsync(accessToken);
        //    var infor = new
        //    {
        //        result.Item1.Id,
        //        result.Item1.Name,
        //        result.Item1.Email,
        //        result.Item1.Avatar,
        //        result.Item1.PhoneNumber,
        //        Role = result.Item2.FirstOrDefault(),
        //    };
        //    return Ok(new ApiResponse(StatusCodes.Status200OK
        //        , accessToken
        //        , infor));
        //}
    }
}
