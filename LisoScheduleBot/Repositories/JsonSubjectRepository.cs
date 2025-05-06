using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using LisoScheduleBot.Config;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;

namespace LisoScheduleBot.Repositories;

public class JsonSubjectRepository : IRepository<Subject>
{
    private readonly string _filePath;
    private readonly JsonSerializerSettings _settings;

    public JsonSubjectRepository(AppConfig config)
    {
        _filePath = config.SubjectsJson;
        _settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = { new StringEnumConverter() }
        };
    }

    public async Task<List<Subject>> GetAll()
    {
        return await LoadSubjects();
    }

    public async Task<Subject?> Get(int subjectId)
    {
        var subjects = await LoadSubjects();
        return subjects.FirstOrDefault(s => s.SubjectId == subjectId);
    }

    public async Task Save(Subject subject)
    {
        var subjects = await LoadSubjects();
        var existingSubject = subjects.FirstOrDefault(s => s.SubjectId == subject.SubjectId);

        if (existingSubject != null)
        {
            subjects.Remove(existingSubject);
        }

        subjects.Add(subject);
        await SaveChanges(subjects);
    }

    public async Task Remove(int subjectId)
    {
        var subjects = await LoadSubjects();
        var existingSubject = subjects.FirstOrDefault(s => s.SubjectId == subjectId);

        if (existingSubject != null)
        {
            subjects.Remove(existingSubject);
            await SaveChanges(subjects);
        }
    }

    private async Task<List<Subject>> LoadSubjects()
    {
        if (!File.Exists(_filePath)) return new List<Subject>();

        using (var reader = new StreamReader(_filePath))
        {
            var json = await reader.ReadToEndAsync();
            return JsonConvert.DeserializeObject<List<Subject>>(json, _settings) ?? new List<Subject>();
        }
    }

    private async Task SaveChanges(List<Subject> subjects)
    {
        var json = JsonConvert.SerializeObject(subjects, _settings);
        using (var writer = new StreamWriter(_filePath))
        {
            await writer.WriteAsync(json);
        }
    }

}