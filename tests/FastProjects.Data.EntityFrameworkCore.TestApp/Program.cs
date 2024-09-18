using FastEndpoints;
using FastEndpoints.Swagger;
using FastProjects.Data.EntityFrameworkCore;
using FastProjects.Data.EntityFrameworkCore.TestApp;
using FastProjects.Data.EntityFrameworkCore.TestProject;
using FastProjects.SharedKernel;
using MassTransit;
using Microsoft.EntityFrameworkCore;

/*
 *
 * To launch the app, run the docker-compose file in the root of this project:
 * > docker-compose -f docker-compose.yml up
 * This will run the database instance for the app.
 */

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// FastEndpoints and MediatR
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.DocumentName = "Initial Release";
        s.Title = "FastProjects.Data.EntityFrameworkCore Test API";
        s.Description = "API to test the FastProjects.Data.EntityFrameworkCore library";
        s.Version = "v0";
    };
});
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

// Database
builder.Services.AddDbContext<AppDbContext>(x =>
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
    x.UseNpgsql(connectionString);
});
builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepository<>));
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(EfCoreRepository<>));
builder.Services.AddScoped(typeof(EfCoreRepository<ProjectModel>));
builder.Services.AddSingleton<ISqlConnectionFactory>(_ =>
    new NpgsqlConnectionFactory(builder.Configuration.GetConnectionString("DefaultConnection")!));

// MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddEntityFrameworkOutbox<AppDbContext>(o =>
    {
        o.QueryDelay = TimeSpan.FromSeconds(15);

        o.UsePostgres();
        o.UseBusOutbox();
    });
    x.UsingInMemory();
});

WebApplication app = builder.Build();

// Migrations
app.Services.CreateScope()
    .ServiceProvider.GetRequiredService<AppDbContext>()
    .Database.Migrate();

// Endpoints and swagger
app.MapFastEndpoints();
app.UseSwaggerGen();

app.Run();

public partial class Program;
