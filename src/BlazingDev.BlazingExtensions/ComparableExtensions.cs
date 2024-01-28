using System;

namespace BlazingDev.BlazingExtensions;

public static class ComparableExtensions
{
    public static T LimitTo<T>(this T value, T? minValue, T? maxValue)
        where T : struct, IComparable
    {
        if (minValue != null &&
            maxValue != null &&
            minValue.Value.CompareTo(maxValue.Value) == 1)
        {
            throw new ArgumentException($"MinValue ({minValue}) is greater than MaxValue ({maxValue})!");
        }
        
        if (minValue != null &&
            value.CompareTo(minValue) == -1)
        {
            return minValue.Value;
        }

        if (maxValue != null &&
            value.CompareTo(maxValue) == 1)
        {
            return maxValue.Value;
        }
        
        return value;
    }
}