using Comic.Domain.Entities;
using Comic.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Infrastructure.Data.Repositories
{
    public class CommentLv1Repository : BaseRepository<CommentLv1>, ICommentLv1Repository
    {
        public CommentLv1Repository(ComicDbContext comicDbContext) : base(comicDbContext)
        {
        }
    }
}
