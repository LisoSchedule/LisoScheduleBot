using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Repositories;

namespace LisoScheduleBot.Services;

public class UserService : IUserService
{
    private readonly JsonUserRepository _userRepository;
    private readonly IUserSettingsService _settingsService;

    public UserService(JsonUserRepository userRepository, IUserSettingsService settingsService)
    {
        _userRepository = userRepository;
        _settingsService = settingsService;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _userRepository.GetAll();
    }

    public async Task<User> GetOrCreateUser(long chatId, string? username = null)
    {
        var user = await _userRepository.Get(chatId);

        if (user != null)
        {
            user.Settings = await _settingsService.GetOrCreateUserSettings(user.UserId);
            return user;
        }

        var users = await _userRepository.GetAll();

        user = new User
        {
            UserId = GetNextUserId(users),
            ChatId = chatId,
            Username = username,
            Nickname = string.Empty,
            GroupId = -1,
            Step = UserStep.ChooseNickname,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        user.Settings = await _settingsService.GetOrCreateUserSettings(user.UserId);

        return user;
    }

    public async Task SaveUser(User user)
    {
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.Save(user);
        await _settingsService.SaveUserSettings(user.Settings);
    }

    public async Task RemoveUser(User user)
    {
        await _settingsService.RemoveUserSettings(user.UserId);
        await _userRepository.Remove(user.UserId);
    }

    private int GetNextUserId(List<User> users)
    {
        return users.Any() ? users.Max(u => u.UserId) + 1 : 1;
    }
}
