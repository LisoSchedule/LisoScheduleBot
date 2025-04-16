using LisoScheduleBot.Models;

namespace LisoScheduleBot.Interfaces;

public interface IGroupRepository
{
    Task<List<Group>> GetAll();
    Task<Group?> Get(int groupId);
    Task Save(Group group);
}
