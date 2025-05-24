using Hangfire;
using Hangfire.Storage;
using LisoScheduleBot.Config;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;

namespace LisoScheduleBot.Services;

public class HangfireService
{
    private readonly IService<Lesson> _lessonService;
    private readonly INotificationService _notificationService;
    private readonly IRecurringJobManager _recurringJobManager;

    public HangfireService(IService<Lesson> lessonService, 
        INotificationService notificationService, 
        IRecurringJobManager recurringJobManager)
    {
        _lessonService = lessonService;
        _notificationService = notificationService;
        _recurringJobManager = recurringJobManager;
    }

    public async Task RegisterJobs()
    {
        var crons = await GetReminderCrons();
        var allowedIds = new HashSet<string>();
        var offset = Math.Abs(DateTime.UtcNow.Hour - DateTime.Now.Hour);

        foreach (var cron in crons)
        {
            var parts = cron.Split(' ');
            var hour = (int.Parse(parts[1]) + offset) % 24;
            var min = parts[0];
            var jobId = $"send-reminders-{hour}h-{min}m";
            allowedIds.Add(jobId);

            _recurringJobManager.AddOrUpdate(jobId, () => _notificationService.SendReminders(), cron);
        }

        RemoveObsoleteJobs(allowedIds);
    }

    private async Task<List<TimeOnly>> GetUniqueReminderTimes()
    {
        var lessons = await _lessonService.GetAllEntities();
        return lessons.Select(l => l.StartTime).Distinct().Order().ToList();
    }

    private async Task<List<string>> GetReminderCrons()
    {
        var times = await GetUniqueReminderTimes();
        var reminderTimes = Enum.GetValues<TimeBeforeClass>().Select(v => (int)v).OrderDescending();
        var crons = new List<string>();
        var offset = Math.Abs(DateTime.UtcNow.Hour - DateTime.Now.Hour);

        foreach (var time in times)
        {
            foreach (var reminderTime in reminderTimes)
            {
                var notifyTime = time.AddMinutes(-reminderTime);

                if (notifyTime.Hour < 0 || notifyTime.Minute < 0)
                    continue;

                var localHour = (notifyTime.Hour - offset) % 24;
                var cron = $"{notifyTime.Minute} {localHour} * * *";

                if (!crons.Contains(cron)) crons.Add(cron);
            }
        }

        return crons;
    }

    private void RemoveObsoleteJobs(HashSet<string> allowedIds)
    {
        var connection = JobStorage.Current.GetConnection();

        foreach (var job in connection.GetRecurringJobs())
        {
            if (!allowedIds.Contains(job.Id)) RecurringJob.RemoveIfExists(job.Id);
        }
    }
}
