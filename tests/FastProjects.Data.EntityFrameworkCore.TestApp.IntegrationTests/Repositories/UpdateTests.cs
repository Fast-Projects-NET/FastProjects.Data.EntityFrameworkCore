using FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Repositories;

public sealed class UpdateTests(IntegrationTestWebAppFactory factory) : EfCoreRepositoryBaseTest(factory)
{
    [Fact]
    public async Task Update_Should_UpdateEntity()
    {
        // Arrange
        var project = ProjectModel.Create(Guid.NewGuid(), "Project 1");
        await ProjectRepository.AddAsync(project);
        await UnitOfWork.SaveChangesAsync();

        // Act
        project.UpdateName("Project 2");
        await ProjectRepository.UpdateAsync(project);
        await UnitOfWork.SaveChangesAsync();

        // Assert
        ProjectModel? projectFromDb = await ProjectReadRepository.GetByIdAsync(project.Id);
        projectFromDb.Should().NotBeNull();
        projectFromDb!.Id.Should().Be(project.Id);
        projectFromDb!.Name.Should().Be(project.Name);
    }
}
