using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services.AppServices;

public interface IDepartmentService
{
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(int id);
}
