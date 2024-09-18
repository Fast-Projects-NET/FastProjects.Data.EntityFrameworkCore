using Microsoft.EntityFrameworkCore;

namespace FastProjects.Data.EntityFrameworkCore;

/// <summary>
/// Abstract base class for an EF Core repository that implements the <see cref="IRepository{T}"/> interface.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public abstract class EfCoreRepositoryBase<T>(DbContext dbContext)
    : IRepository<T>
    , IReadRepository<T> where T : class
{
    /// <summary>
    /// Gets the <see cref="DbContext"/> instance.
    /// </summary>
    protected DbContext DbContext { get; init; } = dbContext;

    /// <inheritdoc />
    public async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull =>
        await DbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);

    /// <inheritdoc />
    public async Task<List<T>> ListAsync(CancellationToken cancellationToken = default) =>
        await DbContext.Set<T>().ToListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<int> CountAsync(CancellationToken cancellationToken = default) =>
        await DbContext.Set<T>().CountAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default) =>
        await DbContext.Set<T>().AnyAsync(cancellationToken);

    /// <inheritdoc />
    public Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Add(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        DbContext.AddRange(entities);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Update(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        DbContext.UpdateRange(entities);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Remove(entity);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        DbContext.RemoveRange(entities);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        DbContext.SaveChangesAsync(cancellationToken);
}
