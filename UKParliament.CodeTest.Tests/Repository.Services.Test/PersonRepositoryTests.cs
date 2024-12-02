using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Repository.Services.PersonRepo;
using UKParliament.CodeTest.Tests.Helper;
using Xunit;

namespace UKParliament.CodeTest.Tests.Repository.Services.Test;

public class PersonRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly PersonManagerContext _context;
    private readonly PersonRepository _repository;

    public PersonRepositoryTests()
    {
        _context = InMemoryDatabase.GetDbContext(out _connection);
        _repository = new PersonRepository(_context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPersons()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(6, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnPerson_WhenPersonExists()
    {
        // Arrange
        var personId = _context.People.First().PersonId;

        // Act
        var result = await _repository.GetByIdAsync(personId);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenPersonDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldAddPerson()
    {
        // Arrange
        var departmentId = _context.Departments.First().Id;
        var newPerson = new Person
        {
            PersonId = Guid.NewGuid(),
            FirstName = "Alice",
            LastName = "Wonderland",
            DateOfBirth = new DateTime(1995, 8, 24),
            DepartmentId = departmentId
        };

        // Act
        await _repository.AddAsync(newPerson);

        // Assert
        var result = _context.People.FirstOrDefault(p => p.FirstName == "Alice");
        Assert.NotNull(result);
        Assert.Equal("Alice", result.FirstName);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdatePerson()
    {
        // Arrange
        var person = _context.People.First();
        person.FirstName = "UpdatedName";

        // Act
        await _repository.UpdateAsync(person);

        // Assert
        var result = _context.People.First(p => p.PersonId == person.PersonId);
        Assert.Equal("UpdatedName", result.FirstName);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemovePerson()
    {
        // Arrange
        var person = _context.People.First();

        // Act
        await _repository.DeleteAsync(person.PersonId);

        // Assert
        var result = _context.People.FirstOrDefault(p => p.PersonId == person.PersonId);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetDepartmentAsync_ShouldReturnDepartment_WhenPersonExists()
    {
        // Arrange Sales department Id is 1
        var person = _context.People.Include(p => p.Department).FirstOrDefault(d=>d.DepartmentId == 1);

        // Act
        var result = await _repository.GetDepartmentAsync(person.PersonId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Sales", result.Name);
    }

    [Fact]
    public async Task GetDepartmentAsync_ShouldReturnNull_WhenPersonDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _repository.GetDepartmentAsync(nonExistentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowException_WhenDepartmentIsInvalid()
    {
        // Arrange
        var newPerson = new Person
        {
            PersonId = Guid.NewGuid(),
            FirstName = "Invalid",
            LastName = "Department",
            DateOfBirth = DateTime.UtcNow,
            DepartmentId = 0 // Invalid DepartmentId
        };

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => _repository.AddAsync(newPerson));
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Close();
        _connection.Dispose();
    }
}
