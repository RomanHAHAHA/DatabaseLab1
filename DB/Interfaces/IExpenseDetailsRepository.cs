using DatabaseLab1.Domain.Entities;

namespace DatabaseLab1.DB.Interfaces;

public interface IExpenseDetailsRepository : IRepository<ExpenseDetails>
{
    Task<IEnumerable<ExpenseDetails>> GetApprovedExpensesLastMonth();
}
