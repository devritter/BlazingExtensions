---
status: accepted
date: 2024-04-21
deciders: the-blazing-dev
---

# Extension class suffix

## Context and Problem Statement

Usually for extension methods the containing class (and its name) is quite irrelevant.

But when somebody has to call the methods directly it is annoying to have long class names.

Additionally, I want the ability to pack some "not really an extension method but more an utility method" 
into the extension-method holding class because they are highly related (e.g. Enum parse).

Now the question arises: Is the `...Extensions` class/file suffix really needed?

## Considered Options

* Keep `...Extensions` suffix
* Use `...X` or `...Ex` suffix
* No suffix at all

## Decision Outcome

Chosen option: "Use `...X` or `...Ex` suffix", because it shortens the classes quite a lot and does not hurt anybody.
I personally decided to use `...X` because it is comfortable to read and `ex` is often used in `catch` statements for `Exception`s.

## Pros and Cons of the Options

### Keep `...Extensions` suffix

* Good, because well-known naming pattern
* Neutral, because not really needed, as we are in the "BlazingExtensions" library
* Bad, because long name if calling methods manually

### Use `...X` or `...Ex` suffix

* Good, because short to write if called manually
* Good, because the relation to "EX-tension" is quite obvious
* Neutral, because who never calls methods directly does not have to care anyway

### No suffix at all

* Good, because the shortest option for direct callers
* Neutral, because who never calls methods directly does not have to care anyway
* Bad, because could be confusing: is `BzString/BzDouble/BzDateTime` a new type?
