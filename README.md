# BlazingExtensions

> Extensions for every kind of C# projects - not only for Blazor!

Hello C# developers!

Within this repo I'm collecting all the extensions that I was missing over and over in my daily work. Hopefully they
will also speed up and inspire your life :)

> If you have the possibility to make yourself 5% more productive and your code 5% more readable and bug-free, then you
> should definitely use it!

So let's start into your blazing future ;)

---

# String extensions

## `.HasContent()`

Are you happy using `string.IsNullOrEmpty(value)` all the time? Or was it `string.IsNullOrWhiteSpace(value)`?
And shouldn't you write positive code according to "Clean Code"?
Have you ever forgotten to invert the return value when using it within an `if` statement?

Clear the stage for `value.HasContent()`! :tada:

What will it return?

* `false` for everything useless like `null`, `""`, `" "`, `"\t"`, `"\n"`, `" \t \n "`
* `true` for **useful** content like `"x"` or `" hello\n"`

So no rocket science, actually just the positive version of `string.IsNullOrWhiteSpace()` :shrug:\

## `.Fallback(otherString)`

The `??` operator is cool but only works for `null` values. If you also want to fall back on empty / whitespace /
useless strings, use this extension. \
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

## `.EqualsIgnoreCase(other)` / `.ContainsIgnoreCase(subString)` / `.StartsWithIgnoreCase(subString)` / `.EndsWithIgnoreCase(subString)`

Just shorter versions of:
* `myString.Equals(other, StringComparison.OrdinalIgnoreCase)`
* `myString.Contains(subString, StringComparison.OrdinalIgnoreCase)` 
* `myString.StartsWith(subString, StringComparison.OrdinalIgnoreCase)` 
* `myString.EndsWith(subString, StringComparison.OrdinalIgnoreCase)`.

And because they pop up in Intellisense you get reminded to think about casing ;)

## `.TrimStartOnce(trimValue)`

Trims substrings, not just characters.

```csharp
"info:job done".TrimStartOnce("info:");      // returns: "job done"
"info:info:job done".TrimStartOnce("info:"); // returns: "info:job done"
```

## `.TrimEndOnce(trimValue)`

Trims substrings, not just characters.

```csharp
"file1.txt".TrimEndOnce(".txt"); // returns: "file1"
```

## `.Truncate(maxLength)`

Fast and convenient way to shorten a given text, e.g. for a preview line.

```csharp
"hello".Truncate(5);        // returns "hello", no truncation needed
"how are you?".Truncate(5); // returns "how a"
null.Truncate(5);           // returns "", because nobody likes nulls
```

## `.Ellipsis(maxLengthIncludingEllipsis, ellipsisText)`

Fast and convenient way to shorten a given text + appending ellipsis text, e.g. for a preview line.\
The default ellipsis text `…` only uses 1 character.

```csharp
"hello".Ellipsis(5);        // returns "hello", no ellipsis needed
"how are you?".Ellipsis(5); // returns "how …"
null.Ellipsis(5);           // returns "", because nobody likes nulls

// you can specify your own ellipsis text
"hello beautiful world".Ellipsis(15, " [more]");    // returns "hello be [more]"
```

## `.SwapText(text1, text2)`

Returns a string where `text1` is swapped with `text2` and `text2` is swapped with `text1`.

```csharp
"Anna knows Bob".SwapText("Anna", "Bob"); // returns "Bob knows Anna"

if (preferInformalGreeting)
{
    return GetFormalGreetingLine().SwapText(person.FirstName, person.LastName);
}
```

This would not work:

```csharp
"Anna knows Bob".Replace("Anna", "Bob").Replace("Bob", "Anna"); // would be "Anna knows Anna"
```

## `.BzSplit(separator)`

Handy splitter function which also

* removes empty entries (including whitespace ones)
* trims entries

```csharp
"hello,  ,  world  ,,,".BzSplit(',')   // returns ["hello","world"]
```

## `items.StringJoin(separator)`

Convenient way to join a list together just like `string.Join(separator, items)`.\
Bonus point: allows the items list to be `null` (returns `""` then). \
Tip: to exclude `null` items INSIDE the list, use `.WhereNotNull()` (see below).

Example:

```csharp
var nextThreeSongs = playlistItems.Take(3).Select(x => x.SongName).StringJoin(", ");
```

---

# Numeric extensions

## `someDouble.ToInvariantString()`

Useful when generating HTML/CSS styles and the user has a non-english culture.

```html
<progress value="@percentage.ToInvariantString()" max="1"></progress>
```

---

# DateTime extensions

