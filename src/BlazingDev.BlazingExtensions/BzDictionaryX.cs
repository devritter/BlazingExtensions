using System;
using System.Collections.Generic;

namespace BlazingDev.BlazingExtensions;

public static class BzDictionaryX
{
    public static int GetCount<TKey>(this IReadOnlyDictionary<TKey, int> dictionary, TKey key)
    {
        return dictionary.GetValueOrDefault(key);
    }

    public static int IncrementCount<TKey>(this IDictionary<TKey, int> dictionary, TKey key, int increment = 1)
    {
        if (dictionary.TryAdd(key, increment))
        {
            return increment;
        }

        // already part of dictionary
        {
            var value = dictionary[key] + increment;
            dictionary[key] = value;
            return value;
        }
    }

    public static int DecrementCount<TKey>(this IDictionary<TKey, int> dictionary, TKey key, int decrement = 1)
    {
        // Stryker disable once Arithmetic : *-1 will be replace by /-1 which also works
        return dictionary.IncrementCount(key, decrement * -1);
    }
}