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
* `true` for useful content like `"x"` or `" hello\n"`

So no rocket science, actually just the inverted version of `string.IsNullOrWhiteSpace()` :shrug:\
As a memory hook just think of "myStringVar HasUsefulText() ?"

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

## `.EqualsIgnoreCase(other)`

Just a shorter version of `myString.Equals(other, StringComparison.OrdinalIgnoreCase)`.

## `.ContainsIgnoreCase(subString)`

Just a shorter version of `myString.Contains(subString, StringComparison.OrdinalIgnoreCase)`.

## `.StartsWithIgnoreCase(subString)`

Just a shorter version of `myString.StartsWith(subString, StringComparison.OrdinalIgnoreCase)`.

## `.EndsWithIgnoreCase(subString)`

Just a shorter version of `myString.EndsWith(subString, StringComparison.OrdinalIgnoreCase)`.

## `.TrimStartOnce(trimValue)`

Trims substrings, not just characters.

```csharp
"info:job done".TrimStartOnce("info:"); // returns: "job done"
"info:info:job done".TrimStartOnce("info:"); // returns: "info:job done"
```

## `.TrimEndOnce(trimValue)`

Trims substrings, not just characters.

```csharp
"file1.txt".TrimEndOnce(".txt"); // returns: "file1"
```

## `.Truncate(maxLength)`

Fast and convenient way to shorten a given text, e.g. for a preview line.\
You can expect a non-`null` and trimmed string.

```csharp
"hello world".Truncate(15);                 // returns "hello world", no truncation needed
"hello beautiful world".Truncate(15);       // returns "hello beautiful"

// auto trimming of the input and output string:
"hello world".Truncate(6);                  // returns "hello"
"hello\n\t  ".Truncate(15);                 // returns "hello"
"   hello   ".Truncate(15);                 // returns "hello"
"   hello beautiful world   ".Truncate(15); // returns "hello beautiful"

// handling annoying cases
"  ".Truncate(15);                          // returns ""
null.Truncate(15);                          // returns ""

// 0 = no trimming (of the useful content)
"   ".Truncate(0);                          // returns ""
"  hello  ".Truncate(0);                    // returns "hello"
"hello".Truncate(0);                        // returns "hello"
```

## `.Ellipsis(maxLengthIncludingEllipsis, ellipsisText)`

Fast and convenient way to shorten a given text + appending ellipsis text, e.g. for a preview line.\
The default ellipsis text `…` only uses 1 character.\
You can expect a non-`null` and trimmed string.

```csharp
"hello world".Ellipsis(15);                         // returns "hello world", no ellipsis needed
"hello beautiful world".Ellipsis(15);               // returns "hello beautifu…"

// you can specify your own ellipsis text
"hello beautiful world".Ellipsis(15, " [more]");    // returns "hello be [more]"

// the ellipsis text is always returned even if the maxLength is too small.
// this is handy when your ellipsis text is localized.
"hello beautiful world".Ellipsis(2, " [more]");     // returns "[more]"

// auto trimming of the input and output string:
"hello world   ".Ellipsis(15);                      // returns "hello world"
"   hello beautiful world   ".Ellipsis(15);         // returns "hello beautifu…"

// handling annoying cases
"  ".Ellipsis(15);                                  // returns ""
null.Ellipsis(15);                                  // returns ""

// 0 = no trimming (of the useful content)
"   ".Ellipsis(0);                                  // returns ""
"  hello  ".Ellipsis(0);                            // returns "hello"
"hello".Ellipsis(0);                                // returns "hello"
```

## `.BzSplit(separator)`

Handy splitter function which also

* removes empty entries (including whitespace ones)
* trims entries

```csharp
"hello,  ,  world  ,,,".BzSplit(',')   // returns ["hello","world"]
```

## `items.StringJoin(separator)`

Convenient way to join a string together just like `string.Join(separator, items)`.\
Bonus point: allows the items list to be `null` (returns `""` then). \
Tip: to exclude `null` items INSIDE the list, use `.WhereNotNull()` (see below).

Example:

```csharp
var nextThreeSongs = playlistItems.Take(3).Select(x => x.SongName).StringJoin(", ");
```

---

# Numeric extensions

## `someDouble.ToInvariantString()`

Ever wanted to set some dynamic percentage to a `<div>` element? And it ran on your machine? Only your machine?

Now it will also run on machines in other countries ;)

---

# IComparable&lt;T&gt; extensions

> Almost every struct is `IComparable<T>`, e.g. double, int, DateTime, TimeSpan, ...

## `.Clamp(minValue, maxValue)`

Ever had the need to clamp a value into a given range? Was it `Math.Max(minValue, Math.Min(maxValue, userInput))`? Looks
quite difficult...

Much easier:

* `userInput = userInput.Clamp(minValue, maxValue);`
* `volume = volume.Clamp(0, 10);`
* `percentage = percentage.Clamp(0, 1);`
* `bookingTime = bookingTime.Clamp(options.OpeningTime, options.ClosingTime);`

It's even possible to only limit one part:

* `startPosition = startPosition.Clamp(0, null);`
* `amount = amount.Clamp(null, user.MaxAmount);`

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

# Utilities

## `BzDisposeAction`

A very simple `IDisposable` implementation which just invokes the passed action on manual `.Dispose()` call or when
reaching the end of an `using(...)` statement.

Example:

```csharp
private void HandleButtonClick()
{
    _isButtonEnabled = false;
    using(new DisposeAction(() => _isButtonEnabled = true))
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
private async Task ListenForMessages()
{
    var subscription = someService.CreateSubscription(someTopics);
    await using(new AsyncDisposeAction(() => subscription.UnsubscribeAsync()))
    {
        // busy waiting...
        // await Task.Delay...
        // await someTaskCompletionSource.Task...
    }
}
```
