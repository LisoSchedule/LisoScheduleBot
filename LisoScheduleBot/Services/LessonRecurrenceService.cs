using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Repositories;

namespace LisoScheduleBot.Services;

public class LessonRecurrenceService : IService<LessonRecurrence>
{
    private readonly JsonLessonRecurrenceRepository _recurrenceRepository;

    public LessonRecurrenceService(JsonLessonRecurrenceRepository recurrenceRepository)
    {
        _recurrenceRepository = recurrenceRepository;
    }

    public async Task<List<LessonRecurrence>> GetAllEntities()
    {
        return await _recurrenceRepository.GetAll();
    }

    public async Task<LessonRecurrence> GetOrCreateEntity(int recurrenceId)
    {
        var recurrence = await _recurrenceRepository.Get(recurrenceId);

        if (recurrence != null) return recurrence;

        var allRecurrences = await _recurrenceRepository.GetAll();

        recurrence = new LessonRecurrence
        {
            RecurrenceId = GetNextRecurrenceId(allRecurrences),
            LessonId = -1,
            Repeatability = RepeatPattern.None,
            StartDate = new DateOnly(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return recurrence;
    }

    public async Task SaveEntity(LessonRecurrence recurrence)
    {
        recurrence.UpdatedAt = DateTime.UtcNow;
        await _recurrenceRepository.Save(recurrence);
    }

    public async Task RemoveEntity(int recurrenceId)
    {
        await _recurrenceRepository.Remove(recurrenceId);
    }

    private int GetNextRecurrenceId(List<LessonRecurrence> recurrences)
    {
        return recurrences.Any() ? recurrences.Max(r => r.RecurrenceId) + 1 : 1;
    }
}