## `.ToMidnight()`

Moves the `DateTime`'s time portion to `23:59:59.999`. Useful for data filtering:

```csharp
reportFrom = selectedDay.Date; // just to get sure you don't have any time portion
reportTo = reportFrom.ToMidnight(); // call the extension method upfront if you use EntityFramework
var singleDayDataQuery = dataFromDb.Where(x => x.Timestamp >= reportFrom && x.Timestamp <= reportTo);
```

## `.ToStartOfMonth()`

Moves a given `DateTime` to the first day of its month and sets the time to `00:00:00.000`. Useful for data filtering.
See code example at `.ToEndOfMonth()` below.

## `.ToEndOfMonth()`

Moves a given `DateTime` to the last day of its month and sets the time to `23:59:59.999`. Useful for data filtering:

```csharp
// imagine you need to always fetch a full month here
reportFrom = reportFrom.ToStartOfMonth(); // call the extension method upfront if you use EntityFramework
reportTo = reportFrom.ToEndOfMonth();
return dataFromDb.Where(x => x.Timestamp => reportFrom && x.Timestamp <= reportTo);
```

## `.ToJsTicks()`

Returns the JavaScript ticks for a given `DateTime`.

Info: If the `DateTimeKind` is `Local`, the value is converted to `UTC` before calculating the JS ticks.

```csharp
var xAxisValuesForSomeChart = rawData.Select(x => x.Timestamp.ToJsTicks());
```

## `.IfUndefinedSpecifyKind(kind)`

Simple way to specify the `DateTimeKind` for a `DateTime` if it's current `Kind` is `Unspecified`.

This is different from calling `.ToUniversalTime()` or `.ToLocalTime()` on `Unspecified` DateTimes because there the framework assumes that you desire a conversion and applies the timezone offset.

```csharp
// assume timestamps are UTC
someData.ForEach(x => x.Timestamp = x.Timestamp.IfUndefinedSpecifyKind(DateTimeKind.Utc));
```

---

# IComparable&lt;T&gt; extensions

Almost every struct is `IComparable<T>`, e.g. double, int, DateTime, TimeSpan, ...

## `.Clamp(minValue, maxValue)`

Ever had the need to clamp a value into a given range? Was it `Math.Max(minValue, Math.Min(maxValue, userInput))`? Looks
quite difficult...

Much easier:

```csharp
userInput = userInput.Clamp(minValue, maxValue);
volume = volume.Clamp(0, 10);
percentage = percentage.Clamp(0, 1);
bookingTime = bookingTime.Clamp(options.OpeningTime, options.ClosingTime);
```

It's even possible to only limit one part:

```csharp
startPosition = startPosition.Clamp(0, null);
amount = amount.Clamp(null, user.MaxAmount);
```

## `.IsBetweenInclusive(lowerLimit, upperLimit)`

What's the similarity between `if (value >= 10 && value <= 20)` and `if (10 <= value && value <= 20)`?\
IMHO they are both quite difficult to read. You need some time and mental effort to check if the comparison is made the
right
way.

What about:
``````if (value.IsBetweenInclusive(10, 20))``````? :)

## `.IsBetweenExclusive(lowerLimit, upperLimit)`

Same as above, but the argument numbers itself are not considered "valid".

`10.IsBetweenExclusive(10, 20) // returns false`

---

# Enum extensions

## `.GetDescription()`

Returns the value of the `[Description("...")]` attribute and falls back to simple `.ToString()`.

```csharp
public enum MyEnum
{
    Download,
    Skip,
    [Description("Download later")]
    DownloadLater
}

MyEnum.Skip.GetDescription();          // returns "Skip"
MyEnum.DownloadLater.GetDescription(); // returns "Download later"
```

## `BzEnumX.Parse<TEnum>(text)`

Parses the given `text` to the desired `TEnum`. Also checks for `[Description]` attribute matches. \
Throws `ArgumentException` if parsing was not possible.

```csharp
public enum MyEnum 
{
    FirstEntry = 0,
    [Description("Second entry")] SecondEntry = 1,
    ThirdEntry = 2   
}

// all of the following lines work:
BzEnumX.Parse<MyEnum>("SecondEntry");
BzEnumX.Parse<MyEnum>("Second entry");
BzEnumX.Parse<MyEnum>("1");
BzEnumX.Parse<MyEnum>("secondentry", ignoreCase: true);
BzEnumX.Parse<MyEnum>("second ENTRY", ignoreCase: true);
BzEnumX.Parse<MyEnum>("1", ignoreCase: true);

// and it also works for [Flags] enums!
```

