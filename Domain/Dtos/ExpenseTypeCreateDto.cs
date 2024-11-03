using DatabaseLab1.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLab1.Domain.Dtos;

public class ExpenseTypeCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0, 1000000)]
    public decimal LimitAmount { get; set; }

    public ExpenseType ToEntity()
    {
        return new ExpenseType
        {
            Name = Name,
            Description = Description,
            LimitAmount = LimitAmount,
        };
    }

    public ExpenseType ToEntity(long id)
    {
        var expenseType = ToEntity();
        expenseType.ExpenseTypeId = id;

        return expenseType;
    }
}
