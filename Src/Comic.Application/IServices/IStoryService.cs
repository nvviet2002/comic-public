using Comic.Domain.Entities;
using Comic.Domain.Exceptions;
using Comic.Domain.RequestModels.CategoryModel;
using Comic.Domain.RequestModels.DatatableModel;
using Comic.Domain.RequestModels.PaginateModel;
using Comic.Domain.RequestModels.StoryModel;
using Comic.Domain.ResponseModels.DatatableModel;
using Comic.Domain.ResponseModels.PaginateModel;
using Comic.Domain.ResponseModels.StoryModel;
using Comic.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Application.IServices
{
    public interface IStoryService
    {
        Task<ICollection<Story>> GetListAsync();
        Task<DatatableRes<StoryItemRes>> GetDatatableAsync(DatatableReq datatableReq);
        Task CreateAsync(StoryReq categoryReq);
        Task DeleteAsync(string id);
        Task<StoryRes> GetAsync(string id);
        Task UpdateAsync(string id, StoryReq storyReq);
        Task RefreshViewAsync(string id);
        
        Task<PaginateRes<StoryRes>> GetListPagingAsync(PaginateReq paginateReq, Expression<Func<Story
            , bool>> filter, Expression<Func<Story, dynamic>> orderBy, bool ascendingSort);
        Task<PaginateRes<StoryRes>> SearchPagingAsync(PaginateReq paginateReq
            , Expression<Func<Story, bool>> filter, Expression<Func<Story, dynamic>> orderBy
            , bool ascendingSort);

        Task<StoryRes> GetDetailAsync(string id);
        Task<ICollection<StoryRes>> GetAllAsync(Expression<Func<Story, bool>> filter
           , Expression<Func<Story, dynamic>> orderBy, bool ascendingSort);
    }
}
