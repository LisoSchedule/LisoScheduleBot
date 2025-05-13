using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Repositories;

namespace LisoScheduleBot.Services;

public class UserService : IUserService
{
    private readonly JsonUserRepository _userRepository;

    public UserService(JsonUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _userRepository.GetAll();
    }

    public async Task<User> GetOrCreateUser(long userId, string? username = null)
    {
        //var user = api request

        var user = await _userRepository.Get(userId);

        if (user != null)
        {
            return user;
        }

        var users = await _userRepository.GetAll();

        user = new User
        {
            UserId = GetNextUserId(users),
            ChatId = userId,
            Username = username,
            Nickname = string.Empty,
            GroupId = -1,
            Step = UserStep.ChooseNickname,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return user;
    }

    public async Task SaveUser(User user)
    {
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.Save(user);
    }

    private int GetNextUserId(List<User> users)
    {
        return users.Any() ? users.Max(u => u.UserId) + 1 : 1;
    }
}
