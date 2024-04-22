using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace BlazingDev.BlazingExtensions;

public static class BzStringX
{
    /// <summary>
    /// is the opposite of "string.IsNullOrWhiteSpace". <br/> 
    /// null, empty string, whitespaces (like spaces, tabs, newlines) are NOT considered useful content and the method will return false.
    /// In all other cases where at least one 'visible' character is part of the string, the method returns true. 
    /// </summary>
    /// <param name="input">the value to check</param>
    /// <returns></returns>
    public static bool HasContent([NotNullWhen(true)] this string? input)
    {
        return !string.IsNullOrWhiteSpace(input);
    }

    /// <summary>
    /// Returns the fallback text if the "input" parameter has no text (see <see cref="HasContent">HasText()</see> extension). <br/>
    /// Returns empty string if "input" has no text and "fallbackText" is null.
    /// </summary>
    /// <param name="input">main value to use if useful</param>
    /// <param name="fallbackText">alternative text if "input" has no text. Falls back to empty string if null is passed.</param>
    /// <returns></returns>
    public static string Fallback(this string? input, string? fallbackText)
    {
        if (input.HasContent())
        {
            return input;
        }

        return fallbackText ?? "";
    }

    /// <summary>
    /// searches for the "subString" inside the "mainString" using OrdinalIgnoreCase comparison
    /// </summary>
    public static bool EqualsIgnoreCase(this string value, string? other)
    {
        return value.Equals(other, StringComparison.OrdinalIgnoreCase);
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
    /// Trims away the first occurrence of "trimText" from the start of the "input" string. 
    /// </summary>
    /// <param name="input">The string which potentially contains unwanted start text</param>
    /// <param name="trimText">The text which should be removed</param>
    public static string TrimStartOnce(this string input, string trimText)
    {
        if (input.StartsWith(trimText))
        {
            input = input.Substring(trimText.Length);
        }

        return input;
    }

    /// <summary>
    /// Trims away the last occurrence of "trimText" from the end of the "input" string.
    /// </summary>
    /// <param name="input">The string which potentially contains unwanted start text</param>
    /// <param name="trimText">The text which should be removed</param>
    /// <returns></returns>
    public static string TrimEndOnce(this string input, string trimText)
    {
        if (input.EndsWith(trimText))
        {
            var lengthTrimmed = input.Length - trimText.Length;
            input = input.Substring(0, lengthTrimmed);
        }

        return input;
    }

    /// <summary>
    /// Truncates/shortens/clips a given text if it exceeds a given "maxLength".
    /// No additional characters are placed in case of truncating (use the "Ellipsis" extension method instead)
    /// </summary>
    /// <param name="input">the string which is potentially too long</param>
    /// <param name="maxLength">the max allowed length of the string</param>
    public static string Truncate(this string? input, int maxLength)
    {
        if (maxLength <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxLength), "must be greater than 0");
        }

        if (input == null)
        {
            return "";
        }

        // Stryker disable once equality : both < and <= work here
        if (input.Length <= maxLength)
        {
            return input;
        }

