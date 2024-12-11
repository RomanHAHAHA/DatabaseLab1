using DatabaseLab1.Domain.Entities;

namespace DatabaseLab1.DB.Interfaces;

public interface IExpenseRepository : IRepository<Expense>
{
    Task<IEnumerable<Expense>> GetExpensesExceedingTypeLimit();

    Task<IEnumerable<Expense>> GetExpensesAboveAverageForType(long expenseTypeId);
}
