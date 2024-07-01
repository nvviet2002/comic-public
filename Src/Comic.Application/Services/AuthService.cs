using Comic.Application.Common;
using Comic.Application.IServices;
using Comic.Domain.Common;
using Comic.Domain.Entities;
using Comic.Domain.Exceptions;
using Comic.Domain.ResponseModels.TokenModel;
using Comic.Domain.ResponseModels.User;
using Comic.Domain.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public AuthService(UserManager<User> userManager, IUnitOfWork unitOfWork, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        public async Task<bool> CheckPermissionAsync(string userId, IEnumerable<string> permissions)
        {
            //get permission list
            var permissionList = await _unitOfWork.PermissionRepository.GetByUserIdAsync(userId);

            // check permission
            foreach(var perName in permissions)
            {
                if(permissionList.FirstOrDefault(q => q.Name == perName) == null)
                {
                    return false;
                }
            }
            return true;

        }

        public async Task<UserInfoRes> GetInfomationAsync(string accessToken)
        {
            var jwtSecure = await AuthHelper.DecodeJwtAsync(accessToken);
            if(jwtSecure == null)
            {
                throw new AuthException("Access token invalid");
            }
            var userId = jwtSecure.Claims.FirstOrDefault(q => q.Type == "UserId").Value.ToString();
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                throw new NotFoundException("User not found");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userInfoRes = new UserInfoRes()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Avatar = user.Avatar,
                Roles = roles,
            };
            return userInfoRes;
        }

        public async Task<TokenRes> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException("Not found user");
            }
            var isCheckedPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!isCheckedPassword)
            {
                throw new AuthException("Email or password is wrong");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = AuthHelper.CreateAccessToken(user.Id, user.Email, roles);
            var refreshToken = AuthHelper.CreateRefreshToken();
            var decodeAccessToken = await AuthHelper.DecodeJwtAsync(accessToken);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                //revoked refresh token
                var storedRefreshTokens = await _unitOfWork.RefreshTokenRepository.GetAllAsync(q => q.UserId == user.Id);
                if(storedRefreshTokens != null)
                {
                    foreach (var storedRefreshToken in storedRefreshTokens)
                    {
                        storedRefreshToken.IsRevoked = true;
                        _unitOfWork.RefreshTokenRepository.Update(storedRefreshToken);
                    }
                }
                var newRefreshToken = new RefreshToken()
                {
                    UserId = user.Id,
                    JwtId = decodeAccessToken.Id,
                    Token = refreshToken,
                    IsRevoked = false,
                    ExpireAt = DateTime.UtcNow.AddDays(AppSetting.JWTRTExpireTime),
                };
                await _unitOfWork.RefreshTokenRepository.AddAsync(newRefreshToken);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                throw new AppException(ex.Message, ex);
            }

            return new TokenRes { AccessToken = accessToken, RefreshToken = refreshToken};
        }

        public async Task<string> RegisterAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<string> RenewAccessTokenAsync(string accessToken, string refreshToken)
        {
            var jwtSecurityToken = await AuthHelper.DecodeJwtAsync(accessToken);
            // check algorithm
            var result = jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256Signature,
                StringComparison.InvariantCultureIgnoreCase);
            if (!result)
            {
                throw new AuthException("Access token không đúng");
            }
            var jwtId = jwtSecurityToken.Claims.FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Jti).Value;
            var userId = jwtSecurityToken.Claims.FirstOrDefault(q => q.Type == "UserId").Value;
            var expireTime = long.Parse(jwtSecurityToken.Claims.FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Exp).Value);
            var email = jwtSecurityToken.Claims.FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email).Value;


            // check expire token
            if (DateTime.UtcNow.ToUniversalTime() <= DataHelper.UnixTimeToDateTime(expireTime))
            {
                return accessToken;
            }

            //check exist refresh token
            var storedToken = await _unitOfWork.RefreshTokenRepository
                .GetAsync(q => q.Token == refreshToken && q.UserId == userId);
            if (storedToken == null)
            {
                throw new AuthException("Refresh token không tồn tại");
            }

            //check refresh token is revoked ?
            if (storedToken.IsRevoked)
            {
                throw new AuthException("Access token đã bị thu hồi!");
            }

            //check refresh token is expired
            if (storedToken.ExpireAt < DateTime.UtcNow.ToUniversalTime())
            {
                throw new AuthException("Refresh token đã hết hạn!");
            }

            //check id of access token match with jwtId in refresh token
            if (string.IsNullOrEmpty(jwtId) || storedToken.JwtId != jwtId)
            {
                throw new AuthException("Access token không đúng");
            }

            try
            {
                //create new access token and refresh token
                await _unitOfWork.BeginTransactionAsync();
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                var roles = await _userManager.GetRolesAsync(user);
                var newJwt = AuthHelper.CreateAccessToken(user.Id, user.Email, roles);
                storedToken.JwtId = newJwt;
                _unitOfWork.RefreshTokenRepository.Update(storedToken);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();
                return newJwt;
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                throw new AppException(ex.Message,ex);
            }
            
        }
    }
}
