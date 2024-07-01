using Comic.Domain.Entities;
using Comic.Domain.RequestModels.CategoryModel;
using Comic.Domain.RequestModels.DatatableModel;
using Comic.Domain.RequestModels.UserModel;
using Comic.Domain.ResponseModels.DatatableModel;
using Comic.Domain.ResponseModels.User;
using Comic.Domain.ResponseModels.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Application.IServices
{
    public interface ICategoryService
    {
        Task<ICollection<Category>> GetListAsync();
        Task<DatatableRes<Category>> GetDatatableAsync(DatatableReq datatableReq);
        Task CreateAsync(CategoryReq categoryReq);
        Task DeleteAsync(string id);
        Task<Category> GetAsync(string id);
        Task UpdateAsync(string id, CategoryReq categoryReq);

        Task<ICollection<Category>> GetActivedAllAsync();
    }
}
