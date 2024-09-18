using FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Repositories;

public sealed class UpdateRangeTests(IntegrationTestWebAppFactory factory) : EfCoreRepositoryBaseTest(factory)
{
    [Fact]
    public async Task UpdateRange_Should_UpdateEntities()
    {
        // Arrange
        var project1 = ProjectModel.Create(Guid.NewGuid(), "Project 1");
        var project2 = ProjectModel.Create(Guid.NewGuid(), "Project 2");
        await ProjectRepository.AddRangeAsync([project1, project2]);
        await UnitOfWork.SaveChangesAsync();

        // Act
        project1.UpdateName("Project 1 Updated");
        project2.UpdateName("Project 2 Updated");
        await ProjectRepository.UpdateRangeAsync([project1, project2]);
        await UnitOfWork.SaveChangesAsync();

        // Assert
        ProjectModel? project1FromDb = await ProjectReadRepository.GetByIdAsync(project1.Id);
        project1FromDb.Should().NotBeNull();
        project1FromDb!.Id.Should().Be(project1.Id);
        project1FromDb!.Name.Should().Be(project1.Name);

        ProjectModel? project2FromDb = await ProjectReadRepository.GetByIdAsync(project2.Id);
        project2FromDb.Should().NotBeNull();
        project2FromDb!.Id.Should().Be(project2.Id);
        project2FromDb!.Name.Should().Be(project2.Name);
    }
}
