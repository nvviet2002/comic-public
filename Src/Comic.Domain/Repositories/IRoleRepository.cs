using Comic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.Repositories
{
    public interface IRoleRepository: IBaseRepository<Role>
    {
        Task<IDictionary<string, int>> CountAllHasUserAsync();
    }
}
