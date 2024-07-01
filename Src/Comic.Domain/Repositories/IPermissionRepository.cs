using Comic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.Repositories
{
    public interface IPermissionRepository: IBaseRepository<Permission>
    {
        Task<ICollection<Permission>> GetByUserIdAsync(string userId);
    }
}
