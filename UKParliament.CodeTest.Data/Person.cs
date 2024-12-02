using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UKParliament.CodeTest.Data;

public class Person
{
    [Key]
    public Guid PersonId { get; set; }

    [Required, MaxLength(250)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(250)]
    public string LastName { get; set; } =  string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    [ForeignKey("DepartmentId")]
    public Department? Department { get; set; }
}