        return input.Substring(0, maxLength);
    }

    public const string DefaultEllipsisText = "…";

    /// <summary>
    /// Truncates a given text if it exceeds a given "maxLength" and appends some "ellipsisText" if the text was truncated.
    /// The "maxLengthIncludingEllipsis" must be large enough to contain the "ellipsisText".
    /// </summary>
    /// <param name="input">the string which is potentially too long</param>
    /// <param name="maxLengthIncludingEllipsis">the max desired length of the string</param>
    /// <param name="ellipsisText">the text which indicates that there is more text</param>
    public static string Ellipsis(this string? input, int maxLengthIncludingEllipsis,
        string ellipsisText = DefaultEllipsisText)
    {
        if (maxLengthIncludingEllipsis <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxLengthIncludingEllipsis), "must be greater than 0");
        }

        if (maxLengthIncludingEllipsis < ellipsisText.Length)
        {
            throw new ArgumentException($"{nameof(maxLengthIncludingEllipsis)} with value '{maxLengthIncludingEllipsis}' " +
                $"is too small for {nameof(ellipsisText)} with value '{ellipsisText}'!");
        }

        if (input == null)
        {
            return "";
        }


        // no ellipsis if the text just fits the max length window
        if (input.Length <= maxLengthIncludingEllipsis)
        {
            return input;
        }

        var maxTextLength = maxLengthIncludingEllipsis - ellipsisText.Length;
        input = input.Substring(0, maxTextLength);
        input += ellipsisText;

        return input;
    }

    /// <summary>
    /// Returns a string where "text1" is swapped with "text2" and "text2" is swapped with "text1"
    /// </summary>
    /// <param name="input">the starting point which contains the swap items</param>
    /// <param name="text1">swap item 1</param>
    /// <param name="text2">swap item 2</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">If any of the strings is null or empty</exception>
    public static string SwapText(this string input, string text1, string text2)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(text1) || string.IsNullOrEmpty(text2))
        {
            throw new ArgumentException("Input string and texts to swap cannot be null or empty.");
        }

        // the longer text should be matched first, otherwise some replacements behave unexpectedly
        // Stryker disable once equality : both < and <= work here
        if (text1.Length < text2.Length)
        {
            (text1, text2) = (text2, text1);
        }

        // Escape any special characters in the texts to avoid unexpected behavior
        string escapedText1 = Regex.Escape(text1);
        string escapedText2 = Regex.Escape(text2);

        // Create a regular expression pattern to match both texts
        string pattern = $"({escapedText1})|({escapedText2})";

        // Use a MatchEvaluator to handle replacements
        string result = Regex.Replace(input, pattern, match =>
        {
            if (match.Groups[1].Success)
            {
                return text2;
            }
            else if (match.Groups[2].Success)
            {
                return text1;
            }
            return match.Value; // Return unchanged if no match
        });

        return result;
    }

    /// <summary>
    /// A handy string split function which <br />
    /// a) removes empty entries (including white-space ones) <br />
    /// b) trims the non-empty entries
    /// </summary>
    /// <param name="input">the string you want to split</param>
    /// <param name="separator">separator character</param>
    /// <returns></returns>
    public static string[] BzSplit(this string input, char separator)
    {
        return BzSplitCore(input.Split(separator, StringSplitOptions.RemoveEmptyEntries));
    }

    /// <summary>
    /// A handy string split function which <br />
    /// a) removes empty entries (including white-space ones) <br />
    /// b) trims the non-empty entries
    /// </summary>
    /// <param name="input">the string you want to split</param>
    /// <param name="separators">separator characters</param>
    /// <returns></returns>
    public static string[] BzSplit(this string input, params char[] separators)
    {
        return BzSplitCore(input.Split(separators, StringSplitOptions.RemoveEmptyEntries));
    }

    /// <summary>
    /// A handy string split function which <br />
    /// a) removes empty entries (including white-space ones) <br />
    /// b) trims the non-empty entries
    /// </summary>
    /// <param name="input">the string you want to split</param>
    /// <param name="separator">separator string</param>
    /// <returns></returns>
    public static string[] BzSplit(this string input, string separator)
    {
        return BzSplitCore(input.Split(separator, StringSplitOptions.RemoveEmptyEntries));
    }

    /// <summary>
    /// A handy string split function which <br />
    /// a) removes empty entries (including white-space ones) <br />
    /// b) trims the non-empty entries
    /// </summary>
    /// <param name="input">the string you want to split</param>
    /// <param name="separators">separator strings</param>
    /// <returns></returns>
    public static string[] BzSplit(this string input, params string[] separators)
    {
        return BzSplitCore(input.Split(separators, StringSplitOptions.RemoveEmptyEntries));
    }

    private static string[] BzSplitCore(string[] items)
    {
        return items
            .Where(x => x.HasContent())
            .Select(x => x.Trim())
            .ToArray();
    }
}