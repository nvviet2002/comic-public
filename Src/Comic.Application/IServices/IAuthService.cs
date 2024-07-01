using Comic.Domain.ResponseModels.TokenModel;
using Comic.Domain.ResponseModels.User;

namespace Comic.Application.IServices
{
    public interface IAuthService
    {
        Task<string> RenewAccessTokenAsync(string accessToken, string refreshToken);
        Task<TokenRes> LoginAsync(string email, string password);
        Task<string> RegisterAsync(string email, string password);
        Task<UserInfoRes> GetInfomationAsync(string accessToken);
        Task<bool> CheckPermissionAsync(string userId , IEnumerable<string> permissions);
    }
}
