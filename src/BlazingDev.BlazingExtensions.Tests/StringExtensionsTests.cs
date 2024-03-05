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
    [InlineData("   ", " ", "")] // space removes multiple spaces
    [InlineData("     ", "  ", " ")] // 2-space vs 5-space = 1-space
    [InlineData(" ", "", " ")] // no trimming at all (but also no endless loop)
    [InlineData("", " ", "")] // useless input, remains
    [InlineData("", "", "")] // noting to do
    [InlineData(" ", "\t", " ")] // wrong trim string -> no trimming
    [InlineData("hello", "", "hello")]
    [InlineData("hello", " ", "hello")]
    [InlineData("", "hello", "")]
    [InlineData("hello", "hello", "")] // full trimming
    [InlineData("hellohello", "hello", "")] // multiple full trimming
    public void TrimStart_TrimEnd_HandleEdgeCasesProperly(string input, string trimValue, string expectedOutput)
    {
        input.TrimStart(trimValue).Should().Be(expectedOutput);
        input.TrimEnd(trimValue).Should().Be(expectedOutput);
    }

    [Theory]
    // normal cases
    [InlineData("info:job done", "info:", "job done")] // expected behavior
    [InlineData("info:job done   ", "info:", "job done   ")] // end spaces remain
    [InlineData("info:info:job done", "info:", "job done")] // multiple trimming
    [InlineData("info:info:job done   ", "info:", "job done   ")] // end spaces remain
    [InlineData("info: job done", "info:", " job done")] // space remains
    // no trimming
    [InlineData("info:job done", "warn:", "info:job done")] // no replace if start text is different
    [InlineData(" info:job done", "info:", " info:job done")] // expected behavior
    [InlineData("info:job done", "INFO:", "info:job done")] // different casing -> no trimming
    // todo what to do with null?
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

    [Theory]
    [InlineData("file1.txt", ".txt", "file1")]
    [InlineData("   file1.txt", ".txt", "   file1")] // start spaces remain
    [InlineData("FILE1.txt", ".txt", "FILE1")] // casing remains
    [InlineData("file1 .txt", ".txt", "file1 ")] // space remains
    [InlineData("file1.txt", ".mp3", "file1.txt")] // no match, no trimming
    [InlineData("file1.txt.txt", ".txt", "file1")] // multiple trimming
    [InlineData("file1.TXT", ".txt", "file1.TXT")] // different casing, no trimming
    public void TrimEnd(string fullString, string trimValue, string expectedOutput)
    {
        fullString.TrimEnd(trimValue).Should().Be(expectedOutput);
    }
}