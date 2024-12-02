using AutoMapper;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.DTO.Person;

namespace UKParliament.CodeTest.Services.MappingProfile;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<Person, PersonDTO>()
             .ForMember(dest => dest.DepartmentName,
                        opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null));

        CreateMap<PersonDTO, Person>()
            .ForMember(dest => dest.Department, opt => opt.Ignore());
    }
}
