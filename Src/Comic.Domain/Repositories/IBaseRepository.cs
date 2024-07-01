using Comic.Domain.RequestModels.PaginateModel;
using Comic.Domain.ResponseModels.PaginateModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Comic.Domain.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> GetByQuery();
        T? GetById(string id);

        Task<T?> GetByIdAsync(string id);

        ICollection<T> GetAll(Expression<Func<T, bool>> expression);
        ICollection<T> GetAll();
        Task<ICollection<T>> GetAllAsync();

        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> expression);
        Task<ICollection<T>> GetAllSortAsync(Expression<Func<T, dynamic>> orderBy, bool ascendingSort);
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> filter
            ,Expression<Func<T, dynamic>> orderBy, bool ascendingSort);
        
        Task<PaginateRes<T>> GetAllPaginateAsync(Expression<Func<T, bool>> filter
            , Expression<Func<T, dynamic>> orderBy, bool ascendingSort, PaginateReq paginateReq);
        Task<ICollection<T>> GetAllDatatableAsync(Expression<Func<T, bool>> filter
            , Expression<Func<T, dynamic>> orderBy, string dirSort, int start, int length);

        T Get(Expression<Func<T, bool>> expression);

        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        

        void Add(T entity);

        Task AddAsync(T entity);

        void AddRange(IEnumerable<T> entites);

        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity);

        void UpdateRange(IEnumerable<T> entities);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);

        int SaveChanges();

        Task<int> SaveChangesAsync();

    }
}
