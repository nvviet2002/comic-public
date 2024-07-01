using Comic.Application.Services;
using Comic.Domain.Entities;
using Comic.Domain.RequestModels.ChapterModel;
using Comic.Domain.RequestModels.DatatableModel;
using Comic.Domain.RequestModels.StoryModel;
using Comic.Domain.ResponseModels.ChapterModel;
using Comic.Domain.ResponseModels.DatatableModel;
using Comic.Domain.ResponseModels.StoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Application.IServices
{
    public interface IChapterService
    {
        Task<ICollection<Story>> GetListAsync();
        Task<DatatableRes<ChapterItemRes>> GetDatatableAsync(string storyId,DatatableReq datatableReq);
        Task CreateAsync(string storyId,ChapterReq chapterReq);
        Task DeleteAsync(string id);
        Task<ChapterRes> GetAsync(string id);
        Task<Chapter> UpdateAsync(string id, ChapterReq storyReq);

        Task IncreaseViewAsync(string id, int view);

        //Task<ChapterRes> GetDetailAsync(string id);

    }
}
