using System.Linq.Expressions;

namespace Common.Persistence.EFCore.Abstractions;

public interface IRepository<TEntity, TKey> where TEntity : class
{
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken token = default);
    Task<IEnumerable<TEntity>> CreateAsync(IEnumerable<TEntity> entities, CancellationToken token = default);
    Task<TEntity> RemoveAsync(TKey id, CancellationToken token = default);
    Task<TEntity> UpdateAsync(TKey id, TEntity entity, CancellationToken token = default);
    Task<IEnumerable<TEntity>> UpdateAsync(IDictionary<TKey, TEntity> entities, CancellationToken token = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default);
    Task<TEntity> GetAsync(TKey id, CancellationToken token = default);
    IQueryable<TEntity> AsQueryable();

    Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken token = default);
}