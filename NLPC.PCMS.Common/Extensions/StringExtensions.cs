namespace NLPC.PCMS.Common.Extensions
{
    public static class StringExtensions
    {
        public static string GetNonEmpty(this string str, string str2, string str3 = null)
        {
            if (string.IsNullOrEmpty((str ?? "").Trim()) is false)
                return str;
            else if (string.IsNullOrEmpty((str2 ?? "").Trim()) is false)
                return str2;
            else if (string.IsNullOrEmpty((str3 ?? "").Trim()) is false)
                return str3;
            else
                return string.Empty;
        }

        public static bool IsEqualTo(this string str, string other)
        {
            return string.Equals(str, other, StringComparison.OrdinalIgnoreCase);
        }

        //public static TimeSpan ConvertCronToTimeSpan(this string cronExpression)
        //{
        //    var schedule = CrontabSchedule.Parse(cronExpression);
        //    DateTime now = DateTime.UtcNow.ToNigeriaDateTime();  // Current time
        //    DateTime nextOccurrence = schedule.GetNextOccurrence(now);  // Next occurrence based on cron

        //    // Calculate the TimeSpan between now and the next occurrence
        //    TimeSpan timeUntilNext = nextOccurrence - now;

        //    return timeUntilNext;
        //}
    }
}
