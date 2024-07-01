using Comic.WebClient.Common;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Comic.WebClient.ApiClient.Client
{
    public class BaseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected BaseApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        protected async Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration.GetValue<string?>("ApiServer"));
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();
            ApiResponse<TResponse> myDeserializedObjList = JsonSerializer.Deserialize<ApiResponse<TResponse>>(body);
            return myDeserializedObjList;
        }

        //public async Task<List<T>> GetListAsync<T>(string url, bool requiredLogin = false)
        //{
        //    var sessions = _httpContextAccessor
        //       .HttpContext
        //       .Session
        //       .GetString(SystemConstants.AppSettings.Token);
        //    var client = _httpClientFactory.CreateClient();
        //    client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

        //    var response = await client.GetAsync(url);
        //    var body = await response.Content.ReadAsStringAsync();
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var data = (List<T>)JsonConvert.DeserializeObject(body, typeof(List<T>));
        //        return data;
        //    }
        //    throw new Exception(body);
        //}

        //public async Task<bool> Delete(string url)
        //{
        //    var sessions = _httpContextAccessor
        //       .HttpContext
        //       .Session
        //       .GetString(SystemConstants.AppSettings.Token);
        //    var client = _httpClientFactory.CreateClient();
        //    client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

        //    var response = await client.DeleteAsync(url);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
    }
}
