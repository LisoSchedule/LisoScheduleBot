namespace LisoScheduleBot.Interfaces;

public interface IRepository<T>
{
    Task<List<T>> GetAll();
    Task<T?> Get(int id);
    Task Save(T entity);
    Task Remove(int id);
}
