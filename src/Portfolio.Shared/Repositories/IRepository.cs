namespace Repositories;
public interface IRepository<T>
{    
    Task<IEnumerable<T>> GetAllAsync();
}