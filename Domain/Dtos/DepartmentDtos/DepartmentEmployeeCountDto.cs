using System.Data.SqlClient;

namespace DatabaseLab1.Domain.Dtos.DepartmentDtos;

public class DepartmentEmployeeCountDto
{
    public string DepartmentName { get; set; } = string.Empty;

    public int EmployeeCount { get; set; }  

    public static DepartmentEmployeeCountDto FromReader(SqlDataReader reader)
    {
        return new DepartmentEmployeeCountDto
        {
            DepartmentName = reader[nameof(DepartmentName)].ToString() ?? string.Empty,
            EmployeeCount = Convert.ToInt32(reader[nameof(EmployeeCount)])
        };
    }
}
