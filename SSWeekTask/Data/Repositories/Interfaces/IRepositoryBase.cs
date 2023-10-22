namespace SSWeekTask.Data.Repositories.Interfaces;

public interface IRepositoryBase<T>
{
    Task<ICollection<T>> GetAll();

    Task<T> GetById(Guid id);

    Task<T> Update(T entity);

    Task<Guid> Create(T entity);

    Task Delete(T entity);
}
