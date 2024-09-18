using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Infrastructure;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16.0")
        .WithDatabase("fastprojects")
        .WithUsername("fastprojects")
        .WithPassword("fastprojects")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Database
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql($"{_dbContainer.GetConnectionString()!}");
            });
            
            services.RemoveAll<ISqlConnectionFactory>();
            services.AddSingleton<ISqlConnectionFactory>(_ =>
                new NpgsqlConnectionFactory(_dbContainer.GetConnectionString()!));
        });
    }
    
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        
        await base.DisposeAsync();
    }
}
