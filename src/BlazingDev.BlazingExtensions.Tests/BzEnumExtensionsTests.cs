using System.ComponentModel;

using Xunit.Abstractions;

namespace BlazingDev.BlazingExtensions.Tests;

public class BzEnumExtensionsTests(ITestOutputHelper testOutput)
{
    [Theory]
    [InlineData(FileMode.OpenOrCreate, "OpenOrCreate")]
    [InlineData((FileMode)500, "500")]
    [InlineData(SomeEnum.FirstEntry, "FirstEntry")]
    [InlineData(SomeEnum.SecondEntry, "Second entry")]
    [InlineData(FlagsEnum.Bit2, "Bit2")]
    [InlineData(FlagsEnum.Bit4, "fourth bit set")]
    [InlineData(FlagsEnum.Bit1 | FlagsEnum.Bit2, "Bit1, Bit2")]
    [InlineData(FlagsEnum.Bit1 | FlagsEnum.Bit4, "Default setting")]
    [InlineData(FlagsEnum.Bit1 | FlagsEnum.Bit2 | FlagsEnum.Bit4, "All")]
    public void GetDescription(Enum value, string expected)
    {
        value.GetDescription().Should().Be(expected);
    }

    [Theory]
    [InlineData("FirstEntry", SomeEnum.FirstEntry)]
    [InlineData("SecondEntry", SomeEnum.SecondEntry)]
    [InlineData("Second entry", SomeEnum.SecondEntry)]
    public void Parse_WorksForNormalEnums(string input, SomeEnum expected)
    {
        BzEnumExtensions.Parse<SomeEnum>(input).Should().Be(expected);
        BzEnumExtensions.Parse(typeof(SomeEnum), input).Should().Be(expected);
    }

    [Theory]
    [InlineData("Bit1", FlagsEnum.Bit1)]
    [InlineData("Bit2", FlagsEnum.Bit2)]
    [InlineData("Bit4", FlagsEnum.Bit4)]
    [InlineData("fourth bit set", FlagsEnum.Bit4)]
    [InlineData("Default", FlagsEnum.Default)]
    [InlineData("Default setting", FlagsEnum.Default)]
    [InlineData("All", FlagsEnum.All)]
    public void Parse_WorksForFlagsEnums(string input, FlagsEnum expected)
    {
        BzEnumExtensions.Parse<FlagsEnum>(input).Should().Be(expected);
        BzEnumExtensions.Parse(typeof(FlagsEnum), input).Should().Be(expected);
    }

    // todo ignore case
    // todo exception handling

    [Theory]
    [InlineData(FlagsEnum.Bit1 | FlagsEnum.Bit2, FlagsEnum.Bit1, FlagsEnum.Bit2)]
    [InlineData(FlagsEnum.Bit1 | FlagsEnum.Bit2, FlagsEnum.Bit2, FlagsEnum.Bit1)]
    [InlineData(FlagsEnum.Default, FlagsEnum.Bit1, FlagsEnum.Bit4)]
    [InlineData(FlagsEnum.All, FlagsEnum.Bit2, FlagsEnum.Default)]
    [InlineData(FlagsEnum.Bit1, FlagsEnum.Bit1, (FlagsEnum)0)] // removing last flag
    [InlineData(FlagsEnum.Bit1, FlagsEnum.Bit4, FlagsEnum.Bit1)] // removing irrelevant flag
    public void RemoveFlag(FlagsEnum start, FlagsEnum remove, FlagsEnum expect)
    {
        start.RemoveFlag(remove).Should().Be(expect);
        testOutput.WriteLine(new { start, remove, expect }.ToString());

        var ushortStart = (FlagsEnumUshortBased)start;
        var ushortRemove = (FlagsEnumUshortBased)remove;
        var ushortExpect = (FlagsEnumUshortBased)expect;
        ushortStart.RemoveFlag(ushortRemove).Should().Be(ushortExpect);
        testOutput.WriteLine(new { ushortStart, ushortRemove, ushortExpect }.ToString());

        var ulongStart = (FlagsEnumUlongBased)start;
        var ulongRemove = (FlagsEnumUlongBased)remove;
        var ulongExpect = (FlagsEnumUlongBased)expect;
        ulongStart.RemoveFlag(ulongRemove).Should().Be(ulongExpect);
        testOutput.WriteLine(new { ulongStart, ulongRemove, ulongExpect }.ToString());
    }

    public enum SomeEnum
    {
        FirstEntry,
        [Description("Second entry")] SecondEntry,
        ThirdEntry
    }

    [Flags]
    public enum FlagsEnum
    {
        Bit1 = 1,
        Bit2 = 2,
        [Description("fourth bit set")] Bit4 = 4,
        [Description("Default setting")] Default = 5,
        All = 7
    }

    [Flags]
    public enum FlagsEnumUshortBased : ushort
    {
        Bit1 = 1,
        Bit2 = 2,
        [Description("fourth bit set")] Bit4 = 4,
        [Description("Default setting")] Default = 5,
        All = 7
    }

    [Flags]
    public enum FlagsEnumUlongBased : ulong
    {
        Bit1 = 1,
        Bit2 = 2,
        [Description("fourth bit set")] Bit4 = 4,
        [Description("Default setting")] Default = 5,
        All = 7
    }
}