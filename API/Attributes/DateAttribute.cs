using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DatabaseLab1.API.Attributes;

public class DateAttribute : ValidationAttribute
{
    private readonly string _dateFormat = "dd.MM.yyyy";

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        var dateString = value as string;

        if (string.IsNullOrWhiteSpace(dateString))
        {
            return new ValidationResult("Date is required.");
        }

        if (!DateTime.TryParseExact(dateString, _dateFormat,
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None, out _))
        {
            return new ValidationResult($"Date must be in the format {_dateFormat}.");
        }

        return ValidationResult.Success!;
    }
}
