using System.Linq.Expressions;

namespace NLPC.PCMS.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Find(int id);
        Task<T> Find(long id);
        Task<T> Find(string id);
        Task<T> Find(Guid id, bool ignoreGlobalFilter = false);
        Task<T> Find(Guid id, string columnName, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> Find();
        Task<T> Find(Expression<Func<T, bool>> match,
    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);

        Task<T> Find(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, IEnumerable<string>? includePaths = null, bool ignoreGlobalFilter = false);

        Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
IEnumerable<string>? includePaths = null, bool ignoreGlobalFilter = false, int? take = null);

        IQueryable<T> FindAllQuerable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    IEnumerable<string>? includePaths = null);

        Task<T> AddAsync(T entity);

        Task<IList<T>> AddRangeAsync(IList<T> entity);

        Task<T> EditAsync(T updated);

        Task<IList<T>> EditRange(IList<T> entity);
        Task Remove(T t);
        Task RemoveRange(IEnumerable<T> entities);
        Task<bool> Exist();
        Task<bool> Exist(Expression<Func<T, bool>> predicate);    
    }
}
