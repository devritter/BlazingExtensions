using System;
using System.ComponentModel;
using System.Reflection;

namespace BlazingDev.BlazingExtensions;

public static class BzEnumExtensions
{
    /// <summary>
    /// returns the value of the [Description("...")] attribute and falls back to simple .ToString()
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetDescription(this Enum value)
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
}