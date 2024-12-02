using Microsoft.Data.Sqlite;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Repository.Services.DepartmentRepo;
using UKParliament.CodeTest.Tests.Helper;
using Xunit;

namespace UKParliament.CodeTest.Tests.Repository.Services.Test;

public class DepartmentRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly PersonManagerContext _context;
    private readonly DepartmentRepository _repository;

    public DepartmentRepositoryTests()
    {
        _context = InMemoryDatabase.GetDbContext(out _connection);
        _repository = new DepartmentRepository(_context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllDepartments()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDepartment_WhenIdExists()
    {
        // Arrange
        var id = 1; // Existing DepartmentId

        // Act
        var result = await _repository.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Sales", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenIdDoesNotExist()
    {
        // Arrange
        var id = 999; // Non-existent DepartmentId

        // Act
        var result = await _repository.GetByIdAsync(id);

        // Assert
        Assert.Null(result);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Close();
        _connection.Dispose();
    }
}
