using System.Linq.Expressions;

namespace GAROperations.Core.Interfaces.Repository
{
    public interface IGarRepository<T> where T : class
    {
        Task InsertAsync(T entity);
        Task InsertBulkAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(long objectId);
        Task<bool> IsExist(T entity);

        Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize, Expression<Func<T, bool>>? filter = null);
        Task<long> GetCountAsync(Expression<Func<T, bool>>? filter = null);
    }
}
