using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Repositories;
using LisoScheduleBot.Utils;

namespace LisoScheduleBot.Services;

public class JsonScheduleService : IScheduleService
{
    private readonly JsonClassroomRepository _classroomRepository;
    private readonly JsonLessonRepository _lessonRepository;
    private readonly JsonLessonRecurrenceRepository _lessonRecurrenceRepository;
    private readonly JsonSubjectRepository _subjectRepository;
    private readonly JsonTeacherRepository _teacherRepository;

    public JsonScheduleService(IUserService userService, JsonClassroomRepository classroomRepository, 
        JsonLessonRepository lessonRepository, JsonLessonRecurrenceRepository lessonRecurrenceRepository,
        JsonSubjectRepository subjectRepository, JsonTeacherRepository teacherRepository)
    {
        _classroomRepository = classroomRepository;
        _lessonRepository = lessonRepository;
        _lessonRecurrenceRepository = lessonRecurrenceRepository;
        _subjectRepository = subjectRepository;
        _teacherRepository = teacherRepository;
    }

    public async Task<List<ScheduleItem>> GetScheduleItemsByDate(DateOnly date, User user)
    {
        var recurrences = await _lessonRecurrenceRepository.GetAll();
        var scheduleItems = new List<ScheduleItem>();

        foreach (var recurrence in recurrences)
        {
            if (!IsDateInRecurrence(recurrence, date)) continue;

            var lesson = await _lessonRepository.Get(recurrence.LessonId);
            if (lesson == null) continue;

            if (lesson.GroupId != user.GroupId) continue;

            var subject = await _subjectRepository.Get(lesson.SubjectId);
            var teacher = await _teacherRepository.Get(lesson.TeacherId);
            var classroom = await _classroomRepository.Get(lesson.ClassroomId);

            scheduleItems.Add(
                new ScheduleItem 
                {
                    SubjectType = EnumConverter<SubjectType>.EnumToString(subject!.Type),
                    Subject = subject!.Name,
                    Teacher = teacher!.FullName,
                    Classroom = $"Корпус {classroom!.Hull}, Ауд. {classroom.Room}",
                    StartTime = lesson.StartTime,
                    Duration = lesson.Duration
                }
            );
        }

        scheduleItems = scheduleItems.OrderBy(s => s.StartTime).ToList();

        return scheduleItems;
    }

    private bool IsDateInRecurrence(LessonRecurrence recurrence, DateOnly date)
    {
        if (date < recurrence.StartDate || date > recurrence.EndDate)
            return false;

        switch (recurrence.RepeatType)
        {
            case RepeatType.Daily:
                int daysDiff = (date.DayNumber - recurrence.StartDate.DayNumber);
                return daysDiff % recurrence.RepeatValue == 0;

            case RepeatType.Weekly:
                int totalDays = (date.DayNumber - recurrence.StartDate.DayNumber);
                if (totalDays < 0) return false;
                return (totalDays % (7 * recurrence.RepeatValue) == 0);

            case RepeatType.Monthly:
                int monthsDiff = ((date.Year - recurrence.StartDate.Year) * 12) + (date.Month - recurrence.StartDate.Month);
                return date.Day == recurrence.StartDate.Day && monthsDiff % recurrence.RepeatValue == 0;

            default:
                return false;
        }
    }

}
