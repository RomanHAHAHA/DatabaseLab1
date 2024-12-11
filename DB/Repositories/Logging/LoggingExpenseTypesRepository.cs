using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Dtos.ExpenseTypeDtos;
using DatabaseLab1.Domain.Entities;
using DatabaseLab1.Services.Interfaces;

namespace DatabaseLab1.DB.Repositories.Logging;

public class LoggingExpenseTypesRepository : IExpenseTypeRepository
{
    private readonly IExpenseTypeRepository _expenseTypeRepository;
    private readonly IReportService _reportService;

    public LoggingExpenseTypesRepository(
        IExpenseTypeRepository expenseTypeRepository,
        IReportService reportService)
    {
        _expenseTypeRepository = expenseTypeRepository;
        _reportService = reportService;
    }

    public async Task<bool> CreateAsync(ExpenseType entity)
        => await _expenseTypeRepository.CreateAsync(entity);

    public async Task<IQueryable<ExpenseType>> GetAllAsync()
        => await _expenseTypeRepository.GetAllAsync();

    public async Task<IEnumerable<ExpenseTypeCountDto>> GetAverageLimitPerExpenseType()
    {
        var expenseTypes = await _expenseTypeRepository
            .GetAverageLimitPerExpenseType();

        await _reportService.LogToCacheAsync(
            "Отримати всі типи витрат з кількістю самих витрат що прив'язані до цього типу",
            expenseTypes);

        return expenseTypes;
    }

    public async Task<ExpenseType?> GetByIdAsync(long id)
        => await _expenseTypeRepository.GetByIdAsync(id);

    public async Task<IEnumerable<ExpenseTypeMaxDto>> GetMaxApprovedExpensesPerType()
    {
        var expenseTypes = await _expenseTypeRepository
            .GetMaxApprovedExpensesPerType();

        await _reportService.LogToCacheAsync(
            "Отримати типи витрат що були одобрені з максимальним знаенням",
            expenseTypes);

        return expenseTypes;
    }

    public async Task<IEnumerable<ExpenseType>> GetUnusedExpenseTypesInDepartment(long departmentId)
    {
        var expenseTypes = await _expenseTypeRepository
            .GetUnusedExpenseTypesInDepartment(departmentId);

        await _reportService.LogToCacheAsync(
            "Отримати типи витрат що не використовуються в певному відділені",
            expenseTypes);

        return expenseTypes;
    }

    public async Task<bool> RemoveAsync(long id)
        => await _expenseTypeRepository.RemoveAsync(id);

    public async Task<bool> UpdateAsync(ExpenseType entity)
        => await _expenseTypeRepository.UpdateAsync(entity);
}
