using System.Reflection;
using FastProjects.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace FastProjects.Data.EntityFrameworkCore.TestApp;

public sealed class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IDomainEventDispatcher? domainEventDispatcher)
    : AppDbContextBase(options, domainEventDispatcher), IUnitOfWork
{
    protected override Assembly ExecutingAssembly => Assembly.GetExecutingAssembly();
    
    public DbSet<ProjectModel> Projects => Set<ProjectModel>();
}
