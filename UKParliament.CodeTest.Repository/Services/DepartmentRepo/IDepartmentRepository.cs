using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Repository.Services.DepartmentRepo;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(int id);
}
