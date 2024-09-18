using FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Repositories;

public sealed class DeleteTests(IntegrationTestWebAppFactory factory) : EfCoreRepositoryBaseTest(factory)
{
    [Fact]
    public async Task Delete_Should_DeleteEntity()
    {
        // Arrange
        var project = ProjectModel.Create(Guid.NewGuid(), "Project 1");
        await ProjectRepository.AddAsync(project);
        await UnitOfWork.SaveChangesAsync();

        // Act
        await ProjectRepository.DeleteAsync(project);
        await UnitOfWork.SaveChangesAsync();

        // Assert
        ProjectModel? projectFromDb = await ProjectReadRepository.GetByIdAsync(project.Id);
        projectFromDb.Should().BeNull();
    }
}
