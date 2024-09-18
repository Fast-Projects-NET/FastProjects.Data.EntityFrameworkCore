using FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Repositories;

public sealed class AddTests(IntegrationTestWebAppFactory factory) : EfCoreRepositoryBaseTest(factory)
{
    [Fact]
    public async Task Add_Should_AddEntity()
    {
        // Arrange
        var project = ProjectModel.Create(Guid.NewGuid(), "Project 1");

        // Act
        await ProjectRepository.AddAsync(project);
        await UnitOfWork.SaveChangesAsync();

        // Assert
        ProjectModel? projectFromDb = await ProjectReadRepository.GetByIdAsync(project.Id);
        projectFromDb.Should().NotBeNull();
        projectFromDb!.Id.Should().Be(project.Id);
        projectFromDb!.Name.Should().Be(project.Name);
    }
}
