using System.Data.SqlClient;

namespace DatabaseLab1.Domain.Entities;

public class ExpenseType
{
    public long ExpenseTypeId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal LimitAmount { get; set; }

    public static ExpenseType FromReader(SqlDataReader reader)
    {
        return new ExpenseType
        {
            ExpenseTypeId = long.Parse(reader[nameof(ExpenseTypeId)].ToString() ?? string.Empty),
            Name = reader[nameof(Name)].ToString() ?? string.Empty,
            Description = reader[nameof(Description)].ToString() ?? string.Empty,
            LimitAmount = decimal.Parse(reader[nameof(LimitAmount)].ToString() ?? string.Empty),
        };
    }
}
