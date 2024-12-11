using DatabaseLab1.API.Options;
using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Dtos.ExpenseTypeDtos;
using DatabaseLab1.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace DatabaseLab1.DB.Repositories.Default;

public class ExpenseTypeRepository : IExpenseTypeRepository
{
    private readonly string _connectionString;

    public ExpenseTypeRepository(IOptions<DbOptions> options)
    {
        _connectionString = options.Value.ConnectionString;
    }

    private SqlConnection CreateConnection() => new(_connectionString);

    #region CRUD
    public async Task<bool> CreateAsync(ExpenseType entity)
    {
        const string sqlQuery = @"
            INSERT INTO ExpenseTypes (Name, Description, LimitAmount)
            VALUES (@Name, @Description, @LimitAmount);
        ";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Name", entity.Name);
        command.Parameters.AddWithValue("@Description", entity.Description);
        command.Parameters.AddWithValue("@LimitAmount", entity.LimitAmount);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<IQueryable<ExpenseType>> GetAllAsync()
    {
        const string sqlQuery = "SELECT ExpenseTypeId, Name, Description, LimitAmount FROM ExpenseTypes";
        var expenseTypes = new List<ExpenseType>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            expenseTypes.Add(ExpenseType.FromReader(reader));
        }

        return expenseTypes.AsQueryable();
    }

    public async Task<ExpenseType?> GetByIdAsync(long id)
    {
        const string sqlQuery = "SELECT ExpenseTypeId, Name, Description, LimitAmount FROM ExpenseTypes WHERE ExpenseTypeId = @Id";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return ExpenseType.FromReader(reader);
        }

        return null;
    }

    public async Task<bool> RemoveAsync(long id)
    {
        const string sqlQuery = @"
            BEGIN TRANSACTION;
            DELETE FROM Expenses WHERE ExpenseTypeId = @Id;
            DELETE FROM ExpenseTypes WHERE ExpenseTypeId = @Id;
            COMMIT TRANSACTION;
        ";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Id", id);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(ExpenseType entity)
    {
        const string sqlQuery = @"
            UPDATE ExpenseTypes
            SET Name = @Name, Description = @Description, LimitAmount = @LimitAmount
            WHERE ExpenseTypeId = @ExpenseTypeId;
        ";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Name", entity.Name);
        command.Parameters.AddWithValue("@Description", entity.Description);
        command.Parameters.AddWithValue("@LimitAmount", entity.LimitAmount);
        command.Parameters.AddWithValue("@ExpenseTypeId", entity.ExpenseTypeId);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }
    #endregion

    public async Task<IEnumerable<ExpenseTypeCountDto>> GetAverageLimitPerExpenseType()
    {
        const string sqlQuery = @"
            SELECT 
                et.Name AS ExpenseType,
                COUNT(e.ExpenseId) AS TotalExpenses
            FROM 
                ExpenseTypes et
            LEFT JOIN 
                Expenses e ON et.ExpenseTypeId = e.ExpenseTypeId
            GROUP BY 
                et.Name
            ORDER BY 
                TotalExpenses DESC; 
        ";

        var expenseTypeAverages = new List<ExpenseTypeCountDto>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            expenseTypeAverages
                .Add(ExpenseTypeCountDto.FromReader(reader));
        }

        return expenseTypeAverages.AsQueryable();
    }

    public async Task<IEnumerable<ExpenseTypeMaxDto>> GetMaxApprovedExpensesPerType()
    {
        const string sqlQuery = @"
            SELECT 
                et.Name AS ExpenseType,
                MAX(e.Amount) AS MaxApprovedAmount,
                ed.IsApproved AS IsApproved
            FROM 
                Expenses e
            JOIN 
                ExpenseDetails ed ON e.ExpenseId = ed.ExpenseDetailsId
            JOIN 
                ExpenseTypes et ON e.ExpenseTypeId = et.ExpenseTypeId
            WHERE 
                ed.IsApproved = 1
            GROUP BY 
                et.Name, ed.IsApproved;
        ";

        var expenseTypeMaxes = new List<ExpenseTypeMaxDto>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            expenseTypeMaxes.Add(ExpenseTypeMaxDto.FromReader(reader));
        }

        return expenseTypeMaxes.AsQueryable();
    }

    public async Task<IEnumerable<ExpenseType>> GetUnusedExpenseTypesInDepartment(long departmentId)
    {
        const string sqlQuery = @"
            SELECT 
                et.ExpenseTypeId,
                et.Name,
                et.Description,
                et.LimitAmount
            FROM 
                ExpenseTypes et
            WHERE 
                NOT EXISTS (
                    SELECT 1 
                    FROM Expenses e 
                    WHERE e.ExpenseTypeId = et.ExpenseTypeId 
                    AND e.DepartmentId = @DepartmentId
                );
        ";

        var expenseTypes = new List<ExpenseType>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@DepartmentId", departmentId);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            expenseTypes.Add(ExpenseType.FromReader(reader));
        }

        return expenseTypes.AsQueryable();
    }
}
