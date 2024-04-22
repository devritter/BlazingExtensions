using System;
using System.Collections.Generic;

namespace BlazingDev.BlazingExtensions;

public static class BzTypeX
{
    private static readonly HashSet<Type> NumericTypes =
    [
        typeof(byte),
        typeof(sbyte),
        typeof(short),
        typeof(ushort),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),
        typeof(float),
        typeof(double),
        typeof(decimal)
    ];

    /// <summary>
    /// returns true if the given "type" is byte, sbyte, short, ushort, int, uint, long, ulong, float, double, decimal.
    /// </summary>
    /// <param name="type">type to check</param>
    public static bool IsNumeric(this Type type)
    {
        return NumericTypes.Contains(type);
    }

    /// <summary>
    /// If the given type is a nullable type, it returns the underlying type. Otherwise just returns the given type.
    /// </summary>
    /// <param name="type">potential nullable type</param>
    public static Type UnwrapNullable(this Type type)
    {
        return Nullable.GetUnderlyingType(type) ?? type;
    }
}
