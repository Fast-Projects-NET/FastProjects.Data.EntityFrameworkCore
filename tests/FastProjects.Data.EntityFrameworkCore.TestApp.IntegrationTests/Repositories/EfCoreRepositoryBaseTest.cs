using FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Infrastructure;
using FastProjects.SharedKernel;
using Microsoft.Extensions.DependencyInjection;

namespace FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Repositories;

public abstract class EfCoreRepositoryBaseTest(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    protected IRepository<ProjectModel> ProjectRepository =>
        Scope.ServiceProvider.GetRequiredService<IRepository<ProjectModel>>();
    
    protected IReadRepository<ProjectModel> ProjectReadRepository =>
        Scope.ServiceProvider.GetRequiredService<IRepository<ProjectModel>>();
    
    protected IUnitOfWork UnitOfWork =>
        Scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
}
