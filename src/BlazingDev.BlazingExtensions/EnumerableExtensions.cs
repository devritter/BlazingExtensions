using System.Collections.Generic;
using System.Linq;

namespace BlazingDev.BlazingExtensions;

public static class EnumerableExtensions
{
    /// <summary>
    /// returns true if the "enumerable" is NOT NULL and FILLED with something
    /// </summary>
    public static bool SafeAny<T>(this IEnumerable<T>? enumerable)
    {
        return enumerable != null && 
               enumerable.Any();
    }

    /// <summary>
    /// returns true if the "enumerable" is null or empty (count = 0)
    /// </summary>
    public static bool IsEmpty<T>(this IEnumerable<T>? enumerable)
    {
        return enumerable == null ||
               !enumerable.Any();
    }
}