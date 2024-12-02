using System.ComponentModel.DataAnnotations;

namespace UKParliament.CodeTest.Data;

public class Department
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<Person> People { get; set; }
}
