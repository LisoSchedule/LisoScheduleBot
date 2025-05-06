using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LisoScheduleBot.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum TeacherPosition
{
    Assistant,
    SeniorLecturer,
    AssociateProfessor,
    Professor
}
