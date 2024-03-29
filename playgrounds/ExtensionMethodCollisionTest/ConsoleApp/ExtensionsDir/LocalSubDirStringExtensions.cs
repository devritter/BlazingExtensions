namespace ConsoleApp.ExtensionsDir;

public static class LocalSubDirStringExtensions
{
    public static string DoSomething(this string input)
    {
        return $"Did something with string '{input}' as defined in LocalSubDirStringExtensions";
    }
}