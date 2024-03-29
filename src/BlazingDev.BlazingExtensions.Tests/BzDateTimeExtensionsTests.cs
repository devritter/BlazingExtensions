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
}