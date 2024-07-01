using Comic.Domain.Entities;
using Comic.Domain.RequestModels.RoleModel;
using Comic.Domain.ResponseModels.RoleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Application.IServices
{
    public interface IRoleService
    {
        Task<ICollection<RoleItemRes>> GetListAsync();
        Task CreateAsync(string name, ICollection<string> permissions);
        Task DeleteAsync(string id);
        Task<GetRoleRes> GetAsync(string id);
        Task UpdateAsync(string id, RoleReq roleReq);
    }
}
