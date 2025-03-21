using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazingDev.BlazingExtensions;

public static class BzListX
{
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