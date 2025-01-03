using System.Globalization;

namespace BlazingDev.BlazingExtensions.Tests;

public class BzDoubleXTests
{
    [Theory]
    [InlineData("de-DE")]
    [InlineData("en-US")]
    public void ToInvariantString_ReturnsStringInInvariantCulture(string cultureCode)
    {
        var culture = CultureInfo.CreateSpecificCulture(cultureCode);
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;        
        
        Assert.Equal("5", 5.0.ToInvariantString());
        Assert.Equal("5.3", 5.3.ToInvariantString());
    }
}