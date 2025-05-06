using LisoScheduleBot.Enums;

namespace LisoScheduleBot.Utils;

public static class EnumConverter<T>
{
    public static string EnumToString(T enumValue)
    {
        return enumValue switch
        {
            GroupName group => ConvertGroupName(group),
            SubjectType type => ConvertSubjectType(type),
            TeacherPosition position => ConvertTeacherPosition(position),
            _ => throw new ArgumentException($"Unsupported enum type: {typeof(T)}"),
        };
    }

    private static string ConvertGroupName(GroupName group)
    {
        return group switch
        {
            GroupName.CS31 => "КН-31",
            _ => group.ToString()
        };
    }

    private static string ConvertSubjectType(SubjectType type)
    {
        return type switch
        {
            SubjectType.Lecture => "Лекція",
            SubjectType.Practice => "Практика",
            _ => type.ToString()
        };
    }

    private static string ConvertTeacherPosition(TeacherPosition position)
    {
        return position switch
        {
            TeacherPosition.Assistant => "Асистент",
            TeacherPosition.SeniorLecturer => "Старший викладач",
            TeacherPosition.AssociateProfessor => "Доцент",
            TeacherPosition.Professor => "Професор",
            _ => position.ToString()
        };
    }
}
