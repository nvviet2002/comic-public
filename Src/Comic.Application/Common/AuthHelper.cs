using Comic.Domain.Common;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Comic.Application.Common
{
    public class AuthHelper
    {
        public static string CreateAccessToken(string userId, string email, IList<string>? roles )
        {
            var jwtId = Guid.NewGuid().ToString();
            var authClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti,jwtId),
                new Claim("UserId", userId )

            };
            if (!roles.IsNullOrEmpty())
            {
                foreach (var role in roles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSetting.JWTSecretKey));
            var token = new JwtSecurityToken(
                    issuer: AppSetting.JWTValidIssuer,
                    audience: AppSetting.JWTValidAudience,
                    expires: DateTime.UtcNow.AddMinutes((double)AppSetting.JWTATExpireTime),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string CreateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        public static async Task<JwtSecurityToken?> DecodeJwtAsync(string jwt)
        {
            var secretKey = Encoding.UTF8.GetBytes(AppSetting.JWTSecretKey);
            var jwtTokenHander = new JwtSecurityTokenHandler();
            var tokenValidateParam = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = AppSetting.JWTValidAudience,
                ValidIssuer = AppSetting.JWTValidIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateLifetime = false,
            };
            var tokenVertication = await jwtTokenHander.ValidateTokenAsync(jwt, tokenValidateParam);
            var jwtSecurityToken = tokenVertication.SecurityToken as JwtSecurityToken;
            return jwtSecurityToken;
        }
    }
}
