using Comic.Domain.Entities;
using Comic.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Infrastructure.Data.Repositories
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(ComicDbContext comicDbContext) : base(comicDbContext)
        {

        }

        public async Task<ICollection<Permission>> GetByUserIdAsync(string userId)
        {
            var permission = await (from ur in _context.UserRoles
                              join r in _context.Roles on ur.RoleId equals r.Id
                              join rp in _context.RolePermissions on r.Id equals rp.RoleId
                              join p in _context.Permissions on rp.PermissionId equals p.Id
                              where ur.UserId == userId
                              select p).ToListAsync();
            return permission;
                              
        }
    }
}
