using Comic.Domain.Entities;
using Comic.Domain.RequestModels.DatatableModel;
using Comic.Domain.RequestModels.PaginateModel;
using Comic.Domain.ResponseModels.DatatableModel;
using Comic.Domain.ResponseModels.PaginateModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Application.IServices
{
    public interface IPermissionService
    {
        Task<ICollection<Permission>> GetListAsync();
        Task<PaginateRes<Permission>> GetPaginatedListAsync(PaginateReq paginateReq);
        Task<DatatableRes<Permission>> GetDatatableAsync(DatatableReq datatableReq);
        Task CreateAsync(string name);
        Task DeleteAsync(string id);
        Task<Permission> GetAsync(string id);
        Task UpdateAsync(string id, string name);


    }
}
