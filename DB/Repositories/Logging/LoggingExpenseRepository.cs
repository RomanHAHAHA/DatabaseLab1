using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Entities;
using DatabaseLab1.Services.Interfaces;

namespace DatabaseLab1.DB.Repositories.Logging;

public class LoggingExpenseRepository : IExpenseRepository
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IReportService _reportService;

    public LoggingExpenseRepository(
        IExpenseRepository expenseRepository,
        IReportService reportService)
    {
        _expenseRepository = expenseRepository;
        _reportService = reportService;
    }

    public async Task<bool> CreateAsync(Expense entity)
        => await _expenseRepository.CreateAsync(entity);

    public async Task<IQueryable<Expense>> GetAllAsync()
        => await _expenseRepository.GetAllAsync();  

    public async Task<Expense?> GetByIdAsync(long id)
        => await _expenseRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Expense>> GetExpensesAboveAverageForType(long expenseTypeId)
    {
        var expenseTypes = await _expenseRepository
            .GetExpensesAboveAverageForType(expenseTypeId);

        await _reportService.LogToCacheAsync(
            "Отримати витрати з певним типом значення якиз більше за середнє",
            expenseTypes);

        return expenseTypes;
    }

    public async Task<IEnumerable<Expense>> GetExpensesExceedingTypeLimit()
    {
        var expenseTypes = await _expenseRepository
            .GetExpensesExceedingTypeLimit();

        await _reportService.LogToCacheAsync(
            "Отримати витрати ліміт яких більше за встановлений",
            expenseTypes);

        return expenseTypes;
    }

    public async Task<bool> RemoveAsync(long id)
        => await _expenseRepository.RemoveAsync(id);

    public async Task<bool> UpdateAsync(Expense entity)
        => await _expenseRepository.UpdateAsync(entity);
}
