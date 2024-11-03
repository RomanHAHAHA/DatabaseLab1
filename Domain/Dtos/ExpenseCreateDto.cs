using DatabaseLab1.API.Attributes;
using DatabaseLab1.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLab1.Domain.Dtos;

public class ExpenseCreateDto
{
    [Required]
    public string ExpenseCode { get; set; } = string.Empty;

    [Required]
    [Range(0, 1000000)]
    public long ExpenseTypeId { get; set; }

    [Required]
    [Range(0, 1000000)]
    public long DepartmentId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    [Date]
    public string Date { get; set; } = string.Empty;

    public Expense ToEntity()
    {
        return new Expense
        {
            Code = ExpenseCode,
            DepartmentId = DepartmentId,
            ExpenseTypeId = ExpenseTypeId,
            Amount = Amount,
            Date = DateTime.Parse(Date)
        };
    }

    public Expense ToEntity(long id)
    {
        var expense = ToEntity();
        expense.ExpenseId = id;

        return expense;
    }
}
