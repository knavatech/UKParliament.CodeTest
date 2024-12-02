using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.DTO.Person;

namespace UKParliament.CodeTest.Repository.Services.PersonRepo;

public interface IPersonRepository
{
    Task<IEnumerable<PersonDTO>> GetAllAsync();
    Task<PersonDTO?> GetByIdAsync(Guid guid);
    Task<Department?> GetDepartmentAsync(Guid guid);
    Task AddAsync(Person person);
    Task UpdateAsync(Person person);
    Task DeleteAsync(Guid guid);
}
