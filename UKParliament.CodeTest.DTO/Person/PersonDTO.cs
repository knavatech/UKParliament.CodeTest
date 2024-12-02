using System.ComponentModel.DataAnnotations;
using UKParliament.CodeTest.DTO.CustomValidation;

namespace UKParliament.CodeTest.DTO.Person;

public class PersonDTO
{
    public Guid PersonId { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(250, ErrorMessage = "First name cannot exceed 250 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(250, ErrorMessage = "Last name cannot exceed 250 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required")]
    [DataType(DataType.Date), PastDate]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Department ID is required")]
    public int DepartmentId { get; set; }

    public string? DepartmentName { get; set; }
}
