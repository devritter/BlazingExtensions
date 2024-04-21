namespace BlazingDev.BlazingExtensions.Tests;

public class BzStringXTests
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
    public void HasContent(string? input, bool expected)
    {
        Assert.Equal(expected, input.HasContent());
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
    [InlineData("hello", "hello", true)]
    [InlineData("hello", "HELLO", true)]
    [InlineData("HELLO", "hello", true)]
    [InlineData("hello", "hello!", false)]
    [InlineData("hello!", "hello", false)]
    // special cases
    [InlineData("hello", "", false)]
    [InlineData("", "hello", false)]
    [InlineData("hello", null, false)]
    // [InlineData(null, "hello", false)] for now let's just behave like the framework method
    [InlineData("", "", true)]
    [InlineData("\t", "\t", true)]
    public void EqualsIgnoreCase(string value, string? other, bool expect)
    {
        value.EqualsIgnoreCase(other).Should().Be(expect);
        // verify that it behaves the same as the framework method
        value.Equals(other, StringComparison.OrdinalIgnoreCase).Should().Be(expect);
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

    [Theory]
    [InlineData("this is a long text", 100, "this is a long text")]
    [InlineData("this is a long text", 19, "this is a long text")]
    [InlineData("this is a long text", 18, "this is a long tex")]
    [InlineData("this is a long text", 10, "this is a")] // trim trailing space
    // auto trim
    [InlineData("   this is a long   ", 4, "this")]
    [InlineData("   this is   ", 8, "this is")]
    [InlineData("   this          is   ", 10, "this")]
    // special cases
    [InlineData(" ", 10, "")]
    [InlineData("   ", 2, "")]
    [InlineData("", 10, "")]
    [InlineData(null, 10, "")] // nobody likes nulls, so just return empty string
    // 0 = no clipping
    [InlineData("hello", 0, "hello")]
    [InlineData("  hello  ", 0, "hello")]
    [InlineData("  ", 0, "")]
    public void Truncate(string? input, int length, string output)
    {
        input.Truncate(length).Should().Be(output);
        // empty ellipsis should behave the same as Truncate()
        input.Ellipsis(length, "").Should().Be(output);
    }

    [Theory]
    [InlineData("this is a long text", 100, "this is a long text")]
    [InlineData("this is a long text", 15, "this is a long…")]
    [InlineData("hello", 5, "hello")] // ellipsis character does not make sense here
    [InlineData("hello!", 5, "hell…")]
    // auto trim
    [InlineData("     ", 3, "")]
    [InlineData("    hello  ", 6, "hello")]
    [InlineData("    hello  ", 3, "he…")]
    // special cases
    [InlineData("hello", 1, "…")] // we give at least the full ellipsis text back
    [InlineData(null, 5, "")] // nobody likes nulls, so just return empty string
    [InlineData(null, 0, "")] // nobody likes nulls, so just return empty string
    // 0 = no clipping (to have the same behavior as .Truncate() )
    [InlineData("hello", 0, "hello")] // ellipsis character does not make sense here
    public void Ellipsis(string? input, int length, string output)
    {
        input.Ellipsis(length).Should().Be(output);
    }

    [Theory]
    [InlineData("hello world", 11, "hello world")]
    [InlineData("hello world", 10, "hell[more]")]
    [InlineData("see more info", 10, "see [more]")]
    [InlineData("hello world", 7, "h[more]")]
    // auto trim
    [InlineData(" x ", 5, "x")]
    [InlineData(" hello world ", 11, "hello world")]
    [InlineData(" hello world ", 8, "he[more]")]
    // special cases
    [InlineData("hello world", 6, "[more]")] // we give at least the ellipsis back
    [InlineData("hello world", 1, "[more]")] // we give at least the ellipsis back
    [InlineData(null, 5, "")] // nobody likes nulls, so just return empty string
    // 0 = no clipping (to have the same behavior as .Truncate() )
    [InlineData("hello world", 0, "hello world")]
    [InlineData("", 0, "")]
    [InlineData("  ", 0, "")]
    [InlineData("  x  ", 0, "x")] // nobody likes nulls, so just return empty string
    [InlineData(null, 0, "")] // nobody likes nulls, so just return empty string
    public void EllipsisWithCustomText(string? input, int length, string output)
    {
        input.Ellipsis(length, "[more]").Should().Be(output);
    }

    [Theory]
    [InlineData("hello world", 11, "hello world")]
    [InlineData("hello world", 10, "hel [more]")]
    [InlineData("see more info", 10, "see [more]")]
    [InlineData("see\tmore info", 10, "see [more]")]
    [InlineData("h         d", 10, "h [more]")] // only one leading space required
    [InlineData("hello world", 7, "[more]")] // no leading space here
    [InlineData("hello world", 1, "[more]")] // and also not here because not useful
    public void EllipsisWithCustomText_PreservesLeadingSpaceFromEllipsis(string? input, int length, string output)
    {
        input.Ellipsis(length, " [more]").Should().Be(output);
    }

    [Theory]
    [InlineData("some,csv,text")] // start
    [InlineData(",,some,,,csv,,,text,,")] // remove empty entries
    [InlineData("  ,  ,some,  ,  ,csv,  ,  ,text,  ,  ")] // no content also means empty
    [InlineData("  some  ,  csv  ,  text  ")] // auto-trim
    [InlineData(" some ,  , csv ,  , text , , , \t,\n,\t\n")] // everything combined
    public void BzSplit(string input)
    {
        string[] expected = ["some", "csv", "text"];
        
        // single char separator
        input.BzSplit(',').Should().BeEquivalentTo(expected);
        
        // multi char separator
        input.BzSplit('\t', ';', ',').Should().BeEquivalentTo(expected);
        
        // single string separator
        input.BzSplit(",").Should().BeEquivalentTo(expected);
        // validate with multi-character separator
        input.Replace(",", "<sep>").BzSplit("<sep>").Should().BeEquivalentTo(expected);
        
        // multi string separator
        input.BzSplit("<sep>", "\t", ",").Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void BzSplit_SpecialCases()
    {
        // non-content input
        "".BzSplit(",").Should().BeEquivalentTo([]);
        "  ".BzSplit(",").Should().BeEquivalentTo([]);
        " , ".BzSplit(",").Should().BeEquivalentTo([]);
        
        // separator not part of input string
        "hello world".BzSplit(",").Should().BeEquivalentTo(["hello world"]);
        "hello world".BzSplit("  ").Should().BeEquivalentTo(["hello world"]);
        
        // non-content separator
        "hello".BzSplit("").Should().BeEquivalentTo(["hello"]);
        "hello".BzSplit(" ").Should().BeEquivalentTo(["hello"]);
        "  hello  ".BzSplit(" ").Should().BeEquivalentTo(["hello"]);
        
        // non-content both
        "".BzSplit("").Should().BeEquivalentTo([]);
        "  ".BzSplit("").Should().BeEquivalentTo([]);
        " \t ".BzSplit("").Should().BeEquivalentTo([]);
    }
}