using System.ComponentModel.DataAnnotations;
using FastProjects.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastProjects.Data.EntityFrameworkCore.TestApp;

// Project Entity
public sealed class ProjectModel : EntityBase, IAggregateRoot
{
    public const int MaxNameLength = 100;
    
    public static ProjectModel Create(Guid id, string name)
    {
        var project = new ProjectModel(id, name);
        project.RegisterDomainEvent(new ProjectCreatedDomainEvent
        {
            Id = id,
            Name = name
        });
        return project;
    }
    
    [MaxLength(MaxNameLength)]
    public string Name { get; private set; } = null!;
    
    public void UpdateName(string name)
    {
        Name = name;
    }
    
    private ProjectModel(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    
    private ProjectModel()
    {
    }
}

// EntityFrameworkCore Configuration
public sealed class ProjectModelConfiguration : IEntityTypeConfiguration<ProjectModel>
{
    public void Configure(EntityTypeBuilder<ProjectModel> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(ProjectModel.MaxNameLength);
    }
}

// Events
public sealed class ProjectCreatedDomainEvent : DomainEventBase
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
}

public sealed class ProjectCreatedDomainEventHandler : IDomainEventHandler<ProjectCreatedDomainEvent>
{
    public async Task Handle(ProjectCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
    }
}
