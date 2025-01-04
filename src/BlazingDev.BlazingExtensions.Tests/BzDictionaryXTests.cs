namespace BlazingDev.BlazingExtensions.Tests;

public class BzDictionaryXTests
{
    [Fact]
    public void IncrementCount_DecrementCount_GetCount()
    {
        var countByString = new Dictionary<string, int>();
        countByString.IncrementCount("hello");
        countByString.IncrementCount("hello");
        countByString.IncrementCount("world");

        countByString.GetCount("hello").Should().Be(2);
        countByString.GetCount("world").Should().Be(1);
        countByString.GetCount("pretty").Should().Be(0);

        // multiple at once
        countByString.IncrementCount("multipleAtOnce", 3);
        countByString.GetCount("multipleAtOnce").Should().Be(3);

        // decrement
        countByString.IncrementCount("multipleAtOnce", -1);
        countByString.GetCount("multipleAtOnce").Should().Be(2);
        countByString.DecrementCount("multipleAtOnce");
        countByString.GetCount("multipleAtOnce").Should().Be(1);

        // use increment and decrement to get immediate count
        countByString.IncrementCount("multipleAtOnce").Should().Be(2);
        countByString.DecrementCount("multipleAtOnce").Should().Be(1);

        // negative
        countByString.DecrementCount("multipleAtOnce", 10).Should().Be(-9);
    }
}