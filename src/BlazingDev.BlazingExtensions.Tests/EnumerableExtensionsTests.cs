namespace BlazingDev.BlazingExtensions.Tests;

public class EnumerableExtensionsTests
{
    [Fact]
    public void SafeAny()
    {
        Test(false, null);
        Test(false, []);
        Test(true, [1]);

        void Test(bool expected, int[]? values)
        {
            Assert.Equal(expected, values.SafeAny());
        }
    }
    
    [Fact]
    public void IsEmpty()
    {
        Test(true, null);
        Test(true, []);
        Test(false, [1]);

        void Test(bool expected, int[]? values)
        {
            Assert.Equal(expected, values.IsEmpty());
        }
    }

    [Fact]
    public void StringJoin()
    {
        Test([], ",", "");
        Test([""], ",", "");
        Test(["item"], ",", "item");
        Test(["item", ""], ",", "item,");
        Test(["item", "another"], ",", "item,another");
        Test(["item", "another"], "", "itemanother");
        // null handling
        Test(null, "", "");
        Test([null], ",", "");
        Test(["a", null, "b"], ",", "a,,b");

        void Test(IEnumerable<string?>? items, string separator, string expected)
        {
            Assert.Equal(expected, items.StringJoin(separator));
        }
    }
}