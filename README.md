# 🚀 **FastProjects.Data.EntityFrameworkCore**

![Build Status](https://github.com/Fast-Projects-NET/FastProjects.Data.EntityFrameworkCore/actions/workflows/test.yml/badge.svg)
![NuGet](https://img.shields.io/nuget/v/FastProjects.Data.EntityFrameworkCore.svg)
![NuGet Downloads](https://img.shields.io/nuget/dt/FastProjects.Data.EntityFrameworkCore.svg)
![License](https://img.shields.io/github/license/Fast-Projects-NET/FastProjects.Data.EntityFrameworkCore.svg)
![Last Commit](https://img.shields.io/github/last-commit/Fast-Projects-NET/FastProjects.Data.EntityFrameworkCore.svg)
![GitHub Stars](https://img.shields.io/github/stars/Fast-Projects-NET/FastProjects.Data.EntityFrameworkCore.svg)
![GitHub Forks](https://img.shields.io/github/forks/Fast-Projects-NET/FastProjects.Data.EntityFrameworkCore.svg)

> 🚨 ALERT: Project Under Development
> This project is not yet production-ready and is still under active development. Currently, it's being used primarily for personal development needs. However, contributions are more than welcome! If you'd like to collaborate, feel free to submit issues or pull requests. Your input can help shape the future of FastProjects!

---

## 📚 **Overview**

FastProjects.Data.EntityFrameworkCore is a library that provides a base DbContext class with MassTransit outbox support. It's designed to be used in conjunction with the FastProjects framework, but it can also be used as a standalone library.

---

## 🛠 **Roadmap**

- ✅ [AppDbContextBase](src/FastProjects.Data.EntityFrameworkCore/AppDbContextBase.cs) - Base class for DbContext that provides outbox support for MassTransit
- ✅ [EfCoreRepositoryBase](src/FastProjects.Data.EntityFrameworkCore/EfCoreRepositoryBase.cs) - Base class for repository that provides CRUD operations for entities
- ✅ [NpgsqlConnectionFactory](src/FastProjects.Data.EntityFrameworkCore/NpgsqlConnectionFactory.cs) - Factory class for creating NpgsqlConnection objects
- ⏳ [OutBoxProcessingJob]() - Background job for processing outbox messages to handle MediatR events after transaction commit
- ⏳ [OutBoxProcessingJobConfiguration]() - Configuration class for OutBoxProcessingJob

---

## Usage

Registering the services:
```csharp
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
```

Classes:
```csharp
// DbContext
public sealed class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IDomainEventDispatcher? domainEventDispatcher)
    : AppDbContextBase(options, domainEventDispatcher), IUnitOfWork
{
    protected override Assembly ExecutingAssembly => Assembly.GetExecutingAssembly();
    
    public DbSet<ProjectModel> Projects => Set<ProjectModel>();
}

// Generic Repository
public class EfCoreRepository<T>(AppDbContext appDbContext)
    : EfCoreRepositoryBase<T>(appDbContext)
    where T : class, IAggregateRoot;

// Usage
public sealed class CreateProjectCommandHandler(
    EfCoreRepository<ProjectModel> projectCoreRepository,
    IUnitOfWork unitOfWork)
    : SharedKernel.ICommandHandler<CreateProjectCommand>
{
    public async Task<Result> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var newProject = ProjectModel.Create(Guid.NewGuid(), request.Name);
        await projectCoreRepository.AddAsync(newProject, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
```

---

## 🚀 **Installation**

You can download the NuGet package using the following command to install:
```bash
dotnet add package FastProjects.Data.EntityFrameworkCore
```

---

## 🤝 **Contributing**

This project is still under development, but contributions are welcome! Whether you’re opening issues, submitting pull requests, or suggesting new features, we appreciate your involvement. For more details, please check the [contribution guide](CONTRIBUTING.md). Let’s build something amazing together! 🎉

---

## 📄 **License**

FastProjects is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for full details.
