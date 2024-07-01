using Comic.Domain.Entities;
using Comic.Domain.ResponseModels.PaginateModel;
using Comic.WebClient.Common;
using Newtonsoft.Json;

namespace Comic.WebClient.ApiClient.Client
{
    public class CategoryApiClient : ICategoryApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CategoryApiClient(IHttpClientFactory httpClientFactory
            , IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ICollection<Category>> GetActivedAllAsync()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration.GetValue<string?>("ApiServer"));

            var response = await client.GetAsync("/api/category/get-actived-all");
            var body = await response.Content.ReadAsStringAsync();
            ApiResponse<ICollection<Category>> deserializedData
                = JsonConvert.DeserializeObject<ApiResponse<ICollection<Category>>>(body);
            return deserializedData.Data;
        }
    }
}
