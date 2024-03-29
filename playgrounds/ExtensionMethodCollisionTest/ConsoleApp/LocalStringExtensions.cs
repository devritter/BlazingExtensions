namespace ConsoleApp;

public static class LocalStringExtensions
{
    public static string DoSomethingX(this string input)
    {
        return $"Did something with string '{input}' as defined in LocalStringExtensions";
    }
}