using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Repositories;

namespace LisoScheduleBot.Services;

public class JsonClassroomService : IService<Classroom>
{
    private readonly JsonClassroomRepository _classroomRepository;

    public JsonClassroomService(JsonClassroomRepository classroomRepository)
    {
        _classroomRepository = classroomRepository;
    }

    public async Task<List<Classroom>> GetAllEntities()
    {
        return await _classroomRepository.GetAll();
    }

    public async Task<Classroom> GetOrCreateEntity(int classroomId)
    {
        var classroom = await _classroomRepository.Get(classroomId);

        if (classroom != null) return classroom;

        var allClassrooms = await _classroomRepository.GetAll();

        classroom = new Classroom
        {
            ClassroomId = GetNextUserSettignsId(allClassrooms),
            Hull = string.Empty,
            Room = string.Empty,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return classroom;
    }

    public async Task SaveEntity(Classroom classroom)
    {
        classroom.UpdatedAt = DateTime.UtcNow;
        await _classroomRepository.Save(classroom);
    }

    public async Task RemoveEntity(int classroomId)
    {
        await _classroomRepository.Remove(classroomId);
    }

    private int GetNextUserSettignsId(List<Classroom> classrooms)
    {
        return classrooms.Any() ? classrooms.Max(c => c.ClassroomId) + 1 : 1;
    }
}
