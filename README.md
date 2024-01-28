# BlazingExtensions

> Extensions for every kind of C# projects - not only for Blazor!

Hello C# developers!

Within this repo I'm collecting all the extensions that I was missing over and over in my daily work. Hopefully they will also speed up and inspire your life :)

# String extensions

## `.HasText()`

Are you happy using `string.IsNullOrEmpty(value)` all the time? Or was it `string.IsNullOrWhiteSpace(value)`?
And shouldn't you write positive code according to "Clean Code"?
Had you ever forgotten to invert the return value when using it within an `if` statement?

Clear the stage for `value.HasText()`! :tada:

What will it return?
* `null` --> false
* `""` --> false
* `"    "` (spaces) --> false
* `"\t"` (tab) --> false
* `"\n"` (newline) --> false
* `"  \t   \n   "` (very useless content) --> false
* `"x"` --> **true**

So no rocket science, actually just the inverted version of `string.IsNullOrWhiteSpace()` :shrug:


## `.Fallback(otherString)`

Imagine you want to display some short preview text for a `User` class. Would your code look as simple like this?

```csharp
public string GetPreviewText()
{
    return NickName
        .Fallback(FirstName)
        .Fallback(LastName)
        .Fallback(Email)
        .Fallback("User #" + Id);
}
```


# Numeric extensions

## `someDouble.ToInvariantString()`

Ever wanted to set some dynamic percentage to a `<div>` element? And it ran on your machine? Only your machine?

Now it will also run on machines in other countries ;)


# `IEnumerable` extensions

## `.SafeAny()`

Just prevents you from unreadable code like:
* `if (myItems?.Any() == true)`
* `if (myItems?.Any() ?? false)`
* `if ((myItems?.Any()).GetValueOrDefault())`
* `if (myItems?.Count() > 0)`


# `IComparable` extensions (double, int, TimeSpan, ...)

Ever had the need to clamp a value into a given range? Was it `Math.Max(minValue, Math.Min(maxValue, userInput))`? Looks quite difficult...

Much easier: `userInput.LimitTo(minValue, maxValue)`