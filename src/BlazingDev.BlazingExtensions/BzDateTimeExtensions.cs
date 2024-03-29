using System;

namespace BlazingDev.BlazingExtensions;

public static class BzDateTimeExtensions
{
    /// <summary>
    /// Moves the DateTime's time portion to 23:59:59.999
    /// </summary>
    public static DateTime ToMidnight(this DateTime value)
    {
        return value.Date.AddDays(1).AddMilliseconds(-1);
    }

    /// <summary>
    /// Moves a given DateTime to the first day of its month and sets the time to 00:00:00.000
    /// </summary>
    public static DateTime ToStartOfMonth(this DateTime value)
    {
        var minusDays = value.Day - 1;
        return value.Date.AddDays(-minusDays);
    }

    /// <summary>
    /// Moves a given DateTime to the last day of its month and sets the time to 23:59:59.999
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime ToEndOfMonth(this DateTime value)
    {
        return value.ToStartOfMonth().AddMonths(1).AddMilliseconds(-1);
    }
}