using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Repository.Services.PersonRepo;

public class PersonRepository : IPersonRepository
{
    private readonly PersonManagerContext _context;

    public PersonRepository(PersonManagerContext context)
    {
        _context = context;
    }

    //TODO: need to add some filter or paging
    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return  await _context.People.ToListAsync();
    }

    public async Task<Person?> GetByIdAsync(Guid id)
    {
        return await _context.People.FirstOrDefaultAsync(p => p.PersonId == id);
    }

    public async Task<Department?> GetDepartmentAsync(Guid id)
    {
        return await _context.People.Include(d=>d.Department).Where(p => p.PersonId == id)
            .Select(d=>d.Department).SingleOrDefaultAsync();
    }

    public async Task AddAsync(Person person)
    {
        await _context.People.AddAsync(person);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Person person)
    {
        _context.People.Update(person);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var person = await _context.People.FindAsync(id);
        if (person != null)
        {
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
        }
    }
}
