using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Repository.Services.DepartmentRepo;
using UKParliament.CodeTest.Repository.Services.PersonRepo;
using UKParliament.CodeTest.Services.AppServices;
using UKParliament.CodeTest.Services.MappingProfile;

namespace UKParliament.CodeTest.API;

public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddDbContext<PersonManagerContext>(options => options.UseInMemoryDatabase("PersonManagerDb"));
        services.AddAutoMapper(typeof(PersonProfile).Assembly);

        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();

        return services;
    }
}
