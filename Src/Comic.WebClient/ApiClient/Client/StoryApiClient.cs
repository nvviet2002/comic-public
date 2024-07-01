using Comic.Domain.RequestModels.PaginateModel;
using Comic.Domain.ResponseModels.PaginateModel;
using Comic.Domain.ResponseModels.StoryModel;
using Comic.WebClient.Common;
using System.Text.Json;
using Newtonsoft.Json;
using Comic.Domain.Entities;
using Comic.Domain.ResponseModels.ChapterModel;
using Comic.Domain.RequestModels.StoryModel;

namespace Comic.WebClient.ApiClient.Client
{
    public class StoryApiClient : IStoryApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public StoryApiClient(IHttpClientFactory httpClientFactory
            ,IHttpContextAccessor httpContextAccessor,IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<PaginateRes<StoryRes>> GetPagingAsync(PaginateReq paginateReq)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration.GetValue<string?>("ApiServer"));

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.GetAsync("/api/story/list-paging" +
                $"?PageSize={paginateReq.PageSize}&PageNumber={paginateReq.PageNumber}&SearchTerm={paginateReq.SearchTerm}");
            var body = await response.Content.ReadAsStringAsync();
            ApiResponse<PaginateRes<StoryRes>> deserializedData
                = JsonConvert.DeserializeObject<ApiResponse<PaginateRes<StoryRes>>>(body);
            return deserializedData.Data;
        }

        public async Task<PaginateRes<StoryRes>> GetRecommendAsync(PaginateReq paginateReq)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration.GetValue<string?>("ApiServer"));

            var response = await client.GetAsync("/api/story/list-recommend" +
                $"?PageSize={paginateReq.PageSize}&PageNumber={paginateReq.PageNumber}&SearchTerm={paginateReq.SearchTerm}");
            var body = await response.Content.ReadAsStringAsync();
            ApiResponse<PaginateRes<StoryRes>> deserializedData
                = JsonConvert.DeserializeObject<ApiResponse<PaginateRes<StoryRes>>>(body);
            return deserializedData.Data;
        }

        public async Task<StoryRes> GetDetailAsync(string id)
        {
            
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration.GetValue<string?>("ApiServer"));

            var response = await client.GetAsync($"/api/story/get-detail/{id}");
            var body = await response.Content.ReadAsStringAsync();
            ApiResponse<StoryRes> deserializedData
                = JsonConvert.DeserializeObject<ApiResponse<StoryRes>>(body);
            return deserializedData.Data;
        }

        public async Task<ChapterRes> GetDetailChapterAsync(string id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration.GetValue<string?>("ApiServer"));

            var response = await client.GetAsync($"/api/story/ten-truyen/chapter/get-detail/{id}");
            var body = await response.Content.ReadAsStringAsync();
            ApiResponse<ChapterRes> deserializedData
                = JsonConvert.DeserializeObject<ApiResponse<ChapterRes>>(body);
            return deserializedData.Data;
        }

        public async Task<PaginateRes<StoryRes>> SearchPagingAsync(StorySearchReq storySearchReq)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration.GetValue<string?>("ApiServer"));

            var response = await client.GetAsync("/api/story/search-paging" +
                $"?PaginateReq.PageSize={storySearchReq.PaginateReq.PageSize}" +
                $"&PaginateReq.PageNumber={storySearchReq.PaginateReq.PageNumber}" +
                $"&PaginateReq.SearchTerm={storySearchReq.PaginateReq.SearchTerm}" +
                $"&CategoryId={storySearchReq.CategoryId}" +
                $"&Status={storySearchReq.Status}" +
                $"&SortOption={storySearchReq.SortOption}");

            var body = await response.Content.ReadAsStringAsync();
            ApiResponse<PaginateRes<StoryRes>> deserializedData
                = JsonConvert.DeserializeObject<ApiResponse<PaginateRes<StoryRes>>>(body);
            return deserializedData.Data;
        }

        public async Task<ICollection<StoryRes>> GetActivedAllAsync()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration.GetValue<string?>("ApiServer"));

            var response = await client.GetAsync("/api/story/get-actived-all");

            var body = await response.Content.ReadAsStringAsync();
            ApiResponse<ICollection<StoryRes>> deserializedData
                = JsonConvert.DeserializeObject<ApiResponse<ICollection<StoryRes>>>(body);
            return deserializedData.Data;
        }

        



    }
}
