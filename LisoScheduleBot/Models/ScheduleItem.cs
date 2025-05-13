namespace LisoScheduleBot.Models
{
    public class ScheduleItem
    {
        public string? Subject { get; set; }
        public string? Teacher { get; set; }
        public string? Classroom { get; set; }
        public TimeOnly StartTime { get; set; }
        public int Duration { get; set; }
    }
}
