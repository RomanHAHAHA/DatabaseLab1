namespace DatabaseLab1.DB.Interfaces;

public interface IRepository<T>
{
    Task<bool> CreateAsync(T entity);

    Task<T?> GetByIdAsync(long id);

    Task<IQueryable<T>> GetAllAsync();

    Task<bool> UpdateAsync(T entity);

    Task<bool> RemoveAsync(long id);
}
