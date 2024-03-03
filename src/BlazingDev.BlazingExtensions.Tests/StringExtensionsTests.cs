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

    [Theory]
    [InlineData("info:job done", "info:", "job done")] // expected behavior
    [InlineData("info:job done   ", "info:", "job done   ")] // end spaces remain
    [InlineData("info:info:job done", "info:", "job done")] // multiple trimming
    [InlineData("info:info:job done   ", "info:", "job done   ")] // end spaces remain
    [InlineData("info: job done", "info:", " job done")] // space remains
    // no trimming
    [InlineData("info:job done", "warn:", "info:job done")] // no replace if start text is different
    [InlineData(" info:job done", "info:", " info:job done")] // expected behavior
    [InlineData("info:job done", "INFO:", "info:job done")] // different casing -> no trimming
    public void TrimStart(string fullString, string trimValue, string expectedOutput)
    {
        fullString.TrimStart(trimValue).Should().Be(expectedOutput);
    }

    [Theory]
    [InlineData("info:job done", "info:", "job done")] // same behavior like without whitespace removal
    [InlineData("info:info:job done", "info:", "job done")] // same behavior
    [InlineData("info: job done", "info:", "job done")] // whitespace removed
    [InlineData(" info: job done", "info:", "job done")] // leading whitespace removed
    [InlineData(" info: job done   ", "info:", "job done   ")] // leading whitespace removed, trailing remains
    [InlineData(" info: info: job done", "info:", "job done")] // multiple whitespace removed
    [InlineData(" info:\tinfo:\ninfo: job done", "info:", "job done")] // different whitespaces removed
    // no trimming
    [InlineData("info:job done", "warn:", "info:job done")] // no replace if start text is different
    [InlineData("info:job done", "INFO:", "info:job done")] // different casing -> no trimming
    public void TrimStart_WithWhitespaceRemoval(string fullString, string trimValue, string expectedOutput)
    {
        fullString.TrimStart(trimValue, true).Should().Be(expectedOutput);
    }
}