using Comic.Domain.Entities;
using Comic.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Infrastructure.Data.Repositories
{
    public class ChapterRepository : BaseRepository<Chapter>, IChapterRepository
    {
        public ChapterRepository(ComicDbContext comicDbContext) : base(comicDbContext)
        {
        }
    }
}
