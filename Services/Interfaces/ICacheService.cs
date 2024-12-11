namespace DatabaseLab1.Services.Interfaces;

public interface ICacheService
{
    Task<string> SetAsync<T>(T data) where T : class;

    Task<List<T>> GetAllAsync<T>() where T : class;

    Task RemoveAsync(string key);
}