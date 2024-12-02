using AutoMapper;
using Moq;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.DTO.Department;
using UKParliament.CodeTest.DTO.Person;
using UKParliament.CodeTest.Repository.Services.PersonRepo;
using UKParliament.CodeTest.Services.AppServices;
using Xunit;

namespace UKParliament.CodeTest.Tests.Application.Service.Test;

public class PersonServiceTests
{
    private readonly Mock<IPersonRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly PersonService _service;

    public PersonServiceTests()
    {
        _mockRepository = new Mock<IPersonRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new PersonService(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPersons()
    {
        // Arrange
        var persons = new List<PersonDTO>
        {
            new PersonDTO { PersonId = Guid.NewGuid(), FirstName = "John", LastName = "Doe" },
            new PersonDTO { PersonId = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith" }
        };

        _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(persons);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnPerson_WhenPersonExists()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var person = new PersonDTO { PersonId = guid, FirstName = "John", LastName = "Doe" };

        _mockRepository.Setup(repo => repo.GetByIdAsync(guid)).ReturnsAsync(person);

        // Act
        var result = await _service.GetByIdAsync(guid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result?.FirstName);
        _mockRepository.Verify(repo => repo.GetByIdAsync(guid), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenPersonDoesNotExist()
    {
        // Arrange
        var guid = Guid.NewGuid();

        _mockRepository.Setup(repo => repo.GetByIdAsync(guid)).ReturnsAsync((PersonDTO?)null);

        // Act
        var result = await _service.GetByIdAsync(guid);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(repo => repo.GetByIdAsync(guid), Times.Once);
    }

    [Fact]
    public async Task GetDepartmentAsync_ShouldReturnMappedDepartment_WhenExists()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var department = new Department { Id = 1, Name = "Engineering" };
        var departmentDTO = new DepartmentDTO { Id = 1, Name = "Engineering" };

        _mockRepository.Setup(repo => repo.GetDepartmentAsync(guid)).ReturnsAsync(department);
        _mockMapper.Setup(mapper => mapper.Map<DepartmentDTO>(department)).Returns(departmentDTO);

        // Act
        var result = await _service.GetDepartmentAsync(guid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Engineering", result?.Name);
        _mockRepository.Verify(repo => repo.GetDepartmentAsync(guid), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<DepartmentDTO>(department), Times.Once);
    }

    [Fact]
    public async Task GetDepartmentAsync_ShouldReturnNull_WhenDepartmentDoesNotExist()
    {
        // Arrange
        var guid = Guid.NewGuid();

        _mockRepository.Setup(repo => repo.GetDepartmentAsync(guid)).ReturnsAsync((Department?)null);

        // Act
        var result = await _service.GetDepartmentAsync(guid);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(repo => repo.GetDepartmentAsync(guid), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<DepartmentDTO>(It.IsAny<Department>()), Times.Never);
    }

    [Fact]
    public async Task AddAsync_ShouldAddPerson()
    {
        // Arrange
        var personDTO = new PersonDTO { FirstName = "Alice", LastName = "Wonderland" };
        var person = new Person { FirstName = "Alice", LastName = "Wonderland" };

        _mockMapper.Setup(mapper => mapper.Map<Person>(personDTO)).Returns(person);
        _mockRepository.Setup(repo => repo.AddAsync(person)).Returns(Task.CompletedTask);

        // Act
        await _service.AddAsync(personDTO);

        // Assert
        _mockMapper.Verify(mapper => mapper.Map<Person>(personDTO), Times.Once);
        _mockRepository.Verify(repo => repo.AddAsync(person), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdatePerson()
    {
        // Arrange
        var personDTO = new PersonDTO { PersonId = Guid.NewGuid(), FirstName = "Updated", LastName = "Name" };
        var person = new Person { PersonId = personDTO.PersonId, FirstName = "Updated", LastName = "Name" };

        _mockMapper.Setup(mapper => mapper.Map<Person>(personDTO)).Returns(person);
        _mockRepository.Setup(repo => repo.UpdateAsync(person)).Returns(Task.CompletedTask);

        // Act
        await _service.UpdateAsync(personDTO);

        // Assert
        _mockMapper.Verify(mapper => mapper.Map<Person>(personDTO), Times.Once);
        _mockRepository.Verify(repo => repo.UpdateAsync(person), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeletePerson()
    {
        // Arrange
        var guid = Guid.NewGuid();

        _mockRepository.Setup(repo => repo.DeleteAsync(guid)).Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(guid);

        // Assert
        _mockRepository.Verify(repo => repo.DeleteAsync(guid), Times.Once);
    }
}
