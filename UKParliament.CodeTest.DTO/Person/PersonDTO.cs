using System.ComponentModel.DataAnnotations;

namespace UKParliament.CodeTest.DTO.Person;

public class PersonDTO
{
    public Guid PersonId { get; set; }

    [Required, MaxLength(250)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(250)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    public string? DepartmentName { get; set; }
}
