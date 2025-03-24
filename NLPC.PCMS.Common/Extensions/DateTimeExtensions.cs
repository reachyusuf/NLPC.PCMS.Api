namespace NLPC.PCMS.Common.Extensions
{
    public static class DateTimeExtensions
    {
        //public static DateTime ToNigeriaDateTime(this DateTime dt)
        //{
        //    TimeZoneInfo ngrTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
        //    dt = TimeZoneInfo.ConvertTimeFromUtc(dt, ngrTimeZone);
        //    return dt;
        //}

        public static DateTime ToNigeriaDateTime(this DateTime dt)
        {
            // Define the Nigeria time zone
            TimeZoneInfo ngrTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");

            // Ensure the DateTime is treated as UTC if the Kind is unspecified or Local
            if (dt.Kind == DateTimeKind.Unspecified)
            {
                // Treat the DateTime as UTC if the Kind is unspecified (legacy data, etc.)
                dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            }
            else if (dt.Kind == DateTimeKind.Local)
            {
                // Convert Local time to UTC before converting to Nigeria time
                dt = dt.ToUniversalTime();
            }

            // Convert the UTC time to Nigeria time zone
            return TimeZoneInfo.ConvertTimeFromUtc(dt, ngrTimeZone);
        }
    }
}
