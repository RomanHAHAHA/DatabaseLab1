using DatabaseLab1.Domain.Dtos.DepartmentDtos;
using DatabaseLab1.Domain.Dtos.EmployeeDtos;
using DatabaseLab1.Domain.Entities;

namespace DatabaseLab1.DB.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<IEnumerable<EmployeeAverageExpenseDto>> GetAverageExpensePerEmployee();

    Task<IEnumerable<DepartmentEmployeeCountDto>> GetDepartmentsEmployeeCountAboveAverage();
}
