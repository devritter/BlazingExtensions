using System.Globalization;

namespace BlazingDev.BlazingExtensions;

public static class BzDoubleX
{
    public static string ToInvariantString(this double value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }
}