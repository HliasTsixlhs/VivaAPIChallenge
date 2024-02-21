namespace Common.Persistence.EFCore.Abstractions;

public interface IEfCoreRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
{
    TEntity Create(TEntity entity);
    IEnumerable<TEntity> Create(IEnumerable<TEntity> entities);
    Task CommitAsync(CancellationToken token = default);
}