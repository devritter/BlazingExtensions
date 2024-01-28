namespace BlazingDev.BlazingExtensions.Tests;

public class StringExtensionsTests
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("\t", false)]
    [InlineData("\n", false)]
    [InlineData("   \t   \n  ", false)]
    [InlineData("x", true)]
    [InlineData("    x\t", true)]
    public void HasText(string? input, bool expected)
    {
        Assert.Equal(expected, input.HasText());
    }

    [Theory]
    [InlineData("hello")]
    [InlineData("  x  ")]
    [InlineData("_")]
    public void Fallback_UsesMainValue_IfUseful(string input)
    {
        Assert.Equal(input, input.Fallback("fallback"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData(" \t \n ")]
    public void Fallback_UsesFallbackValue_IfMainValueHasNoText(string input)
    {
        Assert.Equal("fallback", input.Fallback("fallback"));
        Assert.Equal("\t\t", input.Fallback("\t\t"));
        // fallback string is retained even if it has not really useful text
    }

    [Fact]
    public void ReturnsEmptyString_IfMainAndFallbackStringIsUseless()
    {
        Assert.Equal("", ((string?)null).Fallback(null));
        Assert.Equal("", "".Fallback(null));
        Assert.Equal("", "  ".Fallback(null));
    }
}