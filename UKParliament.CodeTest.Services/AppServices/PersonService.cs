using AutoMapper;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.DTO.Department;
using UKParliament.CodeTest.DTO.Person;
using UKParliament.CodeTest.Repository.Services.PersonRepo;

namespace UKParliament.CodeTest.Services.AppServices;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _repository;
    private readonly IMapper _mapper;

    public PersonService(IPersonRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PersonDTO>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<PersonDTO?> GetByIdAsync(Guid guid)
    {
        return await _repository.GetByIdAsync(guid);
    }

    public async Task<DepartmentDTO?> GetDepartmentAsync(Guid guid)
    {
        var department = await _repository.GetDepartmentAsync(guid);

        if (department == null)
        {
            return null;
        }

        return _mapper.Map<DepartmentDTO>(department);
    }

    public async Task AddAsync(PersonDTO person)
    {
        var addPerson = _mapper.Map<Person>(person);
        await _repository.AddAsync(addPerson);
    }

    public async Task UpdateAsync(PersonDTO person)
    {
        var updatePerson = _mapper.Map<Person>(person);
        await _repository.UpdateAsync(updatePerson);
    }

    public async Task DeleteAsync(Guid guid)
    {
        await _repository.DeleteAsync(guid);
    }
}