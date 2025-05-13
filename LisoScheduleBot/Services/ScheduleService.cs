using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Repositories;

namespace LisoScheduleBot.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly JsonLessonRepository _lessonRepository;
        private readonly JsonLessonRecurrenceRepository _lessonRecurrenceRepository;

        public ScheduleService(JsonLessonRepository lessonRepository, JsonLessonRecurrenceRepository lessonRecurrenceRepository)
        {
            _lessonRepository = lessonRepository;
            _lessonRecurrenceRepository = lessonRecurrenceRepository;
        }

        //public async Task<List<ScheduleItem>> GetScheduleDetailsByDate(DateOnly date)
        //{
        //    var lessons = await _lessonRepository.GetAll();
        //    var lessonRecurrences = await _lessonRecurrenceRepository.GetAll();
        //}
    }
}
