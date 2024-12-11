using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Dtos.DepartmentDtos;
using DatabaseLab1.Domain.Dtos.ExpenseDtos;
using DatabaseLab1.Domain.Entities;
using DatabaseLab1.Services.Interfaces;

namespace DatabaseLab1.DB.Repositories.Logging;

public class LoggingDepartmentRepository : IDepartmentRepository
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IReportService _reportService;

    public LoggingDepartmentRepository(
        IDepartmentRepository departmentRepository,
        IReportService reportService)
    {
        _departmentRepository = departmentRepository;
        _reportService = reportService;
    }

    public async Task<bool> CreateAsync(Department entity)
        => await _departmentRepository.CreateAsync(entity);

    public async Task<IQueryable<Department>> GetAllAsync()
        => await _departmentRepository.GetAllAsync();

    public async Task<IEnumerable<DepartmentExpenseApprovalDto>> GetApprovedAndNotApprovedExpenses()
    {
        var expenses = await _departmentRepository
            .GetApprovedAndNotApprovedExpenses();

        await _reportService.LogToCacheAsync(
            "Отримати кількість одобрених і неободрених витрат кожного відділу",
            expenses);

        return expenses;
    }

    public async Task<Department?> GetByIdAsync(long id)
        => await _departmentRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Department>> GetDepartmentsAboveAverageEmployees()
    {
        var expenses = await _departmentRepository
            .GetDepartmentsAboveAverageEmployees();

        await _reportService.LogToCacheAsync(
            "Отримати відділи кількість співробітників яких більше середнього",
            expenses);

        return expenses;
    }

    public async Task<IEnumerable<DepartmentExpenseDto>> GetDepartmentsWithExpensesAboveThreshold(decimal threshold)
    {
        var expenses = await _departmentRepository
            .GetDepartmentsWithExpensesAboveThreshold(threshold);

        await _reportService.LogToCacheAsync(
            "Отримати відділи сума витрат яких більше за поріг",
            expenses);

        return expenses;
    }

    public async Task<IEnumerable<DepartmentEmployeeCountDto>> GetEmployeeCountPerDepartment()
    {
        var expenses = await _departmentRepository
            .GetEmployeeCountPerDepartment();

        await _reportService.LogToCacheAsync(
            "Отримати кількість співробітників у кожному відділі",
            expenses);

        return expenses;
    }

    public async Task<IEnumerable<DepartmentExpenseDto>> GetExpenseAmountOfEachDepartment()
    {
        var expenses = await _departmentRepository
            .GetExpenseAmountOfEachDepartment();

        await _reportService.LogToCacheAsync(
            "Отримати відділи з сумою їх витрат",
            expenses);

        return expenses;
    }

    public async Task<IEnumerable<DepartmentTotalExpenseDto>> GetFilteredTotalExpensesPerDepartment()
    {
        var expenses = await _departmentRepository
            .GetFilteredTotalExpensesPerDepartment();

        await _reportService.LogToCacheAsync(
            "Отримати відділи з сумою їх витрат кількістю одобрених витрат та максимальним лімітом",
            expenses);

        return expenses;
    }

    public async Task<bool> RemoveAsync(long id)
        => await _departmentRepository.RemoveAsync(id);

    public async Task<bool> UpdateAsync(Department entity)
        => await _departmentRepository.UpdateAsync(entity);
}
