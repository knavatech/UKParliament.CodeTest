using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Repository.Services.PersonRepo;

namespace UKParliament.CodeTest.Services.AppServices;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _repository;

    public PersonService(IPersonRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Person?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Department?> GetDepartmentAsync(Guid id)
    {
        return await _repository.GetDepartmentAsync(id);
    }

    public async Task AddAsync(Person person)
    {
        await _repository.AddAsync(person);
    }

    public async Task UpdateAsync(Person person)
    {
        await _repository.UpdateAsync(person);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }
}