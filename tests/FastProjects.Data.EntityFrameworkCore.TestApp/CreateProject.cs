using FastEndpoints;
using FastProjects.Endpoints;
using FastProjects.ResultPattern;
using FastProjects.SharedKernel;
using FluentValidation;
using MediatR;
using ICommand = FastProjects.SharedKernel.ICommand;

namespace FastProjects.Data.EntityFrameworkCore.TestApp;

// Application layer below implemented via MediatR
public sealed record CreateProjectCommand(string Name) : ICommand;
    
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

// Presentation layer below implemented via FastEndpoints

public sealed class CreateProjectRequest
{
    public string Name { get; init; }
}

public sealed class CreateProjectValidator : Validator<CreateProjectRequest>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ProjectModel.MaxNameLength);
    }
}

public sealed class CreateProjectEndpoint(IMediator mediator)
    : FastEndpointWithoutResponse<CreateProjectRequest,
        CreateProjectCommand,
        Result>(mediator)
{
    public override void Configure()
    {
        Post("/projects");
        Version(0);
        AllowAnonymous();
        
        Summary(s =>
        {
            s.ExampleRequest = new CreateProjectRequest { Name = $"Test Project" };
        });
    }
    
    protected override CreateProjectCommand CreateMediatorCommand(CreateProjectRequest request) =>
        new(request.Name);
}
