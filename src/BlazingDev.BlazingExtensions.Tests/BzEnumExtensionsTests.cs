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
    [InlineData("  FirstEntry  ", SomeEnum.FirstEntry)]
    [InlineData("SecondEntry", SomeEnum.SecondEntry)]
    [InlineData("Second entry", SomeEnum.SecondEntry)]
    [InlineData("  Second entry  ", SomeEnum.SecondEntry)]
    public void Parse_WorksForNormalEnums(string input, SomeEnum expected)
    {
        BzEnumExtensions.Parse<SomeEnum>(input).Should().Be(expected);
        BzEnumExtensions.Parse<SomeEnum>(input, true).Should().Be(expected);
        BzEnumExtensions.Parse<SomeEnum>(input.ToUpper(), true).Should().Be(expected);
        BzEnumExtensions.Parse(typeof(SomeEnum), input).Should().Be(expected);
        BzEnumExtensions.Parse(typeof(SomeEnum), input, true).Should().Be(expected);
        BzEnumExtensions.Parse(typeof(SomeEnum), input.ToLower(), true).Should().Be(expected);
    }

    [Theory]
    [InlineData("0", SomeEnum.FirstEntry)]
    [InlineData("1", SomeEnum.SecondEntry)]
    public void Parse_WorksForNormalEnumIntegerValues(string numberText, SomeEnum expect)
    {
        Parse_WorksForNormalEnums(numberText, expect);
    }

    [Theory]
    [InlineData("Bit1", FlagsEnum.Bit1)]
    [InlineData("Bit2", FlagsEnum.Bit2)]
    [InlineData("Bit4", FlagsEnum.Bit4)]
    [InlineData("  Bit4  ", FlagsEnum.Bit4)]
    [InlineData("fourth bit set", FlagsEnum.Bit4)]
    [InlineData("Default", FlagsEnum.Default)]
    [InlineData("Default setting", FlagsEnum.Default)]
    [InlineData("  Default setting  ", FlagsEnum.Default)]
    [InlineData("All", FlagsEnum.All)]
    [InlineData("  All  ", FlagsEnum.All)]
    public void Parse_WorksForFlagsEnums(string input, FlagsEnum expected)
    {
        BzEnumExtensions.Parse<FlagsEnum>(input).Should().Be(expected);
        BzEnumExtensions.Parse<FlagsEnum>(input, true).Should().Be(expected);
        BzEnumExtensions.Parse<FlagsEnum>(input.ToLower(), true).Should().Be(expected);
        BzEnumExtensions.Parse(typeof(FlagsEnum), input).Should().Be(expected);
        BzEnumExtensions.Parse(typeof(FlagsEnum), input, true).Should().Be(expected);
        BzEnumExtensions.Parse(typeof(FlagsEnum), input.ToUpper(), true).Should().Be(expected);
    }

    [Theory]
    [InlineData("2", FlagsEnum.Bit2)]
    [InlineData("  2  ", FlagsEnum.Bit2)]
    [InlineData("5", FlagsEnum.Default)]
    [InlineData("  5  ", FlagsEnum.Default)]
    public void Parse_WorksForFlagsEnumIntegerValues(string numberText, FlagsEnum expect)
    {
        Parse_WorksForFlagsEnums(numberText, expect);
    }

    [Fact]
    public void Parse_WorksForAnyIntegerValue()
    {
        BzEnumExtensions.Parse<SomeEnum>("64"); // no exception
        BzEnumExtensions.Parse(typeof(SomeEnum), "64"); // no exception
        BzEnumExtensions.Parse<FlagsEnum>("64"); // no exception
        BzEnumExtensions.Parse(typeof(FlagsEnum), "64"); // no exception
    }

    [Fact]
    public void Parse_ThrowsExceptionForUnknownStrings()
    {
        Test(() => BzEnumExtensions.Parse<SomeEnum>("Unkown"));
        Test(() => BzEnumExtensions.Parse(typeof(SomeEnum), "Unkown"));
        Test(() => BzEnumExtensions.Parse<FlagsEnum>("Unkown"));
        Test(() => BzEnumExtensions.Parse(typeof(FlagsEnum), "Unkown"));

        Test(() => BzEnumExtensions.Parse<SomeEnum>("Second   Entry"));

        void Test(Action action)
        {
            action.Should().Throw<ArgumentException>();
        }
    }

    [Theory]
    [InlineData(FlagsEnum.Bit1 | FlagsEnum.Bit2, FlagsEnum.Bit1, FlagsEnum.Bit2)]
    [InlineData(FlagsEnum.Bit1 | FlagsEnum.Bit2, FlagsEnum.Bit2, FlagsEnum.Bit1)]
    [InlineData(FlagsEnum.Default, FlagsEnum.Bit1, FlagsEnum.Bit4)]
    [InlineData(FlagsEnum.All, FlagsEnum.Bit2, FlagsEnum.Default)]
    public void RemoveFlag_AddFlag_HandlesObviousSituations(FlagsEnum start, FlagsEnum remove, FlagsEnum expect)
    {
        start.RemoveFlag(remove).Should().Be(expect);
        start.SetFlag(remove, false).Should().Be(expect);
        testOutput.WriteLine(new { start, remove, expect }.ToString());
        expect.AddFlag(remove).Should().Be(start);
        expect.SetFlag(remove, true).Should().Be(start);

        var ushortStart = (FlagsEnumUshortBased)start;
        var ushortRemove = (FlagsEnumUshortBased)remove;
        var ushortExpect = (FlagsEnumUshortBased)expect;
        ushortStart.RemoveFlag(ushortRemove).Should().Be(ushortExpect);
        ushortStart.SetFlag(ushortRemove, false).Should().Be(ushortExpect);
        testOutput.WriteLine(new { ushortStart, ushortRemove, ushortExpect }.ToString());
        ushortExpect.AddFlag(ushortRemove).Should().Be(ushortStart);
        ushortExpect.SetFlag(ushortRemove, true).Should().Be(ushortStart);

        var ulongStart = (FlagsEnumUlongBased)start;
        var ulongRemove = (FlagsEnumUlongBased)remove;
        var ulongExpect = (FlagsEnumUlongBased)expect;
        ulongStart.RemoveFlag(ulongRemove).Should().Be(ulongExpect);
        ulongStart.SetFlag(ulongRemove, false).Should().Be(ulongExpect);
        testOutput.WriteLine(new { ulongStart, ulongRemove, ulongExpect }.ToString());
        ulongExpect.AddFlag(ulongRemove).Should().Be(ulongStart);
        ulongExpect.SetFlag(ulongRemove, true).Should().Be(ulongStart);
    }

    [Theory]
    [InlineData(FlagsEnum.Bit1, FlagsEnum.Bit1, (FlagsEnum)0)] // removing last flag
    [InlineData(FlagsEnum.Bit1, FlagsEnum.Bit4, FlagsEnum.Bit1)] // removing irrelevant flag
    public void RemoveFlag_HandlesSpecialSituations(FlagsEnum start, FlagsEnum remove, FlagsEnum expect)
    {
        start.RemoveFlag(remove).Should().Be(expect);
        start.SetFlag(remove, false).Should().Be(expect);

        var ushortStart = (FlagsEnumUshortBased)start;
        var ushortRemove = (FlagsEnumUshortBased)remove;
        var ushortExpect = (FlagsEnumUshortBased)expect;
        ushortStart.RemoveFlag(ushortRemove).Should().Be(ushortExpect);
        ushortStart.SetFlag(ushortRemove, false).Should().Be(ushortExpect);

        var ulongStart = (FlagsEnumUlongBased)start;
        var ulongRemove = (FlagsEnumUlongBased)remove;
        var ulongExpect = (FlagsEnumUlongBased)expect;
        ulongStart.RemoveFlag(ulongRemove).Should().Be(ulongExpect);
        ulongStart.SetFlag(ulongRemove, false).Should().Be(ulongExpect);
    }

    [Theory]
    [InlineData(FlagsEnum.Bit1, FlagsEnum.Bit1, FlagsEnum.Bit1)] // flag already set - simple
    [InlineData(FlagsEnum.Default, FlagsEnum.Bit4, FlagsEnum.Default)] // flag already set - combo-flag
    public void AddFlag_HandlesSpecialSituations(FlagsEnum start, FlagsEnum add, FlagsEnum expect)
    {
        start.AddFlag(add).Should().Be(expect);
        start.SetFlag(add, true).Should().Be(expect);

        var ushortStart = (FlagsEnumUshortBased)start;
        var ushortAdd = (FlagsEnumUshortBased)add;
        var ushortExpect = (FlagsEnumUshortBased)expect;
        ushortStart.AddFlag(ushortAdd).Should().Be(ushortExpect);
        ushortStart.SetFlag(ushortAdd, true).Should().Be(ushortExpect);

        var ulongStart = (FlagsEnumUlongBased)start;
        var ulongAdd = (FlagsEnumUlongBased)add;
        var ulongExpect = (FlagsEnumUlongBased)expect;
        ulongStart.AddFlag(ulongAdd).Should().Be(ulongExpect);
        ulongStart.SetFlag(ulongAdd, true).Should().Be(ulongExpect);
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