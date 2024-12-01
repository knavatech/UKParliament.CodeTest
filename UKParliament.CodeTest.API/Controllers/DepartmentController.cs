using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Services.AppServices;

namespace UKParliament.CodeTest.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var persons = await _departmentService.GetAllAsync();
        return Ok(persons);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var person = await _departmentService.GetByIdAsync(id);
        return person == null ? NotFound() : Ok(person);
    }

    
}
