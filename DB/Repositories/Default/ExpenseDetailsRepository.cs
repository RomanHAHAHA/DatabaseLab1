using DatabaseLab1.API.Options;
using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace DatabaseLab1.DB.Repositories.Default;

public class ExpenseDetailsRepository : IExpenseDetailsRepository
{
    private readonly string _connectionString;

    public ExpenseDetailsRepository(IOptions<DbOptions> options)
    {
        _connectionString = options.Value.ConnectionString;
    }

    private SqlConnection CreateConnection() => new(_connectionString);

    #region CRUD
    public async Task<bool> CreateAsync(ExpenseDetails entity)
    {
        const string sqlQuery = @"
            INSERT INTO ExpenseDetails (ExpenseDetailsId, Notes, IsApproved, ApprovalDate)
            VALUES (@ExpenseDetailsId, @Notes, @IsApproved, @ApprovalDate);
        ";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@ExpenseDetailsId", entity.ExpenseDetailsId);
        command.Parameters.AddWithValue("@Notes", (object?)entity.Notes ?? DBNull.Value);
        command.Parameters.AddWithValue("@IsApproved", entity.IsApproved);
        command.Parameters.AddWithValue("@ApprovalDate", (object?)entity.ApprovalDate ?? DBNull.Value);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<ExpenseDetails?> GetByIdAsync(long id)
    {
        const string sqlQuery = @"
            SELECT ExpenseDetailsId, Notes, IsApproved, ApprovalDate 
            FROM ExpenseDetails 
            WHERE ExpenseDetailsId = @Id;
        ";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return ExpenseDetails.FromReader(reader);
        }

        return null;
    }

    public async Task<bool> UpdateAsync(ExpenseDetails entity)
    {
        const string sqlQuery = @"
            UPDATE ExpenseDetails
            SET Notes = @Notes, IsApproved = @IsApproved, ApprovalDate = @ApprovalDate
            WHERE ExpenseDetailsId = @ExpenseDetailsId;
        ";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Notes", (object?)entity.Notes ?? DBNull.Value);
        command.Parameters.AddWithValue("@IsApproved", entity.IsApproved);
        command.Parameters.AddWithValue("@ApprovalDate", (object?)entity.ApprovalDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@ExpenseDetailsId", entity.ExpenseDetailsId);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> RemoveAsync(long id)
    {
        const string sqlQuery = "DELETE FROM ExpenseDetails WHERE ExpenseDetailsId = @Id";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Id", id);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<IQueryable<ExpenseDetails>> GetAllAsync()
    {
        const string sqlQuery = "SELECT ExpenseDetailsId, Notes, IsApproved, ApprovalDate FROM ExpenseDetails";
        var expenseDetailsList = new List<ExpenseDetails>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            expenseDetailsList.Add(ExpenseDetails.FromReader(reader));
        }

        return expenseDetailsList.AsQueryable();
    }
    #endregion

    public async Task<IEnumerable<ExpenseDetails>> GetApprovedExpensesLastMonth()
    {
        const string sqlQuery = @"
            SELECT 
                ed.ExpenseDetailsId,
                ed.Notes,
                ed.IsApproved,
                ed.ApprovalDate
            FROM 
                ExpenseDetails ed
            WHERE 
                ed.IsApproved = 1
                AND ed.ApprovalDate > 
                (SELECT DATEADD(MONTH, -1, GETDATE()));
        ";

        var expenseDetails = new List<ExpenseDetails>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            expenseDetails.Add(ExpenseDetails.FromReader(reader));
        }

        return expenseDetails;
    }

}
