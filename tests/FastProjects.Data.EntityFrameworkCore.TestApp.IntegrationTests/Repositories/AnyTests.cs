using FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Repositories;

public sealed class AnyTests(IntegrationTestWebAppFactory factory) : EfCoreRepositoryBaseTest(factory)
{
    [Fact]
    public async Task Any_Should_ReturnFalse_WhenNoEntities()
    {
        // Arrange
        List<ProjectModel> projects = await ProjectReadRepository.ListAsync();
        await ProjectRepository.DeleteRangeAsync(projects);
        await UnitOfWork.SaveChangesAsync();

        // Act
        bool any = await ProjectReadRepository.AnyAsync();

        // Assert
        any.Should().BeFalse();
    }

    [Fact]
    public async Task Any_Should_ReturnTrue_WhenExist()
    {
        // Arrange
        var project = ProjectModel.Create(Guid.NewGuid(), "Project 1");
        await ProjectRepository.AddAsync(project);
        await UnitOfWork.SaveChangesAsync();

        // Act
        bool any = await ProjectReadRepository.AnyAsync();

        // Assert
        any.Should().BeTrue();
    }
}
