using DatabaseLab1.API.Attributes;
using DatabaseLab1.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DatabaseLab1.Domain.Dtos.ExpenseDetailsDtos;

public class ExpenseDetailsCreateDto
{
    [Required]
    [Range(0, int.MaxValue)]
    public long ExpenseDetailsId { get; set; }

    [Required]
    [MinLength(5)]
    [MaxLength(500)]
    public string Notes { get; set; } = string.Empty;

    [Required]
    public bool IsApproved { get; set; }

    [Required]
    [Date]
    public string ApprovalDate { get; set; } = string.Empty;

    public ExpenseDetails ToEntity()
    {
        return new ExpenseDetails
        {
            ExpenseDetailsId = ExpenseDetailsId,
            Notes = Notes,
            IsApproved = IsApproved,
            ApprovalDate = DateTime.Parse(ApprovalDate),
        };
    }

    public ExpenseDetails ToEntity(long id)
    {
        var expenseDetails = ToEntity();
        expenseDetails.ExpenseDetailsId = id;

        return expenseDetails;
    }
}
