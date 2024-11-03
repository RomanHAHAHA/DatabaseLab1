using DatabaseLab1.API.Options;
using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace DatabaseLab1.DB.Repositories;

public class ExpenseTypeRepository : IExpenseTypeRepository
{
    private readonly string _connectionString;

    public ExpenseTypeRepository(IOptions<DbOptions> options)
    {
        _connectionString = options.Value.ConnectionString;
    }

    private SqlConnection CreateConnection() => new(_connectionString);

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

    public async Task<IQueryable<ExpenseType>> GetByDescriptionLettersCount()
    {
        const string sqlQuery = "SELECT * " +
            "FROM ExpenseTypes " +
            "WHERE LEN(Description) > 20";
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

    public async Task<IQueryable<ExpenseType>> GetByLimitAmount()
    {
        const string sqlQuery = "SELECT * " +
            "FROM ExpenseTypes " +
            "WHERE LimitAmount > 200" +
            "ORDER BY LimitAmount";
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

    public async Task<IQueryable<ExpenseType>> GetByNameStart()
    {
        const string sqlQuery = "SELECT * " +
            "FROM ExpenseTypes " +
            "WHERE Name LIKE 'a%'";
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
}
