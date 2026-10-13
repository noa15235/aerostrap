using System;

namespace Wpf.Ui.Extensions;

public static class DateTimeExtensions
{
    public static long GetTimestamp(this DateTime dateTime)
    {
        return (long)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }

    public static long GetMillisTimestamp(this DateTime dateTime)
    {
        return (long)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
    }

    public static long GetMicroTimestamp(this DateTime dateTime)
    {
        return (long)dateTime.Subtract(new DateTime(1970, 1, 1)).Ticks / (TimeSpan.TicksPerMillisecond / 1000);
    }
}
