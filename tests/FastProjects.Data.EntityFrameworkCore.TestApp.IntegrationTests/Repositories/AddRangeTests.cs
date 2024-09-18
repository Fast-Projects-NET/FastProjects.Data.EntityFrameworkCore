using FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Repositories;

public sealed class AddRangeTests(IntegrationTestWebAppFactory factory) : EfCoreRepositoryBaseTest(factory)
{
    [Fact]
    public async Task AddRange_Should_AddEntities()
    {
        // Arrange
        var project1 = ProjectModel.Create(Guid.NewGuid(), "Project 1");
        var project2 = ProjectModel.Create(Guid.NewGuid(), "Project 2");
        var project3 = ProjectModel.Create(Guid.NewGuid(), "Project 3");

        // Act
        await ProjectRepository.AddRangeAsync(new[] { project1, project2, project3 });
        await UnitOfWork.SaveChangesAsync();

        // Assert
        ProjectModel? projectFromDb1 = await ProjectReadRepository.GetByIdAsync(project1.Id);
        projectFromDb1.Should().NotBeNull();
        projectFromDb1!.Id.Should().Be(project1.Id);
        projectFromDb1!.Name.Should().Be(project1.Name);

        ProjectModel? projectFromDb2 = await ProjectReadRepository.GetByIdAsync(project2.Id);
        projectFromDb2.Should().NotBeNull();
        projectFromDb2!.Id.Should().Be(project2.Id);
        projectFromDb2!.Name.Should().Be(project2.Name);

        ProjectModel? projectFromDb3 = await ProjectReadRepository.GetByIdAsync(project3.Id);
        projectFromDb3.Should().NotBeNull();
        projectFromDb3!.Id.Should().Be(project3.Id);
        projectFromDb3!.Name.Should().Be(project3.Name);
    }
}
