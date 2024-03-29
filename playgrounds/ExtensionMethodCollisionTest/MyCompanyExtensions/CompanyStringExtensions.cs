namespace MyCompanyExtensions;

public static class CompanyStringExtensions
{
    public static string DoSomething(this string input)
    {
        return $"Did something with string '{input}' as defined in MyCompanyExtensions.csproj";
    }
}
