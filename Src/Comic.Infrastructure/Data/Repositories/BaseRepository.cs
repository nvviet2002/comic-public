using Comic.Domain.Repositories;
using Comic.Domain.RequestModels.PaginateModel;
using Comic.Domain.ResponseModels.PaginateModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;

namespace Comic.Infrastructure.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ComicDbContext _context;
        public BaseRepository(ComicDbContext comicDbContext)
        {
            _context = comicDbContext;
        }

        public IQueryable<T> GetByQuery()
        {
            return _context.Set<T>();
        }

        public T? Get(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().FirstOrDefault(expression);
        }

        public ICollection<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public ICollection<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).ToList();
        }

        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> filter
            , Expression<Func<T, dynamic>> orderBy, bool ascendingSort)
        {
            var raw = new List<T>();
            if (ascendingSort)
            {
                raw = await _context.Set<T>().Where(filter).OrderBy(orderBy).ToListAsync();
            }
            else
            {
                raw = await _context.Set<T>().Where(filter).OrderByDescending(orderBy).ToListAsync();
            }
            return raw;
        }

        public async Task<ICollection<T>> GetAllSortAsync(Expression<Func<T, dynamic>> orderBy, bool ascendingSort)
        {
            var raw = new List<T>();
            if (ascendingSort)
            {
                raw = await _context.Set<T>().OrderBy(orderBy).ToListAsync();
            }
            else
            {
                raw = await _context.Set<T>().OrderByDescending(orderBy).ToListAsync();
            }
            return raw;
        }

        public async Task<PaginateRes<T>> GetAllPaginateAsync(Expression<Func<T, bool>> filter
            ,Expression<Func<T, dynamic>> orderBy,bool ascendingSort, PaginateReq paginateReq)
        {
            var dbSet = _context.Set<T>().Where(filter);
            var totalCount = await dbSet.CountAsync();
            var raw = new List<T>();
            if (ascendingSort)
            {
                raw = await dbSet.OrderBy(orderBy)
                    .Skip((paginateReq.PageNumber - 1) * paginateReq.PageSize)
                    .Take(paginateReq.PageSize).ToListAsync();
            }
            else
            {
                raw = await dbSet.OrderByDescending(orderBy)
                    .Skip((paginateReq.PageNumber - 1) * paginateReq.PageSize)
                    .Take(paginateReq.PageSize).ToListAsync();
            }

            var paginateRes = new PaginateRes<T>()
            {
                PageNumber = paginateReq.PageNumber,
                PageSize = paginateReq.PageSize,
                TotalCount = totalCount,
                PageCount = raw.Count(),
                TotalPages = (int)Math.Ceiling((decimal)totalCount/paginateReq.PageSize),
                Items = raw,
            };
            return paginateRes;
        }

        public async Task<ICollection<T>> GetAllDatatableAsync(Expression<Func<T, bool>> filter, Expression<Func<T, dynamic>> orderBy
            ,string dirSort, int start, int length)
        {
            var dbSet = _context.Set<T>().Where(filter);
            var raw = new List<T>();
            if (dirSort.Equals("asc"))
            {
                raw = await dbSet.OrderBy(orderBy)
                    .Skip(start).Take(length).ToListAsync();
            }
            else
            {
                raw = await dbSet.OrderByDescending(orderBy)
                    .Skip(start).Take(length).ToListAsync();
            }
            return raw;
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }

        public T? GetById(string id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void AddRange(IEnumerable<T> entites)
        {
            _context.Set<T>().AddRange(entites);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }


        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        } 

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

       
    }
}
