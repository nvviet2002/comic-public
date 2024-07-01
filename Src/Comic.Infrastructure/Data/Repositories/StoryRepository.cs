using Comic.Domain.Entities;
using Comic.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Infrastructure.Data.Repositories
{
    public class StoryRepository : BaseRepository<Story>, IStoryRepository
    {
        public StoryRepository(ComicDbContext comicDbContext) : base(comicDbContext)
        {

        }
        public async Task<ICollection<Story>> GetAllDatatableIncludeCategoriesAsync(Expression<Func<Story, bool>> filter
            , Expression<Func<Story, dynamic>> orderBy, string dirSort, int start, int length)
        {
            var dbSet = _context.Stories.Include(q => q.StoryCategories).ThenInclude(q => q.Category).Where(filter);
            var raw = new List<Story>();
            if (dirSort.Equals("asc"))
            {
                raw = await dbSet.OrderBy(orderBy)
                    .Skip(start).Take(length).AsNoTracking().ToListAsync();
            }
            else
            {
                raw = await dbSet.OrderByDescending(orderBy)
                    .Skip(start).Take(length).AsNoTracking().ToListAsync();
            }
            return raw;
        }
    }
}
