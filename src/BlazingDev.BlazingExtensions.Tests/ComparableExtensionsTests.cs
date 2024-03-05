namespace BlazingDev.BlazingExtensions.Tests;

public class ComparableExtensionsTests
{
    [Theory]
    // number is within range
    [InlineData(5, 1, 10, 5)]
    [InlineData(5, 5, 10, 5)]
    [InlineData(5, 1, 5, 5)]
    [InlineData(0.5, 0, 1, 0.5)]
    [InlineData(0.5, 0.5, 1, 0.5)]
    [InlineData(0.5, 0, 0.5, 0.5)]
    // number is less than min
    [InlineData(5, 6, 10, 6)]
    [InlineData(0.5, 0.7, 1, 0.7)]
    // number is greater than max
    [InlineData(5, 1, 4, 4)]
    [InlineData(0.5, 0, 0.4, 0.4)]
    // very hard limit
    [InlineData(5, 4, 4, 4)]
    public void LimitToDouble_WithBothRangesSet(double input, double min, double max, double result)
    {
        Assert.Equal(result, input.LimitTo(min, max));
    }

    [Theory]
    [InlineData(5, 0, 5)]
    [InlineData(5, 5, 5)]
    [InlineData(5, 10, 10)]
    [InlineData(-0.5, -1, -0.5)]
    [InlineData(-0.5, -0.5, -0.5)]
    [InlineData(-0.5, -0.2, -0.2)]
    public void LimitToDouble_WithOnlyMinSet(double input, double min, double result)
    {
        Assert.Equal(result, input.LimitTo(min, null));
    }

    [Theory]
    [InlineData(5, 10, 5)]
    [InlineData(5, 5, 5)]
    [InlineData(5, 4, 4)]
    [InlineData(-0.5, 0, -0.5)]
    [InlineData(-0.5, -0.5, -0.5)]
    [InlineData(-0.5, -0.8, -0.8)]
    public void LimitToDouble_WithOnlyMaxSet(double input, double max, double result)
    {
        Assert.Equal(result, input.LimitTo(null, max));
    }

    [Fact]
    public void LimitTo_WithTimeSpan()
    {
        var ts1 = TimeSpan.FromHours(1);
        var ts2 = TimeSpan.FromHours(2);
        var ts3 = TimeSpan.FromHours(3);
        
        Assert.Equal(ts2, ts2.LimitTo(ts1, ts3));
        Assert.Equal(ts2, ts1.LimitTo(ts2, ts3));
        Assert.Equal(ts2, ts3.LimitTo(ts1, ts2));
    }

    [Fact]
    public void LimitTo_ThrowsArgumentException_IfMinAndMaxAreTwisted()
    {
        Assert.Throws<ArgumentException>(() => 5.LimitTo(10, 1));
    }

    [Theory]
    [InlineData(3,1,5, true)]
    [InlineData(3,3,5, true)]
    [InlineData(3,1,3, true)]
    [InlineData(3,3,3, true)]
    [InlineData(3,4,6, false)] // range is higher
    [InlineData(3,1,2, false)] // range is lower
    [InlineData(-7,-10,-5, true)] // negative
    [InlineData(-7,-7,-5, true)] // negative
    [InlineData(-7,-10,-7, true)] // negative
    [InlineData(-7,-3,0, false)] // negative
    public void IsBetweenInclusive_Int(int toCheck, int lower, int upper, bool expect)
    {
        toCheck.IsBetweenInclusive(lower, upper).Should().Be(expect);
    }
    
    [Theory]
    [InlineData("2000-01-05", "2000-01-01", "2000-01-10", true)]
    [InlineData("2000-01-05", "2000-01-05", "2000-01-10", true)]
    [InlineData("2000-01-05", "2000-01-01", "2000-01-05", true)]
    [InlineData("2000-01-05", "2000-01-05", "2000-01-05", true)]
    [InlineData("2000-01-05", "2000-01-04", "2000-01-04", false)]
    [InlineData("2000-01-05", "2000-01-06", "2000-01-06", false)]
    public void IsBetweenInclusive_DateTime(string toCheck, string lower, string upper, bool expect)
    {
        var toCheckDt = DateTime.Parse(toCheck);
        var lowerDt = DateTime.Parse(lower);
        var upperDt = DateTime.Parse(upper);
        toCheckDt.IsBetweenInclusive(lowerDt, upperDt).Should().Be(expect);
    }

    [Fact]
    public void IsBetweenInclusive_ThrowsException_WhenBordersAreTwisted()
    {
        Assert.Throws<ArgumentException>(() => 5.IsBetweenInclusive(10, 1));
    }

    [Theory]
    [InlineData(3,1,5, true)]
    [InlineData(3,3,5, false)]
    [InlineData(3,1,3, false)]
    [InlineData(3,3,3, false)]
    [InlineData(3,4,6, false)] // range is higher
    [InlineData(3,1,2, false)] // range is lower
    [InlineData(-7,-10,-5, true)] // negative
    [InlineData(-7,-7,-5, false)] // negative
    [InlineData(-7,-10,-7, false)] // negative
    [InlineData(-7,-3,0, false)] // negative
    public void IsBetweenExclusive_Int(int toCheck, int lower, int upper, bool expect)
    {
        toCheck.IsBetweenExclusive(lower, upper).Should().Be(expect);
    }
    
    [Theory]
    [InlineData("2000-01-05", "2000-01-01", "2000-01-10", true)]
    [InlineData("2000-01-05", "2000-01-05", "2000-01-10", false)]
    [InlineData("2000-01-05", "2000-01-01", "2000-01-05", false)]
    [InlineData("2000-01-05", "2000-01-05", "2000-01-05", false)]
    [InlineData("2000-01-05", "2000-01-04", "2000-01-04", false)]
    [InlineData("2000-01-05", "2000-01-06", "2000-01-06", false)]
    public void IsBetweenExclusive_DateTime(string toCheck, string lower, string upper, bool expect)
    {
        var toCheckDt = DateTime.Parse(toCheck);
        var lowerDt = DateTime.Parse(lower);
        var upperDt = DateTime.Parse(upper);
        toCheckDt.IsBetweenExclusive(lowerDt, upperDt).Should().Be(expect);
    }
}