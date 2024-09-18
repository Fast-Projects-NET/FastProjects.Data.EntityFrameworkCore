using FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Repositories;

public sealed class CountTests(IntegrationTestWebAppFactory factory) : EfCoreRepositoryBaseTest(factory)
{
    [Fact]
    public async Task Count_Should_ReturnZero_WhenNoEntities()
    {
        // Arrange

        // Act
        int count = await ProjectReadRepository.CountAsync();

        // Assert
        count.Should().Be(0);
    }

    [Fact]
    public async Task Count_Should_ReturnNumberOfEntities_WhenExist()
    {
        // Arrange
        var project1 = ProjectModel.Create(Guid.NewGuid(), "Project 1");
        var project2 = ProjectModel.Create(Guid.NewGuid(), "Project 2");
        await ProjectRepository.AddAsync(project1);
        await ProjectRepository.AddAsync(project2);
        await UnitOfWork.SaveChangesAsync();

        // Act
        int count = await ProjectReadRepository.CountAsync();

        // Assert
        count.Should().Be(2);
    }
}
