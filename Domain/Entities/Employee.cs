using System.Data.SqlClient;

namespace DatabaseLab1.Domain.Entities;

public class Employee
{
    public long EmployeeId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;

    public long DepartmentId { get; set; }

    public static Employee FromReader(SqlDataReader reader)
    {
        return new Employee
        {
            EmployeeId = long.Parse(reader[nameof(EmployeeId)].ToString() ?? string.Empty),
            FullName = reader[nameof(FullName)].ToString() ?? string.Empty,
            Position = reader[nameof(Position)].ToString() ?? string.Empty,
            DepartmentId = long.Parse(reader[nameof(DepartmentId)].ToString() ?? string.Empty),
        };
    }
}
