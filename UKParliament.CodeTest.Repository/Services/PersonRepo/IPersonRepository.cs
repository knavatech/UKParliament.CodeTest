using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Repository.Services.PersonRepo;

public interface IPersonRepository
{
    Task<IEnumerable<Person>> GetAllAsync();
    Task<Person?> GetByIdAsync(Guid id);
    Task<Department?> GetDepartmentAsync(Guid id);
    Task AddAsync(Person person);
    Task UpdateAsync(Person person);
    Task DeleteAsync(Guid id);
}
