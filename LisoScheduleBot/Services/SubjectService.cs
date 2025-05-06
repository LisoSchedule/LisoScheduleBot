using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Repositories;

namespace LisoScheduleBot.Services;

public class SubjectService : IService<Subject>
{
    private readonly JsonSubjectRepository _subjectRepository;

    public SubjectService(JsonSubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    public async Task<List<Subject>> GetAllEntities()
    {
        return await _subjectRepository.GetAll();
    }

    public async Task<Subject> GetOrCreateEntity(int subjectId)
    {
        var subject = await _subjectRepository.Get(subjectId);

        if (subject != null) return subject;

        var allSubjects = await _subjectRepository.GetAll();

        subject = new Subject
        {
            SubjectId = GetNextSubjectId(allSubjects),
            Type = SubjectType.None,
            Name = string.Empty,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return subject;
    }

    public async Task SaveEntity(Subject subject)
    {
        subject.UpdatedAt = DateTime.UtcNow;
        await _subjectRepository.Save(subject);
    }

    public async Task RemoveEntity(int subjectId)
    {
        await _subjectRepository.Remove(subjectId);
    }

    private int GetNextSubjectId(List<Subject> subjects)
    {
        return subjects.Any() ? subjects.Max(s => s.SubjectId) + 1 : 1;
    }
}