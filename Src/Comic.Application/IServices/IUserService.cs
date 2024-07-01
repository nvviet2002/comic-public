using Comic.Domain.Entities;
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
    public interface IUserService
    {
        Task<ICollection<User>> GetListAsync();
        Task<DatatableRes<UserItemRes>> GetDatatableAsync(DatatableReq datatableReq);
        Task CreateAsync(CreateUserReq userReq);
        Task DeleteAsync(string id);
        Task<UserRes> GetAsync(string id);
        Task UpdateAsync(string id, UpdateUserReq userReq);
    }
}
