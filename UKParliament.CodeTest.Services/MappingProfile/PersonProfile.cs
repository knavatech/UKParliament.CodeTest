using AutoMapper;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.DTO.Person;

namespace UKParliament.CodeTest.Services.MappingProfile;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<Person, PersonDTO>().ReverseMap();
    }
}
