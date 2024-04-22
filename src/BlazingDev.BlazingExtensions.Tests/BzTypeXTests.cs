using System.Text;

namespace BlazingDev.BlazingExtensions.Tests;

public class BzTypeXTests
{
    [Theory]
    [InlineData(typeof(byte))]
    [InlineData(typeof(sbyte))]
    [InlineData(typeof(short))]
    [InlineData(typeof(ushort))]
    [InlineData(typeof(int))]
    [InlineData(typeof(uint))]
    [InlineData(typeof(long))]
    [InlineData(typeof(ulong))]
    [InlineData(typeof(float))]
    [InlineData(typeof(double))]
    [InlineData(typeof(decimal))]
    public void IsNumeric_IsTrue_ForNumericTypes(Type type)
    {
        type.IsNumeric().Should().BeTrue();
    }

    [Theory]
    [InlineData(typeof(string))]
    [InlineData(typeof(DateTime))]
    [InlineData(typeof(TimeSpan))]
    [InlineData(typeof(Enum))]
    [InlineData(typeof(Object))]
    [InlineData(typeof(StringBuilder))]
    public void IsNumeric_IsFalse_ForOtherTypes(Type type)
    {
        type.IsNumeric().Should().BeFalse();
    }

    [Theory]
    // real nullables
    [InlineData(typeof(int?), typeof(int))]
    [InlineData(typeof(bool?), typeof(bool))]
    [InlineData(typeof(DateTime?), typeof(DateTime))]
    // no nullables
    [InlineData(typeof(int), typeof(int))]
    [InlineData(typeof(bool), typeof(bool))]
    [InlineData(typeof(DateTime), typeof(DateTime))]
    // even no structs
    [InlineData(typeof(StringBuilder), typeof(StringBuilder))]
    [InlineData(typeof(List<int>), typeof(List<int>))]
    public void UnwrapNullable(Type input, Type expectedOutput)
    {
        input.UnwrapNullable().Should().Be(expectedOutput);
    }
}
