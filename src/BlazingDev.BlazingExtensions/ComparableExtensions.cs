using System;

namespace BlazingDev.BlazingExtensions;

public static class ComparableExtensions
{
    /// <summary>
    /// Clamps/limits the given "value" into the inclusive boundaries defined by "minValue" and "maxValue". <br/>
    /// You can only define one boundary if you like.
    /// </summary>
    /// <param name="value">the value which gets compared/limited/clamped</param>
    /// <param name="minValue">the lower boundary</param>
    /// <param name="maxValue">the upper boundary</param>
    /// <returns>the value raised to the "minValue", lowered to the "maxValue", or as-is if it has already been inside the range</returns>
    /// <exception cref="ArgumentException">if the "minValue" is larger than the "maxValue"</exception>
    public static T Clamp<T>(this T value, T? minValue, T? maxValue)
        where T : struct, IComparable<T>
    {
        if (minValue != null &&
            maxValue != null &&
            minValue.Value.CompareTo(maxValue.Value) == 1)
        {
            throw new ArgumentException($"minValue ({minValue}) is greater than maxValue ({maxValue})!");
        }

        if (minValue != null &&
            value.CompareTo(minValue.Value) == -1)
        {
            return minValue.Value;
        }

        if (maxValue != null &&
            value.CompareTo(maxValue.Value) == 1)
        {
            return maxValue.Value;
        }

        return value;
    }

    /// <summary>
    /// Checks whether a given number is between the "lowerLimit" and the "upperLimit".
    /// If the number is equal to the "lowerLimit" or "upperLimit", "true" is returned.
    /// </summary>
    /// <param name="input">the number which should be checked</param>
    /// <param name="lowerLimit">the minimum allowed number (which is also a valid number for the check)</param>
    /// <param name="upperLimit">the maximum allowed number (which is also a valid number for the check)</param>
    /// <exception cref="ArgumentException">if the lowerLimit is greater than the upperLimit</exception>
    public static bool IsBetweenInclusive<T>(this T input, T lowerLimit, T upperLimit)
        where T : IComparable<T>
    {
        if (lowerLimit.CompareTo(upperLimit) == 1)
        {
            throw new ArgumentException($"lowerLimit ({lowerLimit}) is greater than upperLimit ({upperLimit})!");
        }

        if (input.CompareTo(lowerLimit) >= 0 && input.CompareTo(upperLimit) <= 0)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks whether a given number is between the "lowerLimit" and the "upperLimit".
    /// If the number is equal to the "lowerLimit" or "upperLimit", "true" is returned.
    /// </summary>
    /// <param name="input">the number which should be checked</param>
    /// <param name="lowerLimit">the lower value which should not be reached</param>
    /// <param name="upperLimit">the upper value which should not be reached</param>
    /// <exception cref="ArgumentException">if the lowerLimit is greater than the upperLimit</exception>
    public static bool IsBetweenExclusive<T>(this T input, T lowerLimit, T upperLimit)
        where T : IComparable<T>
    {
        if (lowerLimit.CompareTo(upperLimit) == 1)
        {
            throw new ArgumentException($"lowerLimit ({lowerLimit}) is greater than upperLimit ({upperLimit})!");
        }

        if (input.CompareTo(lowerLimit) == 1 && input.CompareTo(upperLimit) == -1)
        {
            return true;
        }

        return false;
    }
}