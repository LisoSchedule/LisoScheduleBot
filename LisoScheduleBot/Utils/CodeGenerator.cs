using System.Text;

namespace LisoScheduleBot.Utils;

public static class CodeGenerator
{
    private static readonly Random _random = new();

    public static string GenerateVerificationCode(int digits = 6)
    {
        var sb = new StringBuilder("L-");

        for (int i = 0; i < digits; i++)
        {
            sb.Append(_random.Next(0, 10));
        }

        return sb.ToString();
    }
}
