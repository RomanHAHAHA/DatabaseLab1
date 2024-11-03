using DatabaseLab1.API.Options;
using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace DatabaseLab1.DB.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly string _connectionString;

    public DepartmentRepository(IOptions<DbOptions> options)
    {
        _connectionString = options.Value.ConnectionString;
    }

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<bool> CreateAsync(Department entity)
    {
        const string sqlQuery = @"
            INSERT INTO Departments (Name, EmployeeCount)
            VALUES (@Name, @EmployeeCount);
        ";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Name", entity.Name);
        command.Parameters.AddWithValue("@EmployeeCount", entity.EmployeeCount);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<IQueryable<Department>> GetAllAsync()
    {
        const string sqlQuery = "SELECT DepartmentId, Name, EmployeeCount FROM Departments";
        var departments = new List<Department>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            departments.Add(Department.FromReader(reader));
        }

        return departments.AsQueryable();
    }

    public async Task<Department?> GetByIdAsync(long id)
    {
        const string sqlQuery = "SELECT DepartmentId, Name, EmployeeCount FROM Departments WHERE DepartmentId = @Id";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return Department.FromReader(reader);
        }

        return null;
    }

    public async Task<bool> RemoveAsync(long id)
    {
        const string sqlQuery = @"
            BEGIN TRANSACTION;
            DELETE FROM Expenses WHERE DepartmentId = @Id
            DELETE FROM Departments WHERE DepartmentId = @Id;
            COMMIT TRANSACTION;
        ";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Id", id);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(Department entity)
    {
        const string sqlQuery = @"
            UPDATE Departments
            SET Name = @Name, EmployeeCount = @EmployeeCount
            WHERE DepartmentId = @DepartmentId;
        ";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Name", entity.Name);
        command.Parameters.AddWithValue("@EmployeeCount", entity.EmployeeCount);
        command.Parameters.AddWithValue("@DepartmentId", entity.DepartmentId);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<IQueryable<Department>> GetByEmployeeCount()
    {
        const string sqlQuery = "SELECT * " +
            "FROM Departments " +
            "WHERE EmployeeCount > 500";
        var departments = new List<Department>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            departments.Add(Department.FromReader(reader));
        }

        return departments.AsQueryable();
    }

    public async Task<IQueryable<Department>> GetByNameStart()
    {
        const string sqlQuery = "SELECT * " +
            "FROM Departments " +
            "WHERE Name LIKE 'o%'";
        var departments = new List<Department>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            departments.Add(Department.FromReader(reader));
        }

        return departments.AsQueryable();
    }
}
