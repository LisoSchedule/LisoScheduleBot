using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Repositories;

public class GroupService : IGroupService
{
    private readonly JsonGroupRepository _groupRepository;

    public GroupService(JsonGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<List<Group>> GetAllGroups()
    {
        return await _groupRepository.GetAll();
    }

    public async Task<List<Group>> GetUniqueGroups()
    {
        var groups = await _groupRepository.GetAll();
        return groups
            .GroupBy(g => g.Name)
            .Select(g => g.First())
            .ToList();
    }

    public async Task<List<Group>> GetGroups(string groupName)
    {
        var groups = await _groupRepository.GetAll();
        return groups
            .Where(g => g.Name == groupName)
            .ToList();
    }

    public async Task<Group> GetGroup(int groupId)
    {
        var groups = await _groupRepository.GetAll();
        return groups.FirstOrDefault(g => g.GroupId == groupId) ?? new Group();
    }

    public async Task<Group> GetGroup(string groupName, int subGroup = 1)
    {
        var groups = await _groupRepository.GetAll();
        return groups.FirstOrDefault(g => g.Name == groupName && g.SubGroup == subGroup) ?? new Group();
    }

    public async Task<List<int>> GetSubGroups(string groupName)
    {
        var groups = await _groupRepository.GetAll();
        return groups
            .Where(g => g.Name == groupName)
            .Select(g => g.SubGroup)
            .ToList();
    }

    public async Task SaveGroup(Group group)
    {
        group.UpdatedAt = DateTime.UtcNow;
        await _groupRepository.Save(group);
    }
}
