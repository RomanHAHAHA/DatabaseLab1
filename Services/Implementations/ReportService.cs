using DatabaseLab1.Services.Interfaces;
using Newtonsoft.Json;

namespace DatabaseLab1.Services.Implementations;

public class ReportService(ICacheService cacheService) : IReportService
{
    private readonly ICacheService _cacheService = cacheService;

    public async Task LogToCacheAsync(
        string description,
        object? entity = null)
    {
        var jsonEntity = entity is not null ? JsonConvert.SerializeObject(entity) : "NULL";
        var reportString = $"" +
            $"Description: {description}, " +
            $"Date: {DateTime.Now:[dd.MM.yyyy][HH:mm]}, " +
            $"Object: \n{jsonEntity}";

        await _cacheService.SetAsync(reportString);
    }
}
