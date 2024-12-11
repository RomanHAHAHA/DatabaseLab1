using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Entities;
using DatabaseLab1.Services.Interfaces;

namespace DatabaseLab1.DB.Repositories.Logging;

public class LoggingExpenseDetailsRepository : IExpenseDetailsRepository
{
    private readonly IExpenseDetailsRepository _expenseDetailsRepository;
    private readonly IReportService _reportService;

    public LoggingExpenseDetailsRepository(
        IExpenseDetailsRepository expenseDetailsRepository,
        IReportService reportService)
    {
        _expenseDetailsRepository = expenseDetailsRepository;
        _reportService = reportService;
    }

    public async Task<bool> CreateAsync(ExpenseDetails entity)
        => await _expenseDetailsRepository.CreateAsync(entity);

    public async Task<IQueryable<ExpenseDetails>> GetAllAsync()
        => await _expenseDetailsRepository.GetAllAsync();

    public async Task<IEnumerable<ExpenseDetails>> GetApprovedExpensesLastMonth()
    {
        var expenseDetails = await _expenseDetailsRepository
            .GetApprovedExpensesLastMonth();

        await _reportService.LogToCacheAsync(
            "Отримати деталі витрат що були одобрені за останній місяць",
            expenseDetails);

        return expenseDetails;
    }

    public async Task<ExpenseDetails?> GetByIdAsync(long id)
        => await _expenseDetailsRepository.GetByIdAsync(id);

    public async Task<bool> RemoveAsync(long id)
        => await _expenseDetailsRepository.RemoveAsync(id);

    public async Task<bool> UpdateAsync(ExpenseDetails entity)
        => await _expenseDetailsRepository.UpdateAsync(entity);
}
