using Comic.Domain.Entities;
using Comic.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Infrastructure.Data.Repositories
{
    public class FollowRepository : BaseRepository<Follow>, IFollowRepository
    {
        public FollowRepository(ComicDbContext comicDbContext) : base(comicDbContext)
        {
        }
    }
}
