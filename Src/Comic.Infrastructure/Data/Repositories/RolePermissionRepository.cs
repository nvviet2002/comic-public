using Comic.Domain.Entities;
using Comic.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Comic.Infrastructure.Data.Repositories
{
    public class RolePermissionRepository : BaseRepository<RolePermission>, IRolePermissionRepository
    {
        private readonly ComicDbContext _comicDbContext;

        public RolePermissionRepository(ComicDbContext comicDbContext) : base(comicDbContext)
        {
            _comicDbContext = comicDbContext;
        }
        public async Task<ICollection<RolePermission>> GetByRoleIdAsync(string roleId)
        {
            return await _comicDbContext.RolePermissions.Where(q => q.RoleId == roleId).ToListAsync();
        }
    }
}
