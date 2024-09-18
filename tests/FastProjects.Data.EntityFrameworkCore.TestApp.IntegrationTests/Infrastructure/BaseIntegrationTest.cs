using Microsoft.Extensions.DependencyInjection;

namespace FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Infrastructure;

public abstract class BaseIntegrationTest(IntegrationTestWebAppFactory factory) : IClassFixture<IntegrationTestWebAppFactory>
{
    protected IServiceScope Scope { get; } = factory.Services.CreateScope();
}
