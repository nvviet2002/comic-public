using Comic.Domain.Entities;
using Comic.Domain.RequestModels.PaginateModel;
using Comic.Domain.ResponseModels.ChapterModel;
using Comic.Domain.ResponseModels.PaginateModel;
using Comic.Domain.ResponseModels.StoryModel;

namespace Comic.WebClient.ApiClient.Client
{
    public interface ICategoryApiClient
    {
        Task<ICollection<Category>> GetActivedAllAsync();
    }
}
