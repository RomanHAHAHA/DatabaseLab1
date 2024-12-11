using System.Data.SqlClient;

namespace DatabaseLab1.Domain.Dtos.DepartmentDtos;

public class DepartmentTotalExpenseDto
{
    public long DepartmentId { get; set; }  

    public string DepartmentName { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    public int ApprovedExpensesCount { get; set; }

    public decimal MaxExpenseLimit { get; set; }

    public static DepartmentTotalExpenseDto FromReader(SqlDataReader reader)
    {
        return new DepartmentTotalExpenseDto
        {
            DepartmentId = long.Parse(reader[nameof(DepartmentId)].ToString() ?? string.Empty),
            DepartmentName = reader[nameof(DepartmentName)].ToString() ?? string.Empty,
            TotalAmount = Convert.ToDecimal(reader[nameof(TotalAmount)]),
            ApprovedExpensesCount = Convert.ToInt32(reader[nameof(ApprovedExpensesCount)]),
            MaxExpenseLimit = Convert.ToDecimal(reader[nameof(MaxExpenseLimit)])   
        };
    }
}
