using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using LisoScheduleBot.Config;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Utils;

namespace LisoScheduleBot.Repositories;

public class JsonLessonRecurrenceRepository : IRepository<LessonRecurrence>
{
    private readonly string _filePath;
    private readonly JsonSerializerSettings _settings;

    public JsonLessonRecurrenceRepository(AppConfig config)
    {
        _filePath = config.LessonRecurrencesJson;
        _settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = { 
                new StringEnumConverter(),
                new StringDateOnlyConverter()
            }
        };
    }

    public async Task<List<LessonRecurrence>> GetAll()
    {
        return await LoadRecurrences();
    }

    public async Task<LessonRecurrence?> Get(int reccurenceId)
    {
        var recurrences = await LoadRecurrences();
        return recurrences.FirstOrDefault(r => r.RecurrenceId == reccurenceId);
    }

    public async Task Save(LessonRecurrence recurrence)
    {
        var recurrences = await LoadRecurrences();
        var existingRecurrence = recurrences.FirstOrDefault(r => r.RecurrenceId == recurrence.RecurrenceId);

        if (existingRecurrence != null)
        {
            recurrences.Remove(existingRecurrence);
        }

        recurrences.Add(recurrence);
        await SaveChanges(recurrences);
    }

    public async Task Remove(int recurrenceId)
    {
        var recurrences = await LoadRecurrences();
        var existingRecurrence = recurrences.FirstOrDefault(t => t.RecurrenceId == recurrenceId);

        if (existingRecurrence != null)
        {
            recurrences.Remove(existingRecurrence);
            await SaveChanges(recurrences);
        }
    }

    private async Task<List<LessonRecurrence>> LoadRecurrences()
    {
        if (!File.Exists(_filePath)) return new List<LessonRecurrence>();

        var json = await File.ReadAllTextAsync(_filePath);
        return JsonConvert.DeserializeObject<List<LessonRecurrence>>(json, _settings) ?? new List<LessonRecurrence>();
    }

    private async Task SaveChanges(List<LessonRecurrence> recurrences)
    {
        var json = JsonConvert.SerializeObject(recurrences, _settings);
        await File.WriteAllTextAsync(_filePath, json);
    }
}