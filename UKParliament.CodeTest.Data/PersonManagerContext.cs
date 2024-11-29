using Microsoft.EntityFrameworkCore;
namespace UKParliament.CodeTest.Data;

public class PersonManagerContext : DbContext
{
    public PersonManagerContext(DbContextOptions<PersonManagerContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Sales" },
            new Department { Id = 2, Name = "Marketing" },
            new Department { Id = 3, Name = "Finance" },
            new Department { Id = 4, Name = "HR" });

        modelBuilder.Entity<Person>().HasData(
                new Person { PersonId = Guid.NewGuid(), FirstName = "Jhone", LastName = "Smith", DateOfBirth = DateTime.UtcNow.AddYears(-30), DepartmentId = 4 },
                new Person { PersonId = Guid.NewGuid(), FirstName = "Mark", LastName = "Antony", DateOfBirth = DateTime.UtcNow.AddYears(-25), DepartmentId = 2 },
                new Person { PersonId = Guid.NewGuid(), FirstName = "William", LastName = "Shaks", DateOfBirth = DateTime.UtcNow.AddYears(-36), DepartmentId = 4 },
                new Person { PersonId = Guid.NewGuid(), FirstName = "Alan", LastName = "Banister", DateOfBirth = DateTime.UtcNow.AddYears(-45), DepartmentId = 1 });

    }

    public DbSet<Person> People { get; set; }

    public DbSet<Department> Departments { get; set; }
}