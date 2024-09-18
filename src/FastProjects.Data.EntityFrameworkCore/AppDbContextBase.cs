using System.Reflection;
using FastProjects.SharedKernel;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace FastProjects.Data.EntityFrameworkCore;

/// <summary>
/// Base class for the application's DbContext, providing common functionality for all derived DbContexts.
/// </summary>
/// <remarks>
/// This class includes configurations for domain event dispatching and MassTransit outbox/inbox entities.
/// </remarks>
public abstract class AppDbContextBase : DbContext
{
    private readonly IDomainEventDispatcher? _domainEventDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContextBase"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    /// <param name="domainEventDispatcher">The domain event dispatcher for handling domain events.</param>
    protected AppDbContextBase(
        DbContextOptions options,
        IDomainEventDispatcher? domainEventDispatcher)
        : base(options)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    /// <summary>
    /// Gets the assembly that contains the entity configurations.
    /// </summary>
    protected abstract Assembly ExecutingAssembly { get; }

    /// <summary>
    /// Configures the model that was discovered by convention from the entity types exposed in <see cref="DbSet{TEntity}"/> properties on the derived context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(ExecutingAssembly);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }

    /// <summary>
    /// Asynchronously saves all changes made in this context to the database.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        if (_domainEventDispatcher is null)
        {
            return result;
        }

        EntityBase[] entitiesWithEvents = ChangeTracker.Entries<EntityBase>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

        await _domainEventDispatcher.DispatchAndClearEvents(entitiesWithEvents);

        return result;
    }
}
