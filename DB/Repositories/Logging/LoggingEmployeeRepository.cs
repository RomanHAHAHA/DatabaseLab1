using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Dtos.DepartmentDtos;
using DatabaseLab1.Domain.Dtos.EmployeeDtos;
using DatabaseLab1.Domain.Entities;
using DatabaseLab1.Services.Interfaces;

namespace DatabaseLab1.DB.Repositories.Logging;

public class LoggingEmployeeRepository : IEmployeeRepository
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IReportService _reportService;

    public LoggingEmployeeRepository(
        IEmployeeRepository employeeRepository,
        IReportService reportService)
    {
        _employeeRepository = employeeRepository;
        _reportService = reportService;
    }

    public async Task<bool> CreateAsync(Employee entity)
        => await _employeeRepository.CreateAsync(entity);

    public async Task<IQueryable<Employee>> GetAllAsync()
        => await _employeeRepository.GetAllAsync();

    public async Task<IEnumerable<EmployeeAverageExpenseDto>> GetAverageExpensePerEmployee()
    {
        var employees = await _employeeRepository
            .GetAverageExpensePerEmployee();

        await _reportService.LogToCacheAsync(
            "Отримати назву відділу з середнім значенням витат на співробітника " +
            "сумою витат відділу та кількістю співробітників",
            employees);

        return employees;
    }

    public async Task<Employee?> GetByIdAsync(long id)
        => await _employeeRepository.GetByIdAsync(id);

    public async Task<IEnumerable<DepartmentEmployeeCountDto>> GetDepartmentsEmployeeCountAboveAverage()
    {
        var employees = await _employeeRepository
            .GetDepartmentsEmployeeCountAboveAverage();

        await _reportService.LogToCacheAsync(
            "Отримати назви відділів, з кількістю співробітників, більшою за середню",
            employees);

        return employees;
    }

    public async Task<bool> RemoveAsync(long id)
        => await _employeeRepository.RemoveAsync(id);

    public async Task<bool> UpdateAsync(Employee entity)
        => await _employeeRepository.UpdateAsync(entity);
}
