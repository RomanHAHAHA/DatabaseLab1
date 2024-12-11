using System.Data.SqlClient;

namespace DatabaseLab1.Domain.Entities;

public class ExpenseDetails
{
    public long ExpenseDetailsId { get; set; }

    public string Notes { get; set; } = string.Empty;

    public bool IsApproved { get; set; }

    public DateTime ApprovalDate { get; set; }

    public static ExpenseDetails FromReader(SqlDataReader reader)
    {
        return new ExpenseDetails
        {
            ExpenseDetailsId = long.Parse(reader[nameof(ExpenseDetailsId)].ToString() ?? string.Empty),
            Notes = reader[nameof(Notes)].ToString() ?? string.Empty,
            IsApproved = bool.Parse(reader[nameof(IsApproved)].ToString() ?? string.Empty),
            ApprovalDate = DateTime.Parse(reader[nameof(ApprovalDate)].ToString() ?? string.Empty),
        };
    }
}
