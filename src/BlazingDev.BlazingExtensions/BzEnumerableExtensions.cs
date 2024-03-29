using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BlazingDev.BlazingExtensions;

public static class BzEnumerableExtensions
{
    /// <summary>
    /// returns true if the "enumerable" is NOT NULL and has at least one item
    /// </summary>
    public static bool HasContent<T>([NotNullWhen(true)] this IEnumerable<T>? enumerable)
    {
        return enumerable != null &&
               enumerable.Any();
    }

    /// <summary>
    /// Returns the "items" joined together separated by the "separator"
    /// </summary>
    public static string StringJoin(this IEnumerable<string?>? items, string separator)
    {
        if (items == null)
        {
            return "";
        }

        return string.Join(separator, items);
    }

    public static IOrderedEnumerable<T> OrderByFirstWhere<T>(this IEnumerable<T> items, Predicate<T> predicate)
    {
        return items.OrderBy(x => predicate(x) ? 0 : 1);
    }

    public static IOrderedEnumerable<T> ThenByFirstWhere<T>(this IOrderedEnumerable<T> items, Predicate<T> predicate)
    {
        return items.ThenBy(x => predicate(x) ? 0 : 1);
    }

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> items)
        where T : class
    {
        return items.Where(x => x != null)!;
    }

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> items)
        where T : struct
    {
        return items.Where(x => x.HasValue).Select(x => x!.Value);
    }
}