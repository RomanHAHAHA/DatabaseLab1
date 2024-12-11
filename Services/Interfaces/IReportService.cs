namespace DatabaseLab1.Services.Interfaces;

public interface IReportService
{
    Task LogToCacheAsync(string description, object? entity = null);
}