using FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Repositories;

public sealed class ListTests(IntegrationTestWebAppFactory factory) : EfCoreRepositoryBaseTest(factory)
{
    [Fact]
    public async Task List_Should_ReturnEmptyList_WhenNoEntities()
    {
        // Arrange
        List<ProjectModel> projects = await ProjectReadRepository.ListAsync();
        await ProjectRepository.DeleteRangeAsync(projects);
        await UnitOfWork.SaveChangesAsync();

        // Act
        List<ProjectModel> listProjects = await ProjectReadRepository.ListAsync();

        // Assert
        listProjects.Should().BeEmpty();
    }

    [Fact]
    public async Task List_Should_ReturnEntities_WhenExist()
    {
        // Arrange
        var project1 = ProjectModel.Create(Guid.NewGuid(), "Project 1");
        var project2 = ProjectModel.Create(Guid.NewGuid(), "Project 2");
        await ProjectRepository.AddAsync(project1);
        await ProjectRepository.AddAsync(project2);
        await UnitOfWork.SaveChangesAsync();

        // Act
        List<ProjectModel> projects = await ProjectReadRepository.ListAsync();

        // Assert
        projects.Should().HaveCount(2);
        projects.Should().ContainSingle(p => p.Id == project1.Id && p.Name == project1.Name);
        projects.Should().ContainSingle(p => p.Id == project2.Id && p.Name == project2.Name);
    }
}
