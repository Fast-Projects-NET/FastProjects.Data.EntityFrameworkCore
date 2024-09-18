using FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Repositories;

public sealed class DeleteRangeTests(IntegrationTestWebAppFactory factory) : EfCoreRepositoryBaseTest(factory)
{
    [Fact]
    public async Task DeleteRange_Should_DeleteEntities()
    {
        // Arrange
        var project1 = ProjectModel.Create(Guid.NewGuid(), "Project 1");
        var project2 = ProjectModel.Create(Guid.NewGuid(), "Project 2");
        await ProjectRepository.AddRangeAsync(new[] { project1, project2 });
        await UnitOfWork.SaveChangesAsync();

        // Act
        await ProjectRepository.DeleteRangeAsync(new[] { project1, project2 });
        await UnitOfWork.SaveChangesAsync();

        // Assert
        ProjectModel? projectFromDb1 = await ProjectReadRepository.GetByIdAsync(project1.Id);
        projectFromDb1.Should().BeNull();

        ProjectModel? projectFromDb2 = await ProjectReadRepository.GetByIdAsync(project2.Id);
        projectFromDb2.Should().BeNull();
    }
}
