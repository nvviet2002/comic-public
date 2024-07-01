using Comic.Domain.Entities;
using Comic.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Infrastructure.Data.Repositories
{
    public class CommentLv2Repository : BaseRepository<CommentLv2>, ICommentLv2Repository
    {
        public CommentLv2Repository(ComicDbContext comicDbContext) : base(comicDbContext)
        {
        }
    }
}
