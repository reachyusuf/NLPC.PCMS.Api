using Microsoft.EntityFrameworkCore;
using Mware.CollegeDreams.Infrastructure.Persistence;
using NLPC.PCMS.Application.Interfaces.Repositories;
using System.Linq.Expressions;

namespace NLPC.PCMS.Infrastructure.Persistence.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDBContext _context;
        private readonly IUnitofWork _unitOfWork;

        public GenericRepository(AppDBContext context)
        {
            _context = context;
            _unitOfWork = new UnitofWork(context);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<IList<T>> AddRangeAsync(IList<T> entity)
        {
            await _context.Set<T>().AddRangeAsync(entity);
            return entity;
        }

        public Task<T> EditAsync(T updated)
        {
            return Task.Run(() =>
            {
                if (updated is null)
                {
                    return null;
                }

                _context.Set<T>().Attach(updated);
                _context.Entry(updated).State = EntityState.Modified;
                return updated;
            });
        }

        public Task<IList<T>> EditRange(IList<T> entity)
        {
            return Task.Run(() =>
            {
                _context.Set<T>().UpdateRange(entity);
                return entity;
            });
        }

        public Task Remove(T t)
        {
            return Task.Run(() => _context.Set<T>().Remove(t));
        }

        public Task RemoveRange(IEnumerable<T> entities)
        {
            return Task.Run(() => _context.Set<T>().RemoveRange(entities));
        }

        public async Task<bool> Exist(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> exist = _context.Set<T>().Where(predicate);
            return await exist.AnyAsync();
        }

        public async Task<bool> Exist()
        {
            return await _context.Set<T>().AnyAsync();
        }

        public async Task<T> Find(Expression<Func<T, bool>> match,
    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            IQueryable<T> query = _context.Set<T>();
            if (match != null)
            {
                query = query.Where(match);
            }
            //return await _context.Set<T>().FirstOrDefaultAsync(match);
            if (orderBy != null)
            {
                return await orderBy(query).FirstOrDefaultAsync();
            }
            else
            {
                return await query.FirstOrDefaultAsync();
            }
        }

        public async Task<T> Find(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
IEnumerable<string>? includePaths = null, bool ignoreGlobalFilter = false)
        {
            IQueryable<T> query = _context.Set<T>();
            if (ignoreGlobalFilter is true)
                query = query.IgnoreQueryFilters();

            if (filter is not null)
                query = query.Where(filter);

            if (orderBy is not null)
                query = orderBy(query);

            if (includePaths is not null)
                query = includePaths.Aggregate(query, (current, includePath) => current.Include(includePath));

            return await query.FirstOrDefaultAsync() ?? default(T)!;
        }

        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
IEnumerable<string>? includePaths = null, bool ignoreGlobalFilter = false, int? take = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (ignoreGlobalFilter is true)
                query = query.IgnoreQueryFilters();

            if (filter is not null)
                query = query.Where(filter);

            if (orderBy is not null)
                query = orderBy(query);

            if (includePaths is not null)
                query = includePaths.Aggregate(query, (current, includePath) => current.Include(includePath));

            if (take > 0)
                query = query.Take(take ?? 0);

            return await query.ToListAsync();
        }

        public IQueryable<T> FindAllQuerable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
IEnumerable<string>? includePaths = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (includePaths is not null)
            {
                query = includePaths.Aggregate(query, (current, includePath) => current.Include(includePath));
            }

            //if (includePaths is not null)
            //{
            //    //foreach (string includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            //    //    .Where(x => !string.IsNullOrWhiteSpace(x))
            //    //    .Select(x => x.Trim()))
            //    //{
            //    //    query = query.Include(includeProperty);
            //    //}

            //    foreach (var item in includePaths)
            //    {
            //        query = query.Include(item);
            //    }
            //}

            return query; //.ToListAsync();
        }

        public async Task<T> Find(long id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Find(Guid id, bool ignoreGlobalFilter = false)
        {
            //return await _context.Set<T>().FindAsync(id);

            var query = _context.Set<T>(); //.FirstOrDefaultAsync(match);
            if (ignoreGlobalFilter is true) query.IgnoreQueryFilters();
            return await query.FindAsync(id);
        }

        public async Task<T> Find(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Find(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Find(Guid id, string columnName, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();

            // Apply includes if provided
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            // Use FirstOrDefaultAsync to get the entity with the specified id
            return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, columnName) == id);
        }

        public IQueryable<T> Find()
        {
            return _context.Set<T>().AsQueryable();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _unitOfWork.SaveChanges();
        }
    }
}
