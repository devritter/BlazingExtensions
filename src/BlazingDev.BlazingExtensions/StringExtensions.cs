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

    /// <summary>
    /// searches for the "subString" at the start of the "mainString" using OrdinalIgnoreCase comparison
    /// </summary>
    public static bool StartsWithIgnoreCase(this string mainString, string subString)
    {
        return mainString.StartsWith(subString, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// searches for the "subString" at the end of the "mainString" using OrdinalIgnoreCase comparison
    /// </summary>
    public static bool EndsWithIgnoreCase(this string mainString, string subString)
    {
        return mainString.EndsWith(subString, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Trims away the "trimText" from the start of the "input" string. If it occurs multiple times, it is removed multiple times.
    /// Possibility to also pre- and post-trim white-spaces from the "input" string
    /// </summary>
    /// <param name="input">The string which potentially contains unwanted start text</param>
    /// <param name="trimText">The text which should be removed</param>
    /// <param name="trimWhiteSpacesFromStart">true if starting white-spaces should also be removed from the original and then trimmed input string</param>
    /// <returns></returns>
    public static string TrimStart(this string input, string trimText, bool trimWhiteSpacesFromStart = false)
    {
        if (trimWhiteSpacesFromStart)
        {
            input = input.TrimStart();
        }

        if (trimText == "")
        {
            // native .StartsWith returns true, would end in endless loop
            return input;
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

    /// <summary>
    /// Trims away the "trimText" from the end of the "input" string. If it occurs multiple times, it is removed multiple times.
    /// </summary>
    /// <param name="input">The string which potentially contains unwanted start text</param>
    /// <param name="trimText">The text which should be removed</param>
    /// <returns></returns>
    public static string TrimEnd(this string input, string trimText)
    {
        if (trimText == "")
        {
            // native .EndsWith returns true, would end in endless loop
            return input;
        }
        
        while (input.EndsWith(trimText))
        {
            var lengthTrimmed = input.Length - trimText.Length;
            input = input.Substring(0, lengthTrimmed);
        }

        return input;
    }
}