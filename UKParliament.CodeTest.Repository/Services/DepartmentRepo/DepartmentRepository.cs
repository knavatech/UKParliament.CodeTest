using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Repository.Services.DepartmentRepo;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly PersonManagerContext _context;

    public DepartmentRepository(PersonManagerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _context.Departments.ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        return await _context.Departments.FindAsync(id);
    }
}
