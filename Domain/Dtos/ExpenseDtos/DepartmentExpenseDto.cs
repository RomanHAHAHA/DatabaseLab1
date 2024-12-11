using System.Data.SqlClient;

namespace DatabaseLab1.Domain.Dtos.ExpenseDtos;

public class DepartmentExpenseDto
{
    public string DepartmentName { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    public static DepartmentExpenseDto FromReader(SqlDataReader reader)
    {
        return new DepartmentExpenseDto
        {
            DepartmentName = reader[nameof(DepartmentName)].ToString() ?? string.Empty,
            TotalAmount = Convert.ToDecimal(reader[nameof(TotalAmount)])
        };
    }
}