## `.RemoveFlag(flagToRemove)` / `.AddFlag(flagToAdd)` / `.SetFlag(flag, set)`

Easier to read than:

* `flags = flags | MyFlagEnum.ThirdFlag`
* `flags |= MyFlagEnum.ThirdFlag`
* `flags = flags & ~MyFlagEnum.ThirdFlag`
* `flags &= ~MyFlagEnum.ThirdFlag`

```csharp
var everythingFromInstances = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
var everythingStatic = everythingFromInstances.RemoveFlag(BindingFlags.Instance).AddFlag(BindingFlags.Static);

var staticBindingFlagsWithUserSelection = everythingStatic.SetFlag(BindingFlags.NonPublic, shouldIncludePrivateMembers);
```

> Info! Don't expect too much performance when calling this method very frequently as a lot of conversion is needed!

---

# IEnumerable&lt;T&gt; extensions

## `.HasContent()`

Returns `true` if the collection has items (and therefore is not `null`).\
Prevents you from unreadable (and potentially buggy) code like:

* `if (myItems?.Any() == true)`
* `if (myItems?.Any() != false)` (does not work for `null` collections)
* `if (myItems?.Any() ?? false)`
* `if ((myItems?.Any()).GetValueOrDefault())`
* `if (myItems?.Count() > 0)` (could be slow)

Use `!myItems.HasContent()` to prevent yourself from unreadable (and potentially buggy) code like:

* `if (!myItems.Any())`
* `if (myItems.Any() == false)`
* `if (myItems.Any() != true)`
* `if (myItems.Count() == 0)`
* `if (myItems?.Any() == false)` (which does not work for `null` collections)
* `if (myItems?.Any() != true)`
* `if (myItems?.Count() == 0)` (which does not work for `null` collections and could be slow)

## `.WhereNotNull()`

Returns only non-null items with the correct nullability information so you don't get compiler warnings.

```csharp
foreach (var item in data.WhereNotNull())
{
    // no compiler warning here
    DoSomething(item.Id, item.Name);
}    
```

When used with structs, they are even unpacked!

```csharp
someData
    .Select(x => x.UpdateTimestamp)
    .WhereNotNull()
    // now direct value access is possible
    .Select(x => x.DayOfWeek) 
    // ...
```

## `.ForEach(action)`

Ever tried to call `.ForEach(...)` on an `Array`, `IEnumerable<T>` or `HashSet<T>`? Now you can!
Without calling `.ToList()` upfront!

```csharp
someItems.ForEach(x => DoSomeFurtherProcessing(x));
```

## `.OrderByFirstWhere()` and `.ThenByFirstWhere()`

Useful when you want to have some pinned / favorite / urgent items on the top of a list.

```csharp
return tasks
    .OrderByFirstWhere(x => x.Pinned)
    .ThenByFirstWhere(x => x.IsUrgent)
    .ThenBy(/* whatever you usually sort on */);
```

---

# Type extensions

## `.IsNumeric()`

returns true for numeric types: `byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `float`, `double`, `decimal`.

```csharp
typeof(int).IsNumeric(); // returns true

var numericProperties = someObject
    .GetType()
    .GetProperties()
    .Where(x => x.PropertyType.IsNumeric());
```

## `.UnwrapNullable()`

Unwraps a potential nullable type. Or just returns the input type.

```csharp
typeof(int?).UnwrapNullable();          // returns typeof(int)
typeof(int).UnwrapNullable();           // returns typeof(int)
typeof(StringBuilder).UnwrapNullable(); // returns typeof(StringBuilder)
```

---

# Utilities

## `BzDisposeAction`

A very simple `IDisposable` implementation which just invokes the passed action on manual `.Dispose()` call or when
reaching the end of an `using(...)` statement.

Example:

```csharp
private void HandleButtonClick()
{
    _isButtonEnabled = false;
    using(new BzDisposeAction(() => _isButtonEnabled = true))
    {
        DoSomethingDangerousThatMayThrowAnException();    
    }
    // the compiler creates a "finally" block here for you so the "button enabling" action will get invoked
}
```

## `BzAsyncDisposeAction`

Same as above, but for async actions/disposals. You must use the `await using(...)` statement.

Example:

```csharp
private async Task ListenForMessagesAsync()
{
    var subscription = await someService.CreateSubscriptionAsync(someTopics);
    await using(new BzAsyncDisposeAction(() => subscription.UnsubscribeAsync()))
    {
        // await Task.Delay...
        // await DoSomethingAsync()...
        // await someTaskCompletionSource.Task...
    }
}
```
