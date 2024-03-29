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

    /// <summary>
    /// Returns the JavaScript ticks for a given DateTime. <br />
    /// If the DateTimeKind is Local, the value is converted to UTC before calculating the JS ticks.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static long ToJsTicks(this DateTime value)
    {
        // JS ticks are in UTC, so we need to verify that "value" is also UTC
        // let's interpret "Unspecified" as "UTC" as this information is lost through EntityFramework when using DateTime as column type
        if (value.Kind == DateTimeKind.Local)
        {
            value = value.ToUniversalTime();
        }

        var diff = value - DateTime.UnixEpoch;
        return (long)diff.TotalMilliseconds;
    }

    /// <summary>
    /// Specifies the given kind on a given DateTime if the DateTime's kind is currently "Unspecified". <br />
    /// This is different to calling ".ToUniversalTime()" or ".ToLocalTime()" because there the framework assumes that the
    /// DateTime is currently in the opposite kind and applies the timezone offset.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static DateTime IfUndefinedSetKind(this DateTime value, DateTimeKind kind)
    {
        if (value.Kind == DateTimeKind.Unspecified)
        {
            return DateTime.SpecifyKind(value, kind);
        }

        return value;
    }
}