using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using LisoScheduleBot.Config;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;

namespace LisoScheduleBot.Repositories;

public class JsonUserRepository : IUserRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerSettings _settings;

    public JsonUserRepository(AppConfig config)
    {
        _filePath = config.UsersJson;
        _settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = { new StringEnumConverter() }
        };
    }

    public async Task<List<User>> GetAll()
    {
        return await LoadUsers();
    }

    public async Task<User?> Get(long chatId)
    {
        var users = await LoadUsers();
        return users.FirstOrDefault(u => u.ChatId == chatId);
    }

    public async Task Save(User user)
    {
        var users = await LoadUsers();
        var existingUser = users.FirstOrDefault(u => u.ChatId == user.ChatId);

        if (existingUser != null) users.Remove(existingUser);

        users.Add(user);
        await SaveChanges(users);
    }

    public async Task Remove(int userId)
    {
        var users = await LoadUsers();
        var existingUser = users.FirstOrDefault(u => u.UserId == userId);

        if (existingUser != null)
        {
            users.Remove(existingUser);
            await SaveChanges(users);
        }
    }

    private async Task<List<User>> LoadUsers()
    {
        if (!File.Exists(_filePath)) return new List<User>();
        
        var json = await File.ReadAllTextAsync(_filePath);
        return JsonConvert.DeserializeObject<List<User>>(json, _settings) ?? new List<User>();
    }

    private async Task SaveChanges(List<User> users)
    {
        var json = JsonConvert.SerializeObject(users, _settings);
        await File.WriteAllTextAsync(_filePath, json);
    }
}
