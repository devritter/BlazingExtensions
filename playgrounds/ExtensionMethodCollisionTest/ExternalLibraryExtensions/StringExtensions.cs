namespace ExternalLibraryExtensions;

public static class StringExtensions
{
    public static string DoSomething(this string input)
    {
        return $"Did something with string '{input}' as defined in ExternalLibraryExtensions.csproj";
    }

    public static string DoSomethingOnlyTheExternalLibraryCan(this string input)
    {
        return $"This is really special, I put '{input}' into single quotes!";
    }
}
