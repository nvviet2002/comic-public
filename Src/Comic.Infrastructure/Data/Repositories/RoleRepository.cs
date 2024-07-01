using Comic.Domain.Entities;
using Comic.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Infrastructure.Data.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(ComicDbContext comicDbContext) : base(comicDbContext)
        {

        }

        public async Task<IDictionary<string,int>> CountAllHasUserAsync()
        {
            var rawRoles =  await (from ur in _context.UserRoles
                            join r in _context.Roles on ur.RoleId equals r.Id
                            group ur by new { ur.RoleId } into g
                            select new { g.Key.RoleId, Count = g.Count() }).ToListAsync();
            var newDic = new Dictionary<string,int>();
            foreach (var raw in rawRoles)
            {
                newDic.Add(raw.RoleId, raw.Count);
            }
            return newDic;
        }
    }
}
