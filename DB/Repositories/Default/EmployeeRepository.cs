using DatabaseLab1.API.Options;
using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Dtos.DepartmentDtos;
using DatabaseLab1.Domain.Dtos.EmployeeDtos;
using DatabaseLab1.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace DatabaseLab1.DB.Repositories.Default;

public class EmployeesRepository : IEmployeeRepository
{
    private readonly string _connectionString;

    public EmployeesRepository(IOptions<DbOptions> options)
    {
        _connectionString = options.Value.ConnectionString;
    }

    private SqlConnection CreateConnection() => new(_connectionString);

    #region CRUD
    public async Task<bool> CreateAsync(Employee entity)
    {
        const string sqlQuery = @"
            INSERT INTO Employees (FullName, Position, DepartmentId)
            VALUES (@FullName, @Position, @DepartmentId);
        ";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@FullName", entity.FullName);
        command.Parameters.AddWithValue("@Position", entity.Position);
        command.Parameters.AddWithValue("@DepartmentId", entity.DepartmentId);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<IQueryable<Employee>> GetAllAsync()
    {
        const string sqlQuery = "SELECT EmployeeId, FullName, Position, DepartmentId FROM Employees";
        var employees = new List<Employee>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            employees.Add(Employee.FromReader(reader));
        }

        return employees.AsQueryable();
    }

    public async Task<Employee?> GetByIdAsync(long id)
    {
        const string sqlQuery = "SELECT EmployeeId, FullName, Position, DepartmentId FROM Employees WHERE EmployeeId = @Id";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return Employee.FromReader(reader);
        }

        return null;
    }

    public async Task<bool> RemoveAsync(long id)
    {
        const string sqlQuery = "DELETE FROM Employees WHERE EmployeeId = @Id";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Id", id);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(Employee entity)
    {
        const string sqlQuery = @"
            UPDATE Employees
            SET FullName = @FullName, Position = @Position, DepartmentId = @DepartmentId
            WHERE EmployeeId = @EmployeeId;
        ";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@FullName", entity.FullName);
        command.Parameters.AddWithValue("@Position", entity.Position ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@DepartmentId", entity.DepartmentId);
        command.Parameters.AddWithValue("@EmployeeId", entity.EmployeeId);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
    #endregion

    public async Task<IEnumerable<EmployeeAverageExpenseDto>> GetAverageExpensePerEmployee()
    {
        const string sqlQuery = @"
            SELECT 
                d.Name AS DepartmentName,
                AVG(e.Amount) AS AverageExpensePerEmployee,
                SUM(e.Amount) AS TotalDepartmentExpense,
                (SELECT COUNT(emp.EmployeeId) 
                 FROM Employees emp 
                 WHERE emp.DepartmentId = d.DepartmentId) AS EmployeeCount
            FROM 
                Departments d
            JOIN 
                Employees emp ON d.DepartmentId = emp.DepartmentId
            JOIN 
                Expenses e ON emp.DepartmentId = e.DepartmentId
            GROUP BY 
                d.Name, d.DepartmentId;
        ";

        var departmentAverageExpenses = new List<EmployeeAverageExpenseDto>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            departmentAverageExpenses
                .Add(EmployeeAverageExpenseDto.FromReader(reader));
        }

        return departmentAverageExpenses;
    }
    
    public async Task<IEnumerable<DepartmentEmployeeCountDto>> GetDepartmentsEmployeeCountAboveAverage()
    {
        const string sqlQuery = @"
            SELECT 
                d.Name AS DepartmentName,
                (SELECT COUNT(e.EmployeeId) 
                 FROM Employees e 
                 WHERE e.DepartmentId = d.DepartmentId) AS EmployeeCount
            FROM 
                Departments d
            WHERE 
                (SELECT COUNT(e.EmployeeId) 
                 FROM Employees e 
                 WHERE e.DepartmentId = d.DepartmentId) > 
                (SELECT AVG(DepartmentEmployeeCount) 
                 FROM (SELECT COUNT(emp.EmployeeId) AS DepartmentEmployeeCount
                       FROM Departments d2
                       LEFT JOIN Employees emp ON d2.DepartmentId = emp.DepartmentId
                       GROUP BY d2.DepartmentId) AS Subquery);
        ";

        var departmentEmployeeCounts = new List<DepartmentEmployeeCountDto>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            departmentEmployeeCounts
                .Add(DepartmentEmployeeCountDto.FromReader(reader));
        }

        return departmentEmployeeCounts.AsQueryable();
    }
}
