using AutoMapper;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.DTO.Department;

namespace UKParliament.CodeTest.Services.MappingProfile;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<Department, DepartmentDTO>().ReverseMap().ForMember(dest => dest.People, opt => opt.Ignore());
    }
}
