using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using LisoScheduleBot.Config;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Utils;

namespace LisoScheduleBot.Repositories;

public class JsonLessonRepository : IRepository<Lesson>
{
    private readonly string _filePath;
    private readonly JsonSerializerSettings _settings;

    public JsonLessonRepository(AppConfig config)
    {
        _filePath = config.LessonsJson;
        _settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = { 
                new StringEnumConverter(),
                new StringTimeOnlyConverter()
            }
        };
    }

    public async Task<List<Lesson>> GetAll()
    {
        return await LoadLessons();
    }

    public async Task<Lesson?> Get(int lessonId)
    {
        var lessons = await LoadLessons();
        return lessons.FirstOrDefault(l => l.LessonId == lessonId);
    }

    public async Task Save(Lesson lesson)
    {
        var lessons = await LoadLessons();
        var existingLesson = lessons.FirstOrDefault(l => l.LessonId == lesson.LessonId);

        if (existingLesson != null)
        {
            lessons.Remove(existingLesson);
        }

        lessons.Add(lesson);
        await SaveChanges(lessons);
    }

    public async Task Remove(int lessonId)
    {
        var lessons = await LoadLessons();
        var existingLesson = lessons.FirstOrDefault(c => c.ClassroomId == lessonId);

        if (existingLesson != null)
        {
            lessons.Remove(existingLesson);
            await SaveChanges(lessons);
        }
    }

    private async Task<List<Lesson>> LoadLessons()
    {
        if (!File.Exists(_filePath)) return new List<Lesson>();

        var json = await File.ReadAllTextAsync(_filePath);
        return JsonConvert.DeserializeObject<List<Lesson>>(json, _settings) ?? new List<Lesson>();
    }

    private async Task SaveChanges(List<Lesson> lessons)
    {
        var json = JsonConvert.SerializeObject(lessons, _settings);
        await File.WriteAllTextAsync(_filePath, json);
    }
}