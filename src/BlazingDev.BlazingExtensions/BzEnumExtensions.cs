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

    /// <summary>
    /// Removes the "flagToRemove" from the "value". <br />
    /// Info! Don't expect too much performance when calling this method very frequently as a lot of converting is needed! 
    /// </summary>
    public static T RemoveFlag<T>(this T value, T flagToRemove) where T : struct, Enum
    {
        if (value.HasFlag(flagToRemove))
        {
            var originalValue = Convert.ToInt64(value);
            var flagValue = Convert.ToInt64(flagToRemove);
            var result = originalValue & ~flagValue;
            return (T)Convert.ChangeType(result, typeof(T).GetEnumUnderlyingType());
        }

        return value;
    }

    /// <summary>
    /// Adds the "flagToAdd" to the "value". <br />
    /// Info! Don't expect too much performance when calling this method very frequently as a lot of converting is needed! 
    /// </summary>
    public static T AddFlag<T>(this T value, T flagToAdd) where T : struct, Enum
    {
        if (!value.HasFlag(flagToAdd))
        {
            var originalValue = Convert.ToInt64(value);
            var flagValue = Convert.ToInt64(flagToAdd);
            var result = originalValue | flagValue;
            return (T)Convert.ChangeType(result, typeof(T).GetEnumUnderlyingType());
        }

        return value;
    }

    /// <summary>
    /// Adds or removes the "flag" to/from the "value". <br />
    /// Info! Don't expect too much performance when calling this method very frequently as a lot of converting is needed! 
    /// </summary>
    /// <param name="set">True if the "flag" should be added. False if the "flag" should be removed.</param>
    public static T SetFlag<T>(this T value, T flag, bool set) where T : struct, Enum
    {
        return set ? value.AddFlag(flag) : value.RemoveFlag(flag);
    }
}