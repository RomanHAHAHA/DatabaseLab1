using System.Data.SqlClient;

namespace DatabaseLab1.Domain.Dtos.ExpenseTypeDtos;

public class ExpenseTypeMaxDto
{
    public string ExpenseType { get; set; } = string.Empty;

    public decimal MaxApprovedAmount { get; set; }

    public string IsApproved { get; set; } = string.Empty;

    public static ExpenseTypeMaxDto FromReader(SqlDataReader reader)
    {
        return new ExpenseTypeMaxDto
        {
            ExpenseType = reader[nameof(ExpenseType)].ToString() ?? string.Empty,
            MaxApprovedAmount = Convert.ToDecimal(reader[nameof(MaxApprovedAmount)]),
            IsApproved = Convert.ToBoolean(reader[nameof(IsApproved)]) ? "Yes" : "No",
        };
    }
}
