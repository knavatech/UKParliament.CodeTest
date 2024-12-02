using Microsoft.AspNetCore.Mvc;
using Moq;
using UKParliament.CodeTest.API.Controllers;
using UKParliament.CodeTest.DTO.Department;
using UKParliament.CodeTest.DTO.Person;
using UKParliament.CodeTest.Services.AppServices;
using Xunit;

namespace UKParliament.CodeTest.Tests.API.Controller.Test;

public class PersonControllerTests
{
    private readonly Mock<IPersonService> _mockService;
    private readonly PersonController _controller;

    public PersonControllerTests()
    {
        _mockService = new Mock<IPersonService>();
        _controller = new PersonController(_mockService.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkWithPersons()
    {
        // Arrange
        var persons = new List<PersonDTO>
            {
                new PersonDTO { PersonId = Guid.NewGuid(), FirstName = "John", LastName = "Doe" },
                new PersonDTO { PersonId = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith" }
            };

        _mockService.Setup(service => service.GetAllAsync()).ReturnsAsync(persons);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(persons, okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenPersonExists()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var person = new PersonDTO { PersonId = guid, FirstName = "John", LastName = "Doe" };

        _mockService.Setup(service => service.GetByIdAsync(guid)).ReturnsAsync(person);

        // Act
        var result = await _controller.GetById(guid);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(person, okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenPersonDoesNotExist()
    {
        // Arrange
        var guid = Guid.NewGuid();

        _mockService.Setup(service => service.GetByIdAsync(guid)).ReturnsAsync((PersonDTO?)null);

        // Act
        var result = await _controller.GetById(guid);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetPersonDepartment_ShouldReturnOk_WhenDepartmentExists()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var department = new DepartmentDTO { Id = 1, Name = "Engineering" };

        _mockService.Setup(service => service.GetDepartmentAsync(guid)).ReturnsAsync(department);

        // Act
        var result = await _controller.GetPersonDepartment(guid);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(department, okResult.Value);
    }

    [Fact]
    public async Task GetPersonDepartment_ShouldReturnNotFound_WhenDepartmentDoesNotExist()
    {
        // Arrange
        var guid = Guid.NewGuid();

        _mockService.Setup(service => service.GetDepartmentAsync(guid)).ReturnsAsync((DepartmentDTO?)null);

        // Act
        var result = await _controller.GetPersonDepartment(guid);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Add_ShouldReturnCreated_WhenPersonIsValid()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var person = new PersonDTO { PersonId = guid, FirstName = "Alice", LastName = "Wonderland" };

        _mockService.Setup(service => service.AddAsync(person)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Add(person);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
        Assert.Equal(person, createdResult.Value);
    }

    [Fact]
    public async Task Add_ShouldReturnBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("FirstName", "Required");

        var person = new PersonDTO { PersonId = Guid.NewGuid() };

        // Act
        var result = await _controller.Add(person);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenPersonIsValid()
    {
        // Arrange
        var person = new PersonDTO { PersonId = Guid.NewGuid(), FirstName = "Updated", LastName = "Name" };

        _mockService.Setup(service => service.UpdateAsync(person)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Update(person);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenPersonIdIsEmpty()
    {
        // Arrange
        var person = new PersonDTO { PersonId = Guid.Empty, FirstName = "Invalid", LastName = "Person" };

        // Act
        var result = await _controller.Update(person);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnOk_WhenPersonIsDeleted()
    {
        // Arrange
        var guid = Guid.NewGuid();

        _mockService.Setup(service => service.DeleteAsync(guid)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(guid);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Add_ShouldReturnBadRequest_WhenModelIsInvalid()
    {
        // Arrange
        var person = new PersonDTO
        {
            // Missing FirstName, LastName, and DateOfBirth
            PersonId = Guid.NewGuid(),
            DepartmentId = 1
        };

        _controller.ModelState.AddModelError("FirstName", "First name is required");
        _controller.ModelState.AddModelError("DateOfBirth", "Date of birth is required");

        // Act
        var result = await _controller.Add(person);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);

        var errors = Assert.IsType<SerializableError>(badRequestResult.Value);
        Assert.Contains("FirstName", errors.Keys);
        Assert.Contains("DateOfBirth", errors.Keys);
    }
}
