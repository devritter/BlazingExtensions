using System;
using System.Diagnostics.CodeAnalysis;

namespace BlazingDev.BlazingExtensions
{
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
    }
}