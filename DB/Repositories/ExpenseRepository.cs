﻿using DatabaseLab1.API.Options;
using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace DatabaseLab1.DB.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly string _connectionString;

    public ExpenseRepository(IOptions<DbOptions> options)
    {
        _connectionString = options.Value.ConnectionString;
    }

    private SqlConnection CreateConnection() => new(_connectionString);

    public async Task<bool> CreateAsync(Expense entity)
    {
        const string sqlQuery = @"
            INSERT INTO Expenses (Code, ExpenseTypeId, DepartmentId, Amount, Date)
            VALUES (@Code, @ExpenseTypeId, @DepartmentId, @Amount, @Date);
        ";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Code", entity.Code);
        command.Parameters.AddWithValue("@ExpenseTypeId", entity.ExpenseTypeId);
        command.Parameters.AddWithValue("@DepartmentId", entity.DepartmentId);
        command.Parameters.AddWithValue("@Amount", entity.Amount);
        command.Parameters.AddWithValue("@Date", entity.Date);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<IQueryable<Expense>> GetAllAsync()
    {
        const string sqlQuery = "SELECT ExpenseId, Code, ExpenseTypeId, DepartmentId, Amount, Date FROM Expenses";
        var expenses = new List<Expense>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            expenses.Add(Expense.FromReader(reader));
        }

        return expenses.AsQueryable();
    }

    public async Task<Expense?> GetByIdAsync(long id)
    {
        const string sqlQuery = "SELECT ExpenseId, Code, ExpenseTypeId, DepartmentId, Amount, Date FROM Expenses WHERE ExpenseId = @Id";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return Expense.FromReader(reader);
        }

        return null;
    }

    public async Task<bool> RemoveAsync(long id)
    {
        const string sqlQuery = @"
            BEGIN TRANSACTION;
            DELETE FROM Expenses WHERE ExpenseId = @Id;
            COMMIT TRANSACTION;
        ";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Id", id);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(Expense entity)
    {
        const string sqlQuery = @"
            UPDATE Expenses
            SET Code = @Code, ExpenseTypeId = @ExpenseTypeId, DepartmentId = @DepartmentId, Amount = @Amount, Date = @Date
            WHERE ExpenseId = @ExpenseId;
        ";

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Code", entity.Code);
        command.Parameters.AddWithValue("@ExpenseTypeId", entity.ExpenseTypeId);
        command.Parameters.AddWithValue("@DepartmentId", entity.DepartmentId);
        command.Parameters.AddWithValue("@Amount", entity.Amount);
        command.Parameters.AddWithValue("@Date", entity.Date);
        command.Parameters.AddWithValue("@ExpenseId", entity.ExpenseId);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<IQueryable<Expense>> GetByExpenseTypeId()
    {
        const string sqlQuery = "SELECT * " +
            "FROM Expenses " +
            "WHERE ExpenseTypeId = 7";
        var expenses = new List<Expense>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            expenses.Add(Expense.FromReader(reader));
        }

        return expenses.AsQueryable();
    }

    public async Task<IQueryable<Expense>> GetByDepartmentId()
    {
        const string sqlQuery = "SELECT * " +
            "FROM Expenses " +
            "WHERE DepartmentId = 8";
        var expenses = new List<Expense>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            expenses.Add(Expense.FromReader(reader));
        }

        return expenses.AsQueryable();
    }

    public async Task<IQueryable<Expense>> GetByAmount()
    {
        const string sqlQuery = "SELECT * " +
            "FROM Expenses " +
            "WHERE Amount >= 500";
        var expenses = new List<Expense>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            expenses.Add(Expense.FromReader(reader));
        }

        return expenses.AsQueryable();
    }

    public async Task<IQueryable<Expense>> GetByDate()
    {
        const string sqlQuery = "SELECT * " +
            "FROM Expenses " +
            "WHERE Date >= '2023-01-01'";
        var expenses = new List<Expense>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            expenses.Add(Expense.FromReader(reader));
        }

        return expenses.AsQueryable();
    }

    public async Task<IQueryable<Expense>> GetByCodeLength()
    {
        const string sqlQuery = "SELECT * " +
            "FROM Expenses " +
            "WHERE LEN(CODE) >= 10";
        var expenses = new List<Expense>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            expenses.Add(Expense.FromReader(reader));
        }

        return expenses.AsQueryable();
    }
}