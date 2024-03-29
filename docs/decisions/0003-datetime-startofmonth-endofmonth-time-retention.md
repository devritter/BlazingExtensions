---
status: accepted
date: 2024-03-29
deciders: the-blazing-dev
---

# DateTime StartOFMonth / EndOfMonth time retention

## Context and Problem Statement

I would like to introduce extension methods to shift a given `DateTime` to the beginning of a month and to the end of a
month.

The following questions have come up:

* What name should the methods have?
    * `ToFirstDayOfMonth` or `ToStartOfMonth`?
    * `ToLastDayOfMonth` or `ToEndOfMonth`?
* Should the time portion be cleared or retained?

## Possible use cases

For decision-making, it is good to have some possible use cases in mind, which could be:

* create/find a monthly-based directory for some data you have (ok, then you could also use `.ToString("yyyy-MM")`)
* group data by month
* filter day-based data: `.Where(x => x.CreationDate >= reportFilterFrom && x.CreationDate <= reportFilterTo)`. In this
  case it's
    * important that the `reportFilterFrom` has NO time portion
    * irrelevant if the `reportFilterTo` has any time portion
* filter time-series data: `.Where(x => x.Timestamp >= reportFilterFrom && x.Timestamp <= reportFilterTo)`. In this case
  it's
    * important that the `reportFilterFrom` has NO time portion
    * important that the `reportFilterTo` has a MIDNIGHT time portion

## Decision Outcome

When the method names are `ToFirstDayOfMonth` or `ToLastDayOfMonth`, users could only expect that the day portion is
modified.

But when the method names are `ToStartOfMonth` or `ToEndOfMonth`, users could expect that the time portion is moved
to `00:00:00` or `23:59:59.999` respectively. So let's just use this naming.