// See https://aka.ms/new-console-template for more information


//using ExternalLibraryExtensions;

using MyCompanyExtensions;
using ConsoleApp.ExtensionsDir;

namespace ConsoleApp;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World! Let's do something with some string!");

        var someString = "I am some string";
        Console.WriteLine(someString.DoSomething());
        //Console.WriteLine(someString.DoSomethingOnlyTheExternalLibraryCan());
    }
}

/*
 Learnings:
 - if only one extension providing project is referenced it is quite clear that everything works
 - when only referencing another one everything still works if not both namespaces are used
 - as soon as both namespaces are "imported", the compiler complains
 - one fix is then to call some extension methods as static method invocation
 - when adding one namespace as global using and the other per-file, the compiler still complains (as expected)

 Now a real learning:
 - when the same extension method is within the same namespace as the Program class, it just wins and no compiler error is thrown
 - if the extension method is in the same csproj but in a subdirectory, the compiler warning is back

 => the extension method precedence only works within the same namespace. All other cases result in compiler warnings.
*/