using System.Globalization;

namespace BlazingDev.BlazingExtensions.Tests;

public class DoubleExtensionsTests
{
    [Fact]
    public void ToInvariantString_ReturnsStringInInvariantCulture()
    {
        // assert that the current test runs in an environment where the default ToString() is NOT invariant
        // otherwise the implementation could be faulty and the test could still succeed
        var decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        Assert.NotEqual(".", decimalSeparator);
        
        Assert.Equal("5", 5.0.ToInvariantString());
        Assert.Equal("5.3", 5.3.ToInvariantString());
    }
}