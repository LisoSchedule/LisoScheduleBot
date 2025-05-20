using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LisoScheduleBot.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum TeacherPosition
{
    None,
    Assistant,
    SeniorLecturer,
    AssociateProfessor,
    Professor
}
