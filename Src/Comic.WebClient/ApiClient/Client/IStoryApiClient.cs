using Comic.Domain.RequestModels.PaginateModel;
using Comic.Domain.RequestModels.StoryModel;
using Comic.Domain.ResponseModels.ChapterModel;
using Comic.Domain.ResponseModels.PaginateModel;
using Comic.Domain.ResponseModels.StoryModel;

namespace Comic.WebClient.ApiClient.Client
{
    public interface IStoryApiClient
    {
        Task<PaginateRes<StoryRes>> GetPagingAsync(PaginateReq paginateReq);
        Task<PaginateRes<StoryRes>> GetRecommendAsync(PaginateReq paginateReq);
        Task<StoryRes> GetDetailAsync(string id);
        Task<ChapterRes> GetDetailChapterAsync(string id);

        Task<PaginateRes<StoryRes>> SearchPagingAsync(StorySearchReq searchReq);
        Task<ICollection<StoryRes>> GetActivedAllAsync();

    }
}
