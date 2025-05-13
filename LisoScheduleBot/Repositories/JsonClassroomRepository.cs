using Newtonsoft.Json;
using LisoScheduleBot.Config;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;

namespace LisoScheduleBot.Repositories;

public class JsonClassroomRepository : IRepository<Classroom>
{
    private readonly string _filePath;
    private readonly JsonSerializerSettings _settings;

    public JsonClassroomRepository(AppConfig config)
    {
        _filePath = config.ClassroomsJson;
        _settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        };
    }

    public async Task<List<Classroom>> GetAll()
    {
        return await LoadClassrooms();
    }

    public async Task<Classroom?> Get(int classroomId)
    {
        var classrooms = await LoadClassrooms();
        return classrooms.FirstOrDefault(c => c.ClassroomId == classroomId);
    }

    public async Task Save(Classroom classroom)
    {
        var classrooms = await LoadClassrooms();
        var existingClassroom = classrooms.FirstOrDefault(c => c.ClassroomId == classroom.ClassroomId);

        if (existingClassroom != null) classrooms.Remove(existingClassroom);

        classrooms.Add(classroom);
        await SaveChanges(classrooms);
    }

    public async Task Remove(int classroomId)
    {
        var classrooms = await LoadClassrooms();
        var existingClassroom = classrooms.FirstOrDefault(c => c.ClassroomId == classroomId);

        if (existingClassroom != null)
        {
            classrooms.Remove(existingClassroom);
            await SaveChanges(classrooms);
        }
    }

    private async Task<List<Classroom>> LoadClassrooms()
    {
        if (!File.Exists(_filePath)) return new List<Classroom>();

        var json = await File.ReadAllTextAsync(_filePath);
        return JsonConvert.DeserializeObject<List<Classroom>>(json, _settings) ?? new List<Classroom>();
    }

    private async Task SaveChanges(List<Classroom> classrooms)
    {
        var json = JsonConvert.SerializeObject(classrooms, _settings);
        await File.WriteAllTextAsync(_filePath, json);
    }
}
