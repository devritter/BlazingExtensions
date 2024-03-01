# BlazingExtensions

> Extensions for every kind of C# projects - not only for Blazor!

Hello C# developers!

Within this repo I'm collecting all the extensions that I was missing over and over in my daily work. Hopefully they will also speed up and inspire your life :)

---
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

So no rocket science, actually just the inverted version of `string.IsNullOrWhiteSpace()` :shrug:\
As a memory hook just think of "myStringVar HasUsefulText() ?"


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


## `.ContainsIgnoreCase(subString)`
Just a shorter version of `myString.Contains(subString, StringComparison.OrdinalIgnoreCase)`.


## `items.StringJoin(separator)`
Convenient way to join a string together just like `string.Join(separator, items)`.\
Bonus point: allows the items to be `null`.

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
Almost every struct is `IComparable<T>`, e.g. double, int, DateTime, TimeSpan, ...

## `.LimitTo(minValue, maxValue)`

Ever had the need to clamp a value into a given range? Was it `Math.Max(minValue, Math.Min(maxValue, userInput))`? Looks quite difficult...

Much easier:
* `userInput = userInput.LimitTo(minValue, maxValue);`
* `volume = volume.LimitTo(0, 10);`
* `percentage = percentage.LimitTo(0, 1);`
* `bookingTime = bookingTime.LimitTo(options.OpeningTime, options.ClosingTime);`

It's even possible to only limit one part:
* `startPosition = startPosition.LimitTo(0, null);`
* `amount = amount.LimitTo(null, user.MaxAmount);`

---
# IEnumerable&lt;T&gt; extensions

## `.SafeAny()`

Returns `true` if the collection has items (and therefore is not `null`).\
Prevents you from unreadable (and potentially buggy) code like:
* `if (myItems?.Any() == true)`
* `if (myItems?.Any() != false)` (does not work for `null` collections)
* `if (myItems?.Any() ?? false)`
* `if ((myItems?.Any()).GetValueOrDefault())`
* `if (myItems?.Count() > 0)`

Note: Because I don't want to override Linq's `.Any()` but want to be null-safe, I used the `Safe` prefix.


## `.IsEmpty()`

Returns `true` if the collection is `null` or empty.\
Prevents you from unreadable (and potentially buggy) code like:
* `if (!myItems.Any())`
* `if (myItems.Any() == false)`
* `if (myItems.Any() != true)`
* `if (myItems.Count() == 0)`
* `if (myItems?.Any() == false)` (which does not work for `null` collections)
* `if (myItems?.Any() != true)`
* `if (myItems?.Count() == 0)` (which does not work for `null` collections)

Note: As there is no competing existing extension method, I had no need to use the `Safe` prefix. 

---
# Utilities

## `DisposeAction`

A very simple `IDisposable` implementation which just invokes the passed action on manual `.Dispose()` call or when reaching the end of an `using(...)` statement.

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


## `AsyncDisposeAction`

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
