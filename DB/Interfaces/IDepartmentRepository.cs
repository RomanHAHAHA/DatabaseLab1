using DatabaseLab1.Domain.Entities;

namespace DatabaseLab1.DB.Interfaces;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<IQueryable<Department>> GetByEmployeeCount();

    Task<IQueryable<Department>> GetByNameStart();
}
