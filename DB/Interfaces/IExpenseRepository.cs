using DatabaseLab1.Domain.Entities;

namespace DatabaseLab1.DB.Interfaces;

public interface IExpenseRepository : IRepository<Expense>
{
    Task<IQueryable<Expense>> GetByExpenseTypeId();

    Task<IQueryable<Expense>> GetByDepartmentId();

    Task<IQueryable<Expense>> GetByAmount();

    Task<IQueryable<Expense>> GetByDate();

    Task<IQueryable<Expense>> GetByCodeLength();
}
