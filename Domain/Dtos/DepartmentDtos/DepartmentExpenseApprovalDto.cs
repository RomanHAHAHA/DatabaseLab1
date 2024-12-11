using System.Data.SqlClient;

namespace DatabaseLab1.Domain.Dtos.DepartmentDtos;

public class DepartmentExpenseApprovalDto
{
    public string DepartmentName { get; set; } = string.Empty;

    public int ApprovedExpenses { get; set; } 

    public int NotApprovedExpenses { get; set; }

    public static DepartmentExpenseApprovalDto FromReader(SqlDataReader reader)
    {
        return new DepartmentExpenseApprovalDto
        {
            DepartmentName = reader[nameof(DepartmentName)].ToString() ?? string.Empty,
            ApprovedExpenses = Convert.ToInt32(reader[nameof(ApprovedExpenses)]),
            NotApprovedExpenses = Convert.ToInt32(reader[nameof(NotApprovedExpenses)])
        };
    }
}
