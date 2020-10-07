using System;

namespace ShindaLibrary
{
    public class DateTimeTools
    {
        public static DateTime Now(int utc = 8)
        {
            var time = DateTime.UtcNow.AddHours(8);

            return time;
        }

    }
}
