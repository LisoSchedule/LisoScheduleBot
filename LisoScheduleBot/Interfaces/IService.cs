namespace LisoScheduleBot.Interfaces;

public interface IService<T>
{
    Task<List<T>> GetAllEntities();
    Task<T> GetOrCreateEntity(int id);
    Task SaveEntity(T entity);
    Task RemoveEntity(int id);
}
