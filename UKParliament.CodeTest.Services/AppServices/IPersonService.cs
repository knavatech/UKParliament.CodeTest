
using UKParliament.CodeTest.DTO.Department;
using UKParliament.CodeTest.DTO.Person;

namespace UKParliament.CodeTest.Services.AppServices;

public interface IPersonService
{
    Task<IEnumerable<PersonDTO>> GetAllAsync();
    Task<PersonDTO?> GetByIdAsync(Guid guid);
    Task<DepartmentDTO?> GetDepartmentAsync(Guid guid);
    Task AddAsync(PersonDTO person);
    Task UpdateAsync(PersonDTO person);
    Task DeleteAsync(Guid guid);
}