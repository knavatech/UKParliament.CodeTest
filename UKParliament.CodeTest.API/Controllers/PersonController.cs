using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.DTO.Person;
using UKParliament.CodeTest.Services.AppServices;

namespace UKParliament.CodeTest.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var persons = await _personService.GetAllAsync();
        return Ok(persons);
    }

    [HttpGet("{guid}")]
    public async Task<IActionResult> GetById(Guid guid)
    {
        var person = await _personService.GetByIdAsync(guid);
        return person == null ? NotFound() : Ok(person);
    }

    [HttpGet("{guid}/department")]
    public async Task<IActionResult> GetPersonDepartment(Guid guid)
    {
        var department = await _personService.GetDepartmentAsync(guid);
        return department == null ? NotFound() : Ok(department);
    }


    [HttpPost]
    public async Task<IActionResult> Add(PersonDTO person)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _personService.AddAsync(person);
        return CreatedAtAction(nameof(GetById), new { guid = person.PersonId }, person);
    }

    [HttpPut]
    public async Task<IActionResult> Update(PersonDTO person)
    {
        if (person.PersonId == Guid.Empty) return BadRequest();
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _personService.UpdateAsync(person);
        return Ok();
    }

    [HttpDelete("{guid}")]
    public async Task<IActionResult> Delete(Guid guid)
    {
        await _personService.DeleteAsync(guid);
        return Ok();
    }
}
