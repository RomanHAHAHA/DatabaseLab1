using System.Data.SqlClient;

namespace DatabaseLab1.Domain.Dtos.EmployeeDtos;

public class EmployeeAverageExpenseDto
{
    public string DepartmentName { get; set; } = string.Empty;

    public decimal AverageExpensePerEmployee { get; set; }

    public decimal TotalDepartmentExpense { get; set; }

    public int EmployeeCount { get; set; }  

    public static EmployeeAverageExpenseDto FromReader(SqlDataReader reader)
    {
        return new EmployeeAverageExpenseDto
        {
            DepartmentName = reader[nameof(DepartmentName)].ToString() ?? string.Empty,
            AverageExpensePerEmployee = Convert.ToDecimal(reader[nameof(AverageExpensePerEmployee)]),
            TotalDepartmentExpense = Convert.ToDecimal(reader[nameof(TotalDepartmentExpense)]),
            EmployeeCount = Convert.ToInt32(reader[nameof(EmployeeCount)]),
        };
    }
}
