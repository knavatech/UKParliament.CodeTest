using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Tests.Helper;

public static class InMemoryDatabase
{
    public static PersonManagerContext GetDbContext(out SqliteConnection connection)
    {
        // Initialize SQLite native components
        Batteries.Init();

        // SQLite In-Memory Connection
        connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<PersonManagerContext>()
            .UseSqlite(connection)
            .Options;

        var context = new PersonManagerContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return context;
    }
}

