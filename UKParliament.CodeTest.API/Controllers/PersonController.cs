using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Data;
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var person = await _personService.GetByIdAsync(id);
        return person == null ? NotFound() : Ok(person);
    }

    [HttpGet("{id}/department")]
    public async Task<IActionResult> GetPersonDepartment(Guid id)
    {
        var department = await _personService.GetDepartmentAsync(id);
        return department == null ? NotFound() : Ok(department);
    }


        [HttpPost]
    public async Task<IActionResult> Add(Person person)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _personService.AddAsync(person);
        return CreatedAtAction(nameof(GetById), new { id = person.PersonId }, person);
    }

    [HttpPut]
    public async Task<IActionResult> Update(Person person)
    {
        if (person.PersonId == Guid.Empty) return BadRequest();
        if (!ModelState.IsValid) return BadRequest(ModelState);

        await _personService.UpdateAsync(person);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _personService.DeleteAsync(id);
        return NoContent();
    }
}
