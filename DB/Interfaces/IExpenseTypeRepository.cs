using DatabaseLab1.Domain.Entities;

namespace DatabaseLab1.DB.Interfaces;

public interface IExpenseTypeRepository : IRepository<ExpenseType>
{
    Task<IQueryable<ExpenseType>> GetByDescriptionLettersCount();

    Task<IQueryable<ExpenseType>> GetByLimitAmount();

    Task<IQueryable<ExpenseType>> GetByNameStart();
}
