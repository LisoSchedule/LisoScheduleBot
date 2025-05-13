using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Repositories;

namespace LisoScheduleBot.Services;

public class JsonTeacherService : IService<Teacher>
{
    private readonly JsonTeacherRepository _teacherRepository;

    public JsonTeacherService(JsonTeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public async Task<List<Teacher>> GetAllEntities()
    {
        return await _teacherRepository.GetAll();
    }

    public async Task<Teacher> GetOrCreateEntity(int teacherId)
    {
        var teacher = await _teacherRepository.Get(teacherId);

        if (teacher != null) return teacher;

        var allTeachers = await _teacherRepository.GetAll();

        teacher = new Teacher
        {
            TeacherId = GetNextTeacherId(allTeachers),
            FullName = string.Empty,
            Position = TeacherPosition.None,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return teacher;
    }

    public async Task SaveEntity(Teacher teacher)
    {
        teacher.UpdatedAt = DateTime.UtcNow;
        await _teacherRepository.Save(teacher);
    }

    public async Task RemoveEntity(int teacherId)
    {
        await _teacherRepository.Remove(teacherId);
    }

    private int GetNextTeacherId(List<Teacher> teachers)
    {
        return teachers.Any() ? teachers.Max(t => t.TeacherId) + 1 : 1;
    }
}