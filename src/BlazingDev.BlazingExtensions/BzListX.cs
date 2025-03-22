using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazingDev.BlazingExtensions;

public static class BzListX
{
    /// <summary>
    /// Searches for items in the list, collects them in a dedicated list, and removes them from the source list.
    /// Finally returns the collected (= removed) items.
    /// </summary>
    /// <param name="list">source list where matching items will be removed</param>
    /// <param name="predicate">condition to find the items which should be removed from the source list</param>
    public static List<T> Extract<T>(this List<T> list, Func<T, bool> predicate)
    {
        var extracted = new List<T>();
        list.RemoveAll(x =>
        {
            if (predicate(x))
            {
                extracted.Add(x);
                return true;
            }

            return false;
        });
        return extracted;
    }
}