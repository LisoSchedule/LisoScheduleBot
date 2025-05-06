using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using LisoScheduleBot.Config;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;

namespace LisoScheduleBot.Repositories;

public class JsonTeacherRepository : IRepository<Teacher>
{
    private readonly string _filePath;
    private readonly JsonSerializerSettings _settings;

    public JsonTeacherRepository(AppConfig config)
    {
        _filePath = config.TeachersJson;
        _settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = { new StringEnumConverter() }
        };
    }

    
    public async Task<List<Teacher>> GetAll()
    {
        return await LoadTeachers();
    }

    public async Task<Teacher?> Get(int teacherId)
    {
        var teachers = await LoadTeachers();
        return teachers.FirstOrDefault(t => t.TeacherId == teacherId);
    }

    public async Task Save(Teacher teacher)
    {
        var teachers = await LoadTeachers();
        var existingTeacher = teachers.FirstOrDefault(t => t.TeacherId == teacher.TeacherId);

        if (existingTeacher != null)
        {
            teachers.Remove(existingTeacher);
        }

        teachers.Add(teacher);
        await SaveChanges(teachers);
    }

    public async Task Remove(int teacherId)
    {
        var teachers = await LoadTeachers();
        var existingTeacher = teachers.FirstOrDefault(t => t.TeacherId == teacherId);

        if (existingTeacher != null)
        {
            teachers.Remove(existingTeacher);
            await SaveChanges(teachers);
        }
    }

    private async Task<List<Teacher>> LoadTeachers()
    {
        if (!File.Exists(_filePath)) return new List<Teacher>();

        var json = await File.ReadAllTextAsync(_filePath);
        return JsonConvert.DeserializeObject<List<Teacher>>(json, _settings) ?? new List<Teacher>();
    }

    private async Task SaveChanges(List<Teacher> teachers)
    {
        var json = JsonConvert.SerializeObject(teachers, _settings);
        await File.WriteAllTextAsync(_filePath, json);
    }
}