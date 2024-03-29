using System.ComponentModel;

namespace BlazingDev.BlazingExtensions.Tests;

public class BzEnumExtensionsTests
{
    [Theory]
    [InlineData(FileMode.OpenOrCreate, "OpenOrCreate")]
    [InlineData((FileMode)500, "500")]
    [InlineData(SomeEnum.FirstEntry, "FirstEntry")]
    [InlineData(SomeEnum.SecondEntry, "Second entry")]
    [InlineData(FlagsEnum.Bit2, "Bit2")]
    [InlineData(FlagsEnum.Bit4, "fourth bit set")]
    [InlineData(FlagsEnum.Bit1 | FlagsEnum.Bit2, "Bit1, Bit2")]
    [InlineData(FlagsEnum.Bit1 | FlagsEnum.Bit2 | FlagsEnum.Bit4, "All")]
    [InlineData(FlagsEnum.Bit1 | FlagsEnum.Bit4, "Default setting")]
    public void GetDescription(Enum value, string expected)
    {
        value.GetDescription().Should().Be(expected);
    }

    private enum SomeEnum
    {
        FirstEntry,
        [Description("Second entry")] SecondEntry,
        ThirdEntry
    }

    [Flags]
    private enum FlagsEnum
    {
        Bit1 = 1,
        Bit2 = 2,
        [Description("fourth bit set")] Bit4 = 4,
        All = 7,
        [Description("Default setting")] Default = 5
    }
}