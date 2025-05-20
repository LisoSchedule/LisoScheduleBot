using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Repositories;

namespace LisoScheduleBot.Services;

public class JsonLessonService : IService<Lesson>
{
    private readonly JsonLessonRepository _lessonRepository;

    public JsonLessonService(JsonLessonRepository lessonRepository)
    {
        _lessonRepository = lessonRepository;
    }

    public async Task<List<Lesson>> GetAllEntities()
    {
        return await _lessonRepository.GetAll();
    }

    public async Task<Lesson> GetOrCreateEntity(int lessonId)
    {
        var lesson = await _lessonRepository.Get(lessonId);

        if (lesson != null) return lesson;

        var allLessons = await _lessonRepository.GetAll();

        lesson = new Lesson
        {
            LessonId = GetNextLessonId(allLessons),
            ClassroomId = -1,
            TeacherId = -1,
            GroupId = -1,
            SubjectId = -1,
            Duration = -1,
            StartTime = new TimeOnly(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return lesson;
    }

    public async Task SaveEntity(Lesson Lesson)
    {
        Lesson.UpdatedAt = DateTime.UtcNow;
        await _lessonRepository.Save(Lesson);
    }

    public async Task RemoveEntity(int lessonId)
    {
        await _lessonRepository.Remove(lessonId);
    }

    private int GetNextLessonId(List<Lesson> lessons)
    {
        return lessons.Any() ? lessons.Max(l => l.LessonId) + 1 : 1;
    }
}
