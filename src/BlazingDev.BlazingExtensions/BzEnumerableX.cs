using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BlazingDev.BlazingExtensions;

public static class BzEnumerableX
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

    /// <summary>
    /// Orders the "items" in a way that entries are on top where the "predicate" function returns true
    /// </summary>
    /// <param name="items">items that should be sorted</param>
    /// <param name="predicate">function which returns true for the entries that should be on top</param>
    public static IOrderedEnumerable<T> OrderByFirstWhere<T>(this IEnumerable<T> items, Predicate<T> predicate)
    {
        return items.OrderBy(x => predicate(x) ? 0 : 1);
    }

    /// <summary>
    /// Further orders the "items" in a way that entries are on top where the "predicate" function returns true
    /// </summary>
    /// <param name="items">items that should be sorted</param>
    /// <param name="predicate">function which returns true for the entries that should be on top</param>
    public static IOrderedEnumerable<T> ThenByFirstWhere<T>(this IOrderedEnumerable<T> items, Predicate<T> predicate)
    {
        return items.ThenBy(x => predicate(x) ? 0 : 1);
    }

    /// <summary>
    /// Filters out null items and fixes the nullability information for the compiler.
    /// </summary>
    /// <param name="items">collection with possible null items</param>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> items)
        where T : class
    {
        return items.Where(x => x != null)!;
    }

    /// <summary>
    /// Filters out nullables without a value items and unpacks the value.
    /// </summary>
    /// <param name="items">collection with possible null items</param>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> items)
        where T : struct
    {
        return items.Where(x => x.HasValue).Select(x => x!.Value);
    }

    /// <summary>
    /// Invokes the "action" with every item inside "items"
    /// </summary>
    public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
    {
        foreach (var item in items)
        {
            action(item);
        }
    }
}