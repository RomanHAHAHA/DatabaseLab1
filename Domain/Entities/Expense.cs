using System.Data.SqlClient;

namespace DatabaseLab1.Domain.Entities;

public class Expense
{
    public long ExpenseId { get; set; }

    public string Code { get; set; } = string.Empty;

    public long ExpenseTypeId { get; set; }

    public long DepartmentId { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public static Expense FromReader(SqlDataReader reader)
    {
        return new Expense
        {
            ExpenseId = long.Parse(reader[nameof(ExpenseId)].ToString() ?? string.Empty),
            Code = reader[nameof(Code)].ToString() ?? string.Empty,
            ExpenseTypeId = long.Parse(reader[nameof(ExpenseTypeId)].ToString() ?? string.Empty),
            DepartmentId = long.Parse(reader[nameof(DepartmentId)].ToString() ?? string.Empty),
            Amount = decimal.Parse(reader[nameof(Amount)].ToString() ?? string.Empty),
            Date = DateTime.Parse(reader[nameof(Date)].ToString() ?? string.Empty),
        };
    }
}

