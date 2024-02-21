using System.Linq.Expressions;
using Common.Persistence.EFCore.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Common.Persistence.EFCore;

public abstract class BaseRepository<TEntity, TKey, TContext> : IEfCoreRepository<TEntity, TKey>
    where TEntity : class
    where TContext : DbContext
{
    protected DbSet<TEntity> DbSet { get; }
    protected TContext DbContext { get; }

    protected BaseRepository(TContext context)
    {
        DbContext = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default)
    {
        return await DbSet.ToListAsync(token);
    }

    public virtual async Task<TEntity> GetAsync(TKey id, CancellationToken token = default)
    {
        return await DbSet.FindAsync(new object[] {id}, token);
    }

    public virtual IQueryable<TEntity> AsQueryable()
    {
        return DbSet;
    }

    public virtual async Task<IEnumerable<TEntity>> SearchAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken token = default)
    {
        return await DbSet.Where(predicate).ToListAsync(token);
    }

    public virtual TEntity Create(TEntity entity)
    {
        DbSet.Add(entity);
        return entity;
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken token = default)
    {
        await DbSet.AddAsync(entity, token);
        return entity;
    }

    public virtual IEnumerable<TEntity> Create(IEnumerable<TEntity> entities)
    {
        DbSet.AddRange(entities);
        return entities;
    }

    public virtual async Task<IEnumerable<TEntity>> CreateAsync(
        IEnumerable<TEntity> entities,
        CancellationToken token = default)
    {
        await DbSet.AddRangeAsync(entities, token);
        return entities;
    }

    public virtual async Task<TEntity> RemoveAsync(TKey id, CancellationToken token = default)
    {
        var entity = await GetAsync(id, token);
        if (entity != null)
        {
            DbSet.Remove(entity);
        }

        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(
        TKey id,
        TEntity entity,
        CancellationToken token = default)
    {
        var existingEntity = await GetAsync(id, token);
        if (existingEntity != null)
        {
            DbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        }

        return existingEntity;
    }

    public virtual async Task<IEnumerable<TEntity>> UpdateAsync(
        IDictionary<TKey, TEntity> entities,
        CancellationToken token = default)
    {
        foreach (var entity in entities)
        {
            await UpdateAsync(entity.Key, entity.Value, token);
        }

        return entities.Values;
    }

    public async Task CommitAsync(CancellationToken token = default)
    {
        await DbContext.SaveChangesAsync(token);
    }
}