using System;
using System.ComponentModel;
using System.Reflection;

namespace BlazingDev.BlazingExtensions;

public static class BzEnumExtensions
{
    /// <summary>
    /// Returns the value of the [Description("...")] attribute and falls back to simple .ToString()
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetDescription(this Enum value)
    {
        return GetDescriptionCore(value);
    }

    private static string GetDescriptionCore(this object value)
    {
        var fieldName = value.ToString();
        var fieldInfo = value.GetType().GetField(fieldName);
        if (fieldInfo == null)
        {
            // maybe some (MyEnum)someInt cast
            return fieldName;
        }

        var descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
        if (descriptionAttribute != null)
        {
            return descriptionAttribute.Description;
        }

        return fieldName;
        // DisplayName attribute is not relevant as it can't be placed on enum values
    }

    /// <summary>
    /// Parses the given "text" to the desired "enumType". Also checks for [Description] attribute matches.
    /// Throws "ArgumentException" if parsing was not possible.
    /// </summary>
    /// <param name="enumType">desired enum type</param>
    /// <param name="text">text which represents Enum values directly or the [Description] attribute value</param>
    public static object Parse(Type enumType, string text)
    {
        if (Enum.TryParse(enumType, text, out var parsed))
        {
            return parsed;
        }

        var enumValues = Enum.GetValues(enumType);
        foreach (var enumValue in enumValues)
        {
            var description = GetDescriptionCore(enumValue);
            if (text == description)
            {
                return enumValue;
            }
        }

        // just call Enum.Parse which will throw a good exception
        return Enum.Parse(enumType, text);
    }

    /// <summary>
    /// Parses the given "text" to the desired "TEnum". Also checks for [Description] attribute matches.
    /// Throws "ArgumentException" if parsing was not possible.
    /// </summary>
    /// <param name="text">text which represents Enum values directly or the [Description] attribute value</param>
    public static TEnum Parse<TEnum>(string text) where TEnum : struct, Enum
    {
        return (TEnum)Parse(typeof(TEnum), text);
    }

    public static T RemoveFlag<T>(this T value, T flagToRemove) where T : struct, Enum
    {
        if (value.HasFlag(flagToRemove))
        {
            var originalValue = (int)(object)value;
            var flagValue = (int)(object)flagToRemove;
            var result = originalValue & ~flagValue;
            return (T)(object)result;
        }

        return value;
    }
}