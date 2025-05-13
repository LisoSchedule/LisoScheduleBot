using LisoScheduleBot.Models;

namespace LisoScheduleBot.Interfaces;

public interface IGroupService
{
    Task<List<Group>> GetAllGroups();
    Task<List<Group>> GetUniqueGroups();
    Task<List<Group>> GetGroups(string groupName);
    Task<Group> GetGroup(int groupId);
    Task<Group> GetGroup(string groupName, int subGroup = 1);
    Task<List<int>> GetSubGroups(string groupName);
    Task SaveGroup(Group group);
}
