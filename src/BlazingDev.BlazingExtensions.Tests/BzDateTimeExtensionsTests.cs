namespace BlazingDev.BlazingExtensions.Tests;

public class BzDateTimeExtensionsTests
{
    [Fact]
    public void ToMidnight()
    {
        var expected = new DateTime(2024, 04, 29, 23, 59, 59, 999);
        // start from 00:00:00
        expected.Date.ToMidnight().Should().Be(expected);
        // already at midnight should stay at midnight
        expected.ToMidnight().Should().Be(expected);
        // some time within the day should not care
        expected.Date.AddHours(3).ToMidnight().Should().Be(expected);
    }

    [Fact]
    public void ToStartOfMonth()
    {
        var expected = new DateTime(2024, 03, 01);
        // already there should stay there
        expected.ToStartOfMonth().Should().Be(expected);
        // middle of month should work
        expected.AddDays(10).ToStartOfMonth().Should().Be(expected);
        // end of month should work
        expected.AddMonths(1).AddDays(-1).ToStartOfMonth().Should().Be(expected);
        // time is cleared
        expected.AddDays(5).AddHours(3).AddSeconds(2).ToStartOfMonth().Should().Be(expected);
    }

    [Theory]
    [InlineData(2024, 02, 29)] // leap year's february
    [InlineData(2024, 03, 31)] // normal march 
    public void ToEndOfMonth(int year, int month, int day)
    {
        var expected = new DateTime(year, month, day, 23, 59, 59, 999);
        // already there should stay there
        expected.ToEndOfMonth().Should().Be(expected);
        // middle of month should work
        expected.AddDays(-10).ToEndOfMonth().Should().Be(expected);
        // start of month should work
        new DateTime(year, month, day).ToEndOfMonth().Should().Be(expected);
        // time is cleared
        expected.AddDays(-5).AddHours(3).AddSeconds(2).ToEndOfMonth().Should().Be(expected);
    }

    [Theory]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Utc)]
    [InlineData(DateTimeKind.Unspecified)]
    public void ExtensionMethodsKeepDateTimeKind(DateTimeKind kind)
    {
        var now = DateTime.SpecifyKind(DateTime.Now, kind);
        now.ToMidnight().Kind.Should().Be(kind);
        now.ToStartOfMonth().Kind.Should().Be(kind);
        now.ToEndOfMonth().Kind.Should().Be(kind);
    }

    [Theory]
    [InlineData("2024-03-29T21:11:25.772", 1711746685772)] // Unspecified should be handled as UTC
    [InlineData("2024-03-29T21:11:25.772Z", 1711746685772)] // UTC
    [InlineData("2024-03-29T22:11:25.772+01:00", 1711746685772)] // Local
    public void ToJsTicks(string dateTimeString, long expectedTicks)
    {
        DateTime.Parse(dateTimeString).ToJsTicks().Should().Be(expectedTicks);
    }
}