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
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("   ", true)]
    [InlineData("\t", true)]
    [InlineData("\n", true)]
    [InlineData("   \t   \n  ", true)]
    [InlineData("x", false)]
    [InlineData("    x\t", false)]
    public void LacksContent(string? input, bool expected)
    {
        Assert.Equal(expected, input.LacksContent());
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
    [InlineData("this is a long text", 17, "this is a long te")]
    [InlineData("this is a long text", 16, "this is a long t")]
    [InlineData("this is a long text", 15, "this is a long ")]
    [InlineData("this is a long text", 14, "this is a long")]
    // special cases
    [InlineData("  ", 10, "  ")]
    [InlineData("   ", 2, "  ")]
    [InlineData("", 10, "")]
    [InlineData(null, 10, "")] // nobody likes nulls, so just return empty string
    public void Truncate(string? input, int length, string output)
    {
        input.Truncate(length).Should().Be(output);
        // empty ellipsis should behave the same as Truncate()
        input.Ellipsis(length, "").Should().Be(output);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Truncate_ThrowsException_ForInvalidMaxLengths(int maxLength)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => "".Truncate(maxLength)).Message.Should().Match("*greater*0*");
    }

    [Theory]
    [InlineData("this is a long text", 100, "this is a long text")]
    [InlineData("this is a long text", 19, "this is a long text")]
    [InlineData("this is a long text", 18, "this is a long te…")]
    [InlineData("this is a long text", 17, "this is a long t…")]
    [InlineData("this is a long text", 16, "this is a long …")]
    [InlineData("this is a long text", 15, "this is a long…")]
    [InlineData("this is a long text", 14, "this is a lon…")]
    [InlineData("hello", 5, "hello")] // ellipsis character does not make sense here
    [InlineData("hello!", 5, "hell…")]
    // no trimming of strings
    [InlineData("     ", 3, "  …")]
    [InlineData("  hello  ", 6, "  hel…")]
    [InlineData("  hello  ", 3, "  …")]
    // special cases
    [InlineData("hello", 1, "…")] // we give at least the full ellipsis text back
    [InlineData(null, 5, "")] // nobody likes nulls, so just return empty string
    public void EllipsisWithDefaultText(string? input, int length, string output)
    {
        input.Ellipsis(length).Should().Be(output);
    }

    [Theory]
    [InlineData("hello world", 11, "hello world")]
    [InlineData("hello world", 10, "hell[more]")]
    [InlineData("see more info", 10, "see [more]")]
    [InlineData("hello world", 7, "h[more]")]
    [InlineData("hello world", 6, "[more]")]
    // no auto trim
    [InlineData(" x ", 6, " x ")]
    [InlineData(" hello world ", 11, " hell[more]")]
    // special cases
    [InlineData(null, 6, "")] // nobody likes nulls, so just return empty string
    public void EllipsisWithCustomText(string? input, int length, string output)
    {
        input.Ellipsis(length, "[more]").Should().Be(output);
    }

    [Fact]
    public void EllipsisThrowsException_IfMaxLengthIsShorterThanEllipsisText()
    {
        Assert.Throws<ArgumentException>(() => "".Ellipsis(1, "[more]")).Message.Should().Match("*1*too small*[more]*");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Ellipsis_ThrowsException_ForInvalidMaxLengths(int maxLength)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => "".Ellipsis(maxLength)).Message.Should().Match("*greater*0*");
    }

    [Theory]
    [InlineData("not found", "t1", "t2", "not found")]
    [InlineData("single t1 way", "t1", "t2", "single t2 way")]
    [InlineData("single t2 way", "t1", "t2", "single t1 way")]
    [InlineData("t1t2", "t1", "t2", "t2t1")]
    // both replacements are the same
    [InlineData("hello world", "hello", "hello", "hello world")]
    // only casing
    [InlineData("hello world", "hello", "HELLO", "HELLO world")]
    // multi replacements
    [InlineData("aaabbb", "a", "b", "bbbaaa")]
    // irregular match count
    [InlineData("aaabaaa", "a", "b", "bbbabbb")]
    // whitespace replacement
    [InlineData("hello\tworld", "\t", "\n", "hello\nworld")]
    // possible overlappings at start 
    // => oneone should have higher priority because it appears first in the input string
    [InlineData("oneonetwo", "oneone", "onetwo", "onetwotwo")]
    // first 6 chars should be swapped with 3 chars, and the last 3 with 6
    [InlineData("abcabcabc", "abcabc", "abc", "abcabcabc")]
    // reducing, two 6-chars-blocks found which can be replaced with 3-chars-blocks
    [InlineData("abcabcabcabc", "abcabc", "abc", "abcabc")]
    // text1 is substring of text2
    [InlineData("longer than long", "long", "longer", "long than longer")]
    public void SwapText(string input, string text1, string text2, string expected)
    {
        input.SwapText(text1, text2).Should().Be(expected);
        input.SwapText(text2, text1).Should().Be(expected);
    }

    [Theory]
    [InlineData(null, null, null)]
    [InlineData(null, "x", "x")]
    [InlineData("x", null, "x")]
    [InlineData("x", "x", null)]
    public void SwapText_ThrowsException_ForInvalidInput(string? input, string? text1, string? text2)
    {
        Assert.Throws<ArgumentException>(() =>
            input!.SwapText(text1!, text2!)).Message.Should().Match("*cannot*null*empty*");

        // empty is same useless 
        input ??= "";
        text1 ??= "";
        text2 ??= "";

        Assert.Throws<ArgumentException>(() =>
            input!.SwapText(text1!, text2!)).Message.Should().Match("*cannot*null*empty*");
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

        // non-content both
        "".BzSplit("").Should().BeEquivalentTo([]);
        "  ".BzSplit("").Should().BeEquivalentTo([]);
        " \t ".BzSplit("").Should().BeEquivalentTo([]);
    }

    [Theory]
    [InlineData("hello world", " ", "helloworld")]
    [InlineData("hello world", "o", "hell wrld")]
    [InlineData("hello world", "owrd", "hell l")]
    [InlineData("hello world", "", "hello world")] // no remove char
    [InlineData("hello world", "x", "hello world")] // no match
    [InlineData("hello world", "xyz", "hello world")] // no match - multiple
    [InlineData("hello world", "H", "hello world")] // no match - wrong case
    [InlineData("hello world", "WLD", "hello world")] // no match - wrong case multiple
    [InlineData("hello world", "Xl ", "heoword")] // partial match
    [InlineData("", "Xl ", "")] // empty input
    [InlineData(null, "Xl ", "")] // null imput
    [InlineData("hello world", "heloworld ", "")] // everything removed
    public void BzRemove_WithCharParameters(string? input, string separators, string output)
    {
        if (separators.Length == 1)
        {
            var theChar = separators[0];
            input.BzRemove(theChar).Should().Be(output);
        }
        else
        {
            var charArray = separators.ToCharArray();
            input.BzRemove(charArray).Should().Be(output);
        }
    }

    [Theory]
    [InlineData("hello PRETTY world", " ", "helloPRETTYworld")]
    [InlineData("hello PRETTY world", "o", "hell PRETTY wrld")]
    [InlineData("hello PRETTY world", "hello", " PRETTY world")]
    [InlineData("hello PRETTY world", "PRET", "hello TY world")]
    [InlineData("hello PRETTY world", "hello,world", " PRETTY ")]
    [InlineData("hello PRETTY world", "rld,wo", "hello PRETTY ")]
    [InlineData("hello PRETTY world", "T,T,o", "hell PREY wrld")]
    [InlineData("hello PRETTY world", "", "hello PRETTY world")] // no remove string
    [InlineData("hello PRETTY world", "XYZ", "hello PRETTY world")] // no match
    [InlineData("hello PRETTY world", "abc,XYZ", "hello PRETTY world")] // no match - multiple
    [InlineData("hello PRETTY world", "HELLO", "hello PRETTY world")] // no match - wrong case
    [InlineData("hello PRETTY world", "HELLO,pretty", "hello PRETTY world")] // no match - wrong case multiple
    [InlineData("hello PRETTY world", "goodnight,hello", " PRETTY world")] // partial match
    [InlineData("", "hello", "")] // empty input
    [InlineData(null, "hello,pretty ", "")] // null imput
    [InlineData("hello PRETTY world", "hello PRETTY world", "")] // everything removed in 1 go
    [InlineData("hello PRETTY world", "hello,world, ,PRETTY", "")] // everything removed in multiple parts
    [InlineData("hello PRETTY world", "hello PRETTY world ",
        "hello PRETTY world")] // noting removed because of one space too much
    [InlineData("Total: 500€", "Total,:, ,$,€", "500")] // README sample
    public void BzRemove_WithStringParameters(string? input, string csvSeparators, string output)
    {
        var separators = csvSeparators.Split(',');
        if (separators.Length == 1)
        {
            var singleString = separators[0];
            input.BzRemove(singleString).Should().Be(output);
        }
        else
        {
            input.BzRemove(separators).Should().Be(output);
        }
    }

    [Theory]
    [InlineData("hello", "-", "-hello-")]
    [InlineData("hello", "//", "//hello//")]
    [InlineData("hello", " # ", " # hello # ")]
    [InlineData("hello", "hello", "hellohellohello")]
    [InlineData(" ", " ", "   ")]
    [InlineData("\t", "\n", "\n\t\n")]
    [InlineData(null, "#", "##")]
    [InlineData("", "#", "##")]
    [InlineData(" ", "#", "# #")]
    [InlineData("hello", null, "hello")]
    [InlineData("hello", "", "hello")]
    [InlineData("hello", " ", " hello ")]
    [InlineData(null, null, "")]
    [InlineData("", "", "")]
    public void Wrap_WithSingleArgument(string? input, string? wrap, string output)
    {
        input.Wrap(wrap).Should().Be(output);
        input.Wrap(wrap, wrap).Should().Be(output);
    }

    [Theory]
    [InlineData("hello", "-", "-", "-hello-")]
    [InlineData("hello", "<div>", "</div>", "<div>hello</div>")]
    [InlineData(null, "<div>", "</div>", "<div></div>")]
    [InlineData("hello", null, "</div>", "hello</div>")]
    [InlineData("hello", "<div>", null, "<div>hello")]
    [InlineData("hello", null, null, "hello")]
    [InlineData(null, "<div>", null, "<div>")]
    [InlineData(null, null, "</div>", "</div>")]
    [InlineData(null, null, null, "")]
    public void Wrap_WithTwoArguments(string? middle, string? left, string? right, string output)
    {
        middle.Wrap(left, right).Should().Be(output);
    }
}