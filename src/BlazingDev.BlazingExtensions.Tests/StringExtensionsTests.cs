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
    // edge cases
    [InlineData("hello world", "", true)] // default behavior...
    [InlineData("hello world", " ", true)]
    [InlineData("", "", true)]
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
    [InlineData("hello world", "hello world", true)]
    [InlineData("hello world", "hello", true)]
    [InlineData("hello world", "hello!", false)]
    [InlineData("hello world", "world", false)]
    [InlineData("hello world", "", true)] // default behavior...
    public void StartsWithIgnoreCase_EndsWithIgnoreCase(string mainString, string subString, bool expect)
    {
        mainString.StartsWithIgnoreCase(subString).Should().Be(expect);
        mainString.ToUpper().StartsWithIgnoreCase(subString.ToLower()).Should().Be(expect);
        mainString.ToLower().StartsWithIgnoreCase(subString.ToUpper()).Should().Be(expect);

        // the reverse strings should exactly work for .EndsWith
        var mainStringReverse = new string(mainString.Reverse().ToArray());
        var subStringReverse = new string(subString.Reverse().ToArray());
        
        mainStringReverse.EndsWithIgnoreCase(subStringReverse).Should().Be(expect);
        mainStringReverse.ToUpper().EndsWithIgnoreCase(subStringReverse.ToLower()).Should().Be(expect);
        mainStringReverse.ToLower().EndsWithIgnoreCase(subStringReverse.ToUpper()).Should().Be(expect);
    }
    
    [Theory]
    [InlineData(" ", " ", "")] // space removes space
    [InlineData("   ", " ", "  ")] // only first space may be removed
    [InlineData(" ", "  ", " ")] // 2-space not found, 1-space input returned
    [InlineData(" ", "", " ")] // no trimming at all (but also no endless loop)
    [InlineData("", " ", "")] // useless input, remains
    [InlineData("", "", "")] // noting to do
    [InlineData(" ", "\t", " ")] // wrong trim string -> no trimming
    [InlineData("hello", "", "hello")]
    [InlineData("hello", " ", "hello")]
    [InlineData("", "hello", "")]
    [InlineData("hello", "hello", "")] // full trimming
    public void TrimStartOnce_TrimEndOnce_HandleEdgeCasesProperly(string input, string trimValue, string expectedOutput)
    {
        input.TrimStartOnce(trimValue).Should().Be(expectedOutput);
        input.TrimEndOnce(trimValue).Should().Be(expectedOutput);
    }

    [Theory]
    // normal cases
    [InlineData("hellohello", "hello", "hello")] // only one trimming
    [InlineData("info:job done", "info:", "job done")] // expected behavior
    [InlineData("info:job done   ", "info:", "job done   ")] // end spaces remain
    [InlineData("info:info:job done", "info:", "info:job done")] // only trim one occurrence
    [InlineData("info:info:job done   ", "info:", "info:job done   ")] // end spaces remain
    [InlineData("info: job done", "info:", " job done")] // space remains
    // no trimming
    [InlineData("info:job done", "warn:", "info:job done")] // no replace if start text is different
    [InlineData(" info:job done", "info:", " info:job done")] // no match because leading space
    [InlineData("info:job done", "INFO:", "info:job done")] // different casing -> no trimming
    // todo what to do with null?
    public void TrimStartOnce_TrimEndOnce(string mainString, string trimValue, string expectedOutput)
    {
        mainString.TrimStartOnce(trimValue).Should().Be(expectedOutput);

        // the reverse strings should exactly work for .TrimEnd
        var mainStringReverse = new string(mainString.Reverse().ToArray());
        var trimValueReverse = new string(trimValue.Reverse().ToArray());
        var expectedOutputReverse = new string(expectedOutput.Reverse().ToArray());

        mainStringReverse.TrimEndOnce(trimValueReverse).Should().Be(expectedOutputReverse);
    }
}