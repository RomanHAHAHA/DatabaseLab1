using DatabaseLab1.API.Options;
using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Dtos.DepartmentDtos;
using DatabaseLab1.Domain.Dtos.ExpenseDtos;
using DatabaseLab1.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace DatabaseLab1.DB.Repositories.Default;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly string _connectionString;

    public DepartmentRepository(IOptions<DbOptions> options)
    {
        _connectionString = options.Value.ConnectionString;
    }

    private SqlConnection CreateConnection() => new(_connectionString);

    #region CRUD
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
    #endregion 

    public async Task<IEnumerable<DepartmentExpenseDto>> GetExpenseAmountOfEachDepartment()
    {
        const string sqlQuery = @"
            SELECT 
                d.Name AS DepartmentName,
                SUM(e.Amount) AS TotalAmount
            FROM 
                Departments d
            JOIN 
                Expenses e ON d.DepartmentId = e.DepartmentId
            GROUP BY 
                d.Name;
        ";

        var departmentExpenses = new List<DepartmentExpenseDto>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            departmentExpenses.Add(DepartmentExpenseDto.FromReader(reader));
        }

        return departmentExpenses;
    }

    public async Task<IEnumerable<DepartmentEmployeeCountDto>> GetEmployeeCountPerDepartment()
    {
        const string sqlQuery = @"
            SELECT 
                d.Name AS DepartmentName,
                COUNT(emp.EmployeeId) AS EmployeeCount
            FROM 
                Departments d
            LEFT JOIN 
                Employees emp ON d.DepartmentId = emp.DepartmentId
            GROUP BY 
                d.Name;
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

        return departmentEmployeeCounts;
    }

    public async Task<IEnumerable<DepartmentExpenseApprovalDto>> GetApprovedAndNotApprovedExpenses()
    {
        const string sqlQuery = @"
            SELECT 
                d.Name AS DepartmentName,
                SUM(CASE WHEN ed.IsApproved = 1 THEN 1 ELSE 0 END) AS ApprovedExpenses,
                SUM(CASE WHEN ed.IsApproved = 0 THEN 1 ELSE 0 END) AS NotApprovedExpenses
            FROM 
                Departments d
            JOIN 
                Expenses e ON d.DepartmentId = e.DepartmentId
            JOIN 
                ExpenseDetails ed ON e.ExpenseId = ed.ExpenseDetailsId
            GROUP BY 
                d.Name;
        ";

        var departmentExpenseApprovals = new List<DepartmentExpenseApprovalDto>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            departmentExpenseApprovals
                .Add(DepartmentExpenseApprovalDto.FromReader(reader));
        }

        return departmentExpenseApprovals.AsQueryable();
    }

    public async Task<IEnumerable<DepartmentTotalExpenseDto>> GetFilteredTotalExpensesPerDepartment()
    {
        const string sqlQuery = @"
            SELECT 
                d.DepartmentId,
                d.Name AS DepartmentName,
                SUM(e.Amount) AS TotalAmount,
                COUNT(ed.ExpenseDetailsId) AS ApprovedExpensesCount,
                MAX(et.LimitAmount) AS MaxExpenseLimit
            FROM 
                Departments d
            JOIN 
                Expenses e ON d.DepartmentId = e.DepartmentId
            JOIN 
                ExpenseDetails ed ON e.ExpenseId = ed.ExpenseDetailsId
            JOIN 
                ExpenseTypes et ON e.ExpenseTypeId = et.ExpenseTypeId
            WHERE 
                ed.IsApproved = 1
                AND et.LimitAmount > 0
            GROUP BY 
                d.DepartmentId, d.Name
            ORDER BY 
                TotalAmount DESC;
        ";

        var departmentTotalExpenses = new List<DepartmentTotalExpenseDto>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            departmentTotalExpenses
                .Add(DepartmentTotalExpenseDto.FromReader(reader));
        }

        return departmentTotalExpenses;
    }

    public async Task<IEnumerable<DepartmentExpenseDto>> GetDepartmentsWithExpensesAboveThreshold(decimal threshold)
    {
        const string sqlQuery = @"
            SELECT 
                d.Name AS DepartmentName,
                (SELECT SUM(e.Amount) 
                 FROM Expenses e 
                 WHERE e.DepartmentId = d.DepartmentId) AS TotalAmount
            FROM 
                Departments d
            WHERE 
                (SELECT SUM(e.Amount) 
                 FROM Expenses e 
                 WHERE e.DepartmentId = d.DepartmentId) > @Threshold;
        ";

        var departmentExpenses = new List<DepartmentExpenseDto>();

        using var connection = CreateConnection();
        await connection.OpenAsync();

        using var command = new SqlCommand(sqlQuery, connection);
        command.Parameters.AddWithValue("@Threshold", threshold);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            departmentExpenses
                .Add(DepartmentExpenseDto.FromReader(reader));
        }

        return departmentExpenses.AsQueryable();
    }

    public async Task<IEnumerable<Department>> GetDepartmentsAboveAverageEmployees()
    {
        const string sqlQuery = @"
            SELECT 
                d.DepartmentId,
                d.Name,
                d.EmployeeCount
            FROM 
                Departments d
            WHERE 
                d.EmployeeCount > 
                (SELECT AVG(d2.EmployeeCount) 
                 FROM Departments d2);
        ";

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
