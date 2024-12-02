using Moq;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Repository.Services.DepartmentRepo;
using UKParliament.CodeTest.Services.AppServices;
using Xunit;

namespace UKParliament.CodeTest.Tests.Application.Service.Test;

public class DepartmentServiceTests
{
    private readonly Mock<IDepartmentRepository> _mockRepository;
    private readonly DepartmentService _service;

    public DepartmentServiceTests()
    {
        _mockRepository = new Mock<IDepartmentRepository>();
        _service = new DepartmentService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllDepartments()
    {
        // Arrange
        var departments = new List<Department>
            {
                new Department { Id = 1, Name = "Sales" },
                new Department { Id = 2, Name = "Marketing" }
            };

        _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(departments);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDepartment_WhenIdExists()
    {
        // Arrange
        var id = 1;
        var department = new Department { Id = id, Name = "Sales" };

        _mockRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(department);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Sales", result?.Name);
        _mockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenIdDoesNotExist()
    {
        // Arrange
        var id = 999;

        _mockRepository.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Department?)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(repo => repo.GetByIdAsync(id), Times.Once);
    }
}
