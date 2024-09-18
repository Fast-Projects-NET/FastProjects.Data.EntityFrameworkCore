using FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Infrastructure;
using FluentAssertions;

namespace FastProjects.Data.EntityFrameworkCore.TestApp.IntegrationTests.Repositories;

public sealed class GetByIdTests(IntegrationTestWebAppFactory factory) : EfCoreRepositoryBaseTest(factory)
{
    [Fact]
    public async Task GetById_Should_NotReturnObject_WhenNotExists()
    {
        // Arrange

        // Act
        ProjectModel? project = await ProjectReadRepository.GetByIdAsync(Guid.NewGuid());

        // Assert
        project.Should().BeNull();
    }
    
    [Fact]
    public async Task GetById_Should_ReturnObject_WhenExists()
    {
        // Arrange
        var existingProject = ProjectModel.Create(ProjectData.Id, ProjectData.Name);
        await ProjectRepository.AddAsync(existingProject);
        await UnitOfWork.SaveChangesAsync();

        // Act
        ProjectModel? project = await ProjectReadRepository.GetByIdAsync(ProjectData.Id);

        // Assert
        project.Should().NotBeNull();
        project!.Id.Should().Be(existingProject.Id);
        project!.Name.Should().Be(existingProject.Name);
    }
}
