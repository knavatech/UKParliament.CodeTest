using System.ComponentModel.DataAnnotations;

namespace UKParliament.CodeTest.DTO.CustomValidation;

public class PastDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not DateTime dateValue)
        {
            return new ValidationResult("Invalid date format.");
        }

        if (dateValue >= DateTime.UtcNow)
        {
            return new ValidationResult("The date must be in the past.");
        }

        return ValidationResult.Success;
    }
}
