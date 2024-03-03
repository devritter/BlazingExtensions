using System;
using System.Diagnostics.CodeAnalysis;

namespace BlazingDev.BlazingExtensions;

public static class StringExtensions
{
    /// <summary>
    /// is the opposite of "string.IsNullOrWhiteSpace". <br/> 
    /// null, empty string, whitespaces (like spaces, tabs, newlines) are NOT considered useful text and the method will return false.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool HasText([NotNullWhen(true)] this string? input)
    {
        return !string.IsNullOrWhiteSpace(input);
    }

    /// <summary>
    /// Returns the fallback text if the "input" parameter has no text (see <see cref="HasText">HasText()</see> extension). <br/>
    /// Returns empty string if "input" has no text and "fallbackText" is null.
    /// </summary>
    /// <param name="input">main value to use if useful</param>
    /// <param name="fallbackText">alternative text if "input" has no text. Falls back to empty string if null is passed.</param>
    /// <returns></returns>
    public static string Fallback(this string? input, string? fallbackText)
    {
        if (input.HasText())
        {
            return input;
        }

        return fallbackText ?? "";
    }

    /// <summary>
    /// searches for the "subString" inside the "mainString" using OrdinalIgnoreCase comparison
    /// </summary>
    public static bool ContainsIgnoreCase(this string mainString, string subString)
    {
        return mainString.Contains(subString, StringComparison.OrdinalIgnoreCase);
    }

    public static string TrimStart(this string input, string trimText, bool trimWhiteSpacesFromStart = false)
    {
        if (trimWhiteSpacesFromStart)
        {
            input = input.TrimStart();
        }
        
        while (input.StartsWith(trimText))
        {
            input = input.Substring(trimText.Length);
            if (trimWhiteSpacesFromStart)
            {
                input = input.TrimStart();
            }
        }

        return input;
    }
}