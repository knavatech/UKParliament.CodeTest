using Microsoft.AspNetCore.Mvc;
using Moq;
using UKParliament.CodeTest.API.Controllers;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.AppServices;
using Xunit;

namespace UKParliament.CodeTest.Tests.API.Controller.Test;

public class DepartmentControllerTests
{
    private readonly Mock<IDepartmentService> _mockService;
    private readonly DepartmentController _controller;

    public DepartmentControllerTests()
    {
        _mockService = new Mock<IDepartmentService>();
        _controller = new DepartmentController(_mockService.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkWithDepartments()
    {
        // Arrange
        var departments = new List<Department>
            {
                new Department { Id = 1, Name = "Sales" },
                new Department { Id = 2, Name = "Marketing" }
            };

        _mockService.Setup(service => service.GetAllAsync()).ReturnsAsync(departments);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(departments, okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenDepartmentExists()
    {
        // Arrange
        var id = 1;
        var department = new Department { Id = id, Name = "Sales" };

        _mockService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync(department);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(department, okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenDepartmentDoesNotExist()
    {
        // Arrange
        var id = 999;

        _mockService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync((Department?)null);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
