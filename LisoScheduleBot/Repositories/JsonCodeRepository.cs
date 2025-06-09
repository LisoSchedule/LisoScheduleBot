using Newtonsoft.Json;
using LisoScheduleBot.Config;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;

namespace LisoScheduleBot.Repositories;

public class JsonCodeRepository : IRepository<VerificationCode>
{
    private readonly string _filePath;
    private readonly JsonSerializerSettings _settings;

    public JsonCodeRepository(AppConfig config)
    {
        _filePath = config.CodesJson;
        _settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        };
    }

    public async Task<List<VerificationCode>> GetAll()
    {
        return await LoadCodes();
    }

    public async Task<VerificationCode?> Get(int codeId)
    {
        var codes = await LoadCodes();
        return codes.FirstOrDefault(c => c.CodeId == codeId);
    }

    public async Task Save(VerificationCode code)
    {
        var codes = await LoadCodes();
        var existingCode = codes.FirstOrDefault(c => c.CodeId == code.CodeId);

        if (existingCode != null) codes.Remove(existingCode);

        codes.Add(code);
        await SaveChanges(codes);
    }

    public async Task Remove(int codeId)
    {
        var codes = await LoadCodes();
        var existingCode = codes.FirstOrDefault(c => c.CodeId == codeId);

        if (existingCode != null)
        {
            codes.Remove(existingCode);
            await SaveChanges(codes);
        }
    }

    private async Task<List<VerificationCode>> LoadCodes()
    {
        if (!File.Exists(_filePath)) return new List<VerificationCode>();

        var json = await File.ReadAllTextAsync(_filePath);
        return JsonConvert.DeserializeObject<List<VerificationCode>>(json, _settings) ?? new List<VerificationCode>();
    }

    private async Task SaveChanges(List<VerificationCode> codes)
    {
        var json = JsonConvert.SerializeObject(codes, _settings);
        await File.WriteAllTextAsync(_filePath, json);
    }
}
