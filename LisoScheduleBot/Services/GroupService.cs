using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Repositories;

public class GroupService : IService<Group>, IGroupService
{
    private readonly JsonGroupRepository _groupRepository;

    public GroupService(JsonGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<List<Group>> GetAllEntities()
    {
        return await _groupRepository.GetAll();
    }

    public async Task<Group> GetOrCreateEntity(int groupId)
    {
        var group = await _groupRepository.Get(groupId);

        if (group != null) return group;

        var allGroups = await _groupRepository.GetAll();

        group = new Group
        {
            GroupId = GetNextGroupId(allGroups),
            SubGroup = -1,
            Name = GroupName.None,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return group;
    }

    public async Task SaveEntity(Group group)
    {
        group.UpdatedAt = DateTime.UtcNow;
        await _groupRepository.Save(group);
    }

    public async Task RemoveEntity(int groupId)
    {
        await _groupRepository.Remove(groupId);
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
            .Where(g => g.Name.ToString() == groupName)
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
        return groups.FirstOrDefault(g => g.Name.ToString() == groupName && g.SubGroup == subGroup) ?? new Group();
    }

    public async Task<List<int>> GetSubGroups(string groupName)
    {
        var groups = await _groupRepository.GetAll();
        return groups
            .Where(g => g.Name.ToString() == groupName)
            .Select(g => g.SubGroup)
            .ToList();
    }

    private int GetNextGroupId(List<Group> groups)
    {
        return groups.Any() ? groups.Max(g => g.GroupId) + 1 : 1;
    }
}
