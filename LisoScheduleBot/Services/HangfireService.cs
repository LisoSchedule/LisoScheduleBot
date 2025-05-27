using Hangfire;
using Hangfire.Storage;
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

        foreach (var cron in crons)
        {
            var parts = cron.Split(' ');
            var hour = parts[1];
            var min = parts[0];
            var jobId = $"send-reminders-{hour}h-{min}m";
            allowedIds.Add(jobId);

            var options = new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Local
            };

            _recurringJobManager.AddOrUpdate(jobId, () => _notificationService.SendReminders(), cron, options);
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

        foreach (var time in times)
        {
            foreach (var reminderTime in reminderTimes)
            {
                var notifyTime = time.AddMinutes(-reminderTime);
                if (notifyTime.Hour < 0 || notifyTime.Minute < 0) continue;

                var cron = Cron.Daily(notifyTime.Hour, notifyTime.Minute);
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
