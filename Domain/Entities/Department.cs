using System.Data.SqlClient;

namespace DatabaseLab1.Domain.Entities;

public class Department
{
    public long DepartmentId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int EmployeeCount { get; set; }

    public static Department FromReader(SqlDataReader sqlDataReader)
    {
        return new Department
        {
            DepartmentId = long.Parse(sqlDataReader[nameof(DepartmentId)].ToString() ?? string.Empty),
            Name = sqlDataReader[nameof(Name)].ToString() ?? string.Empty,
            EmployeeCount = int.Parse(sqlDataReader[nameof(EmployeeCount)].ToString() ?? string.Empty),
        };
    }
}
