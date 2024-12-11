using System.Data.SqlClient;

namespace DatabaseLab1.Domain.Dtos.ExpenseTypeDtos;

public class ExpenseTypeCountDto
{
    public string ExpenseType { get; set; } = string.Empty;

    public int TotalExpenses { get; set; }

    public static ExpenseTypeCountDto FromReader(SqlDataReader reader)
    {
        return new ExpenseTypeCountDto
        {
            ExpenseType = reader[nameof(ExpenseType)].ToString() ?? string.Empty,
            TotalExpenses = Convert.ToInt32(reader[nameof(TotalExpenses)])
        };
    }
}
