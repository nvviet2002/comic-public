using Comic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.Repositories
{
    public interface IStoryRepository: IBaseRepository<Story>
    {
        Task<ICollection<Story>> GetAllDatatableIncludeCategoriesAsync(Expression<Func<Story, bool>> filter
            , Expression<Func<Story, dynamic>> orderBy, string dirSort, int start, int length);
    }
}
