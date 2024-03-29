using System;

namespace BlazingDev.BlazingExtensions;

public static class BzDateTimeExtensions
{
    /// <summary>
    /// moves the DateTime to 23:59:59.999
    /// </summary>
    public static DateTime ToMidnight(this DateTime value)
    {
        return value.Date.AddDays(1).AddMilliseconds(-1);
    }
}