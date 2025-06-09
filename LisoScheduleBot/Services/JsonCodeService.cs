using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Repositories;
using LisoScheduleBot.Utils;

namespace LisoScheduleBot.Services;

public class JsonCodeService : IService<VerificationCode>, ICodeService
{
    private readonly JsonCodeRepository _codeRepository;

    public JsonCodeService(JsonCodeRepository codeRepository)
    {
        _codeRepository = codeRepository;
    }

    public async Task<VerificationCode> GetOrCreateLastCode(int userId)
    {
        var allCodes = await _codeRepository.GetAll();
        var code = allCodes
            .Where(c => c.UserId == userId && !c.IsUsed)
            .OrderByDescending(c => c.CreatedAt)
            .FirstOrDefault();

        if (code != null) return code;

        code = new VerificationCode
        {
            CodeId = GetNextCodeId(allCodes),
            UserId = userId,
            Code = CodeGenerator.GenerateVerificationCode(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return code;
    }

    public async Task<List<VerificationCode>> GetAllEntities()
    {
        return await _codeRepository.GetAll();
    }

    public async Task<VerificationCode> GetOrCreateEntity(int codeId)
    {
        var code = await _codeRepository.Get(codeId);

        if (code != null) return code;

        var allCodes = await _codeRepository.GetAll();

        code = new VerificationCode
        {
            CodeId = GetNextCodeId(allCodes),
            UserId = codeId,
            Code = CodeGenerator.GenerateVerificationCode(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return code;
    }

    public async Task SaveEntity(VerificationCode code)
    {
        code.UpdatedAt = DateTime.UtcNow;
        await _codeRepository.Save(code);
    }

    public async Task RemoveEntity(int codeId)
    {
        await _codeRepository.Remove(codeId);
    }

    private int GetNextCodeId(List<VerificationCode> codes)
    {
        return codes.Any() ? codes.Max(c => c.CodeId) + 1 : 1;
    }
}
