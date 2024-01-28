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
}