using DatabaseLab1.Domain.Dtos.DepartmentDtos;
using DatabaseLab1.Domain.Dtos.ExpenseDtos;
using DatabaseLab1.Domain.Entities;

namespace DatabaseLab1.DB.Interfaces;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<IEnumerable<DepartmentExpenseDto>> GetExpenseAmountOfEachDepartment();

    Task<IEnumerable<DepartmentEmployeeCountDto>> GetEmployeeCountPerDepartment();

    Task<IEnumerable<DepartmentExpenseApprovalDto>> GetApprovedAndNotApprovedExpenses();

    Task<IEnumerable<DepartmentTotalExpenseDto>> GetFilteredTotalExpensesPerDepartment();

    Task<IEnumerable<DepartmentExpenseDto>> GetDepartmentsWithExpensesAboveThreshold(decimal threshold);

    Task<IEnumerable<Department>> GetDepartmentsAboveAverageEmployees();
}
