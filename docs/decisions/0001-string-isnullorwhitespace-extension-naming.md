---
status: accepted
date: 2024-03-15
deciders: the-blazing-dev
---

# string IsNullOrWhiteSpace extension naming

## Context and Problem Statement

This method is needed so often every day but it has following "problems":

* very long name
* you can't call it on a variable, you have to call the static method
* it is formulated in a negative way, usually you want to continue if a string is "good"
* problems can occur if you check for empty on some places and for whitespaces on other places

Therefore I need an extension which is easy to remember, easy to read and easy to understand.

## Additional context

In probably 99% you don't care if the string is `null`, empty (`""`) or white-space (`"   "`). All three cases are "
bad".

All other cases are considered "good", because they contain something useful, e.g. `"hello"`.\
This is also true for strings that contain a lot white-spaces but at least some "visible" character, e.g. `"   x   "`.

## Considered Options

* someString.HasText()
    * Good, because null+empty is definitely no text, and white-space is not useful text
    * Neutral, because somehow it sounds like "someDouble.HasNumber", what else should it carry?
* someString.IsUseful()
    * Good, because null+empty+whiteSpace is not "useful"
    * Bad, because usefulness really depends on the situation, e.g. `"invalid@mail com".IsUseful()` is misleading
* someString.IsUsable()
    * see above
* someString.IsValuable()
    * see above
* someString.IsBlank()
    * Bad, more feels like it checks only against "emptiness"
    * Bad, because the other way round, you need to invert the return value
* someString.HasData()
* someString.HasContent()
    * Good, because null+empty definitely is not "content". and you can't really expect white-spaces if this method
      returns true
    * Good, because this method could also be used for null/empty lists
    * Good, because also the opposite is possible: `.LacksContent()`
* someString.HasValue()
    * Good, because analogy to nullables``
    * Bad, because then `""` would be a value, same as default values like `0` or `false`
* someString.IsValid()
    * Bad, because validity is domain specific
    * Bad, could raise too many expectations (we don't check validation attributes)
* someString.IsGood()
    * see above
* someString.IsFilled()
    * Good, because it's quite clear that something has to be in it
    * Bad, because it sounds like some filling limit is reached, e.g. students in a course
* someString.IsPopulated()
    * Good, because it's quite clear that something has to be in it
    * Bad, because it sounds like some "process", e.g. filling a cache
* myString.IsEmptyOrWhiteSpace()
    * Bad, because too long
    * Bad, because complex to read
    * Bad, because the other way round, you need to invert the return value

## Decision Outcome

Chosen option: "someString.HasContent()", because
it is the only option with only positive arguments. A second advante is that the same name can be used for list
out best (see below)}.
