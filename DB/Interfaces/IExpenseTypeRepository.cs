using DatabaseLab1.Domain.Dtos.ExpenseTypeDtos;
using DatabaseLab1.Domain.Entities;

namespace DatabaseLab1.DB.Interfaces;

public interface IExpenseTypeRepository : IRepository<ExpenseType>
{
    Task<IEnumerable<ExpenseTypeCountDto>> GetAverageLimitPerExpenseType();

    Task<IEnumerable<ExpenseTypeMaxDto>> GetMaxApprovedExpensesPerType();

    Task<IEnumerable<ExpenseType>> GetUnusedExpenseTypesInDepartment(long departmentId);
}
