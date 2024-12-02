using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.DTO.Person;

namespace UKParliament.CodeTest.Repository.Services.PersonRepo;

public class PersonRepository : IPersonRepository
{
    private readonly PersonManagerContext _context;

    public PersonRepository(PersonManagerContext context)
    {
        _context = context;
    }

    //TODO: need to add some filter or paging    
    public async Task<IEnumerable<PersonDTO>> GetAllAsync()
    {
        //Returning DTO directly due to avoid eager loading and circular dependencies
        return await _context.People.Include(d => d.Department).AsNoTrackingWithIdentityResolution()
                                            .Select(p => MapPersonToDTO(p)).ToListAsync();
    }

    public async Task<PersonDTO?> GetByIdAsync(Guid guid)
    {
        var person = await _context.People.Include(d => d.Department).AsNoTrackingWithIdentityResolution()
                                            .FirstOrDefaultAsync(p => p.PersonId.Equals(guid));
        if (person == null)
        {
            return null;
        }

        return MapPersonToDTO(person);
    }

    private static PersonDTO MapPersonToDTO(Person person)
    {
        return new PersonDTO()
        {
            PersonId = person.PersonId,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            DepartmentId = person.DepartmentId,
            DepartmentName = person.Department.Name
        };
    }

    public async Task<Department?> GetDepartmentAsync(Guid guid)
    {
        return await _context.People.Include(d => d.Department).Where(p => p.PersonId.Equals(guid))
            .Select(d => d.Department).SingleOrDefaultAsync();
    }

    public async Task AddAsync(Person person)
    {
        person.PersonId = Guid.NewGuid();

        await _context.People.AddAsync(person);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Person person)
    {
        _context.People.Update(person);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid guid)
    {
        var person = await _context.People.FindAsync(guid);
        if (person != null)
        {
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
        }
    }
}
