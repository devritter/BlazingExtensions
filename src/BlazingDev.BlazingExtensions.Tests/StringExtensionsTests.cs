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
    public void Fallback_ReturnsEmptyString_IfMainAndFallbackStringIsUseless()
    {
        Assert.Equal("", ((string?)null).Fallback(null));
        Assert.Equal("", "".Fallback(null));
        Assert.Equal("", "  ".Fallback(null));
    }

    [Theory]
    [InlineData("hello world", "hello world", true)]
    [InlineData("hello world", "hello", true)]
    [InlineData("hello world", "world", true)]
    [InlineData("hello world", "lo wor", true)]
    [InlineData("hello world", "o", true)]
    [InlineData("hello world", "lowor", false)]
    [InlineData("hello world", "world!", false)]
    // should we allow null values?
    public void ContainsIgnoreCase(string longString, string subString, bool expect)
    {
        Assert.Equal(expect, longString.ContainsIgnoreCase(subString));
        
        Assert.Equal(expect, longString.ContainsIgnoreCase(subString.ToUpper()));
        Assert.Equal(expect, longString.ToUpper().ContainsIgnoreCase(subString));
        Assert.Equal(expect, longString.ToUpper().ContainsIgnoreCase(subString.ToUpper()));
        Assert.Equal(expect, longString.ToUpper().ContainsIgnoreCase(subString.ToLower()));
        
        Assert.Equal(expect, longString.ContainsIgnoreCase(subString.ToLower()));
        Assert.Equal(expect, longString.ToLower().ContainsIgnoreCase(subString));
        Assert.Equal(expect, longString.ToLower().ContainsIgnoreCase(subString.ToLower()));
        Assert.Equal(expect, longString.ToLower().ContainsIgnoreCase(subString.ToUpper()));
    }
}