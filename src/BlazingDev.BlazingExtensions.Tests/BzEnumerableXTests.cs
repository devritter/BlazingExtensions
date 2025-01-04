using System.Text;

namespace BlazingDev.BlazingExtensions.Tests;

public class BzEnumerableXTests
{
    [Fact]
    public void HasContent()
    {
        Test(false, null);
        Test(false, []);
        Test(true, [1]);

        void Test(bool expected, int[]? values)
        {
            Assert.Equal(expected, values.HasContent());

            if (values.HasContent())
            {
                // no compiler warning here!
                _ = values.Length;
            }
        }
    }

    [Fact]
    public void LacksContent()
    {
        Test(true, null);
        Test(true, []);
        Test(false, [1]);

        void Test(bool expected, int[]? values)
        {
            Assert.Equal(expected, values.LacksContent());

            if (!values.LacksContent())
            {
                // no compiler warning here!
                _ = values.Length;
            }
        }
    }

    [Fact]
    public void BzAll()
    {
        Test(null, false);
        Test([], false);
        Test([""], false);
        Test(["hello"], true);
        Test(["hello", "world"], true);
        Test(["hello", ""], false);

        void Test(IEnumerable<string?>? items, bool expected)
        {
            items.BzAll(x => x.HasContent()).Should().Be(expected);
        }
    }

    [Fact]
    public void BzAll_ThrowsArgumentException_WhenPredicateIsNull()
    {
        int[] items = [1, 2, 3];
        Assert.Throws<ArgumentNullException>(() => items.BzAll(null!));
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

    private record Song(string Name, bool Favorite, bool BrandNew, int Likes);

    [Fact]
    public void OrderByBool_ByPracticalExample()
    {
        // create a "random" list of songs and order them by:
        // - favorite first,
        // - then brand new if enough likes,
        // - then by alphabet asc

        List<Song> songs =
        [
            // first some uninteresting ones
            new("boring", false, false, 10),
            new("also boring", false, false, 10),
            // then shuffled favorite and new ones
            new("greatest song", true, false, 1000),
            new("hot stuff", false, true, 500),
            new("also like that", true, false, 800),
            new("hot and liked stuff", true, true, 400),
            // and again something boring
            new("zzz I'm sleeping", false, false, 10),
            new("zzz I'm sleeping vol2", false, true, 10),
            new("boring but I like it", true, false, 20),
            // and a brand new one
            new("hot stuff v2", false, true, 200)
        ];

        var sorted = songs
            .OrderByFirstWhere(x => x.Favorite)
            // in our use case we don't want the favorites to be alphabetically sorted, so the "brand new" flag must be ignored
            .ThenByFirstWhere(x => !x.Favorite && x.BrandNew && x.Likes > 100)
            .ThenBy(x => x.Name)
            .ToList();

        // @formatter:keep_existing_initializer_arrangement true
        string[] expected =
        [
            // favorites
            "also like that",
            "boring but I like it",
            "greatest song",
            "hot and liked stuff",
            // brand new
            "hot stuff",
            "hot stuff v2",
            // rest alphabetically
            "also boring",
            "boring",
            "zzz I'm sleeping",
            "zzz I'm sleeping vol2"
        ];
        // @formatter:keep_existing_initializer_arrangement restore

        sorted.Select(x => x.Name).Should().BeEquivalentTo(expected, x => x.WithStrictOrdering());
    }

    [Fact]
    public void OrderByFirstWhere()
    {
        string[] unsorted =
        [
            "I want",
            "Gimme",
            "Please more",
            "Could I have more, please?"
        ];

        var friendlyFirst = unsorted
            .OrderByFirstWhere(x => x.ContainsIgnoreCase("Please"))
            .ToList();

        string[] expected =
        [
            "Please more",
            "Could I have more, please?",
            "I want",
            "Gimme"
        ];

        friendlyFirst.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
    }

    [Fact]
    public void OrderByFirstWhere_CanBeExtendedByExistingLinqSorting()
    {
        string[] unsorted =
        [
            "I want",
            "Gimme",
            "Please more",
            "Could I have more, please?"
        ];

        var friendlyFirst = unsorted
            .OrderByFirstWhere(x => x.ContainsIgnoreCase("Please"))
            .ThenBy(x => x)
            .ToList();

        string[] expected =
        [
            "Could I have more, please?",
            "Please more",
            "Gimme",
            "I want"
        ];

        friendlyFirst.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
    }

    [Fact]
    public void ThenByFirstWhere()
    {
        List<(string desc, bool urgent, bool loved)> tasks =
        [
            ("call back the boss", true, false),
            ("pay the bills", false, false),
            ("holiday", false, true),
            ("fuel", true, false),
            ("sleeping", false, true),
            ("groceries", false, false)
        ];

        var sorted = tasks
            .OrderByFirstWhere(x => x.urgent)
            .ThenByFirstWhere(x => x.loved)
            .ThenBy(x => x.desc)
            .Select(x => x.desc)
            .ToList();

        var expected = new[]
        {
            // urgent
            "call back the boss", "fuel",
            // loved
            "holiday", "sleeping",
            // rest
            "groceries", "pay the bills"
        };

        sorted.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
    }

    [Fact]
    public void WhereNotNull_ForClasses()
    {
        var items = new List<StringBuilder?>() { new StringBuilder().Append("hello"), null, new("world") };

        var stringLength = items.WhereNotNull()
            .Select(x => x.Length).Sum();
        // no compiler warning here ^
        stringLength.Should().Be(10);
    }

    [Fact]
    public void WhereNotNull_ForStructs()
    {
        List<TimeSpan?> items = [TimeSpan.FromSeconds(5), null, TimeSpan.FromSeconds(10), null];
        var totalSeconds = items.WhereNotNull().Select(x => x.TotalSeconds).Sum();
        totalSeconds.Should().Be(15);
    }

    [Fact]
    public void EnumerableForEach()
    {
        var sum = 0;
        var items = new[] { 1, 2, 3 };
        items.ForEach(x => sum += x);
        sum.Should().Be(6);

        sum = 0;
        items.AsEnumerable().ForEach(x => sum += x);
        sum.Should().Be(6);

        sum = 0;
        var hashSet = new HashSet<int>(items);
        hashSet.ForEach(x => sum += x);
        sum.Should().Be(6);

        sum = 0;
        // we can't really assert that the "original" List.ForEach(x) method is called
        // but we just verify that we don't have compiler errors
        var list = new List<int>(items);
        list.ForEach(x => sum += x);
        sum.Should().Be(6);
    }

    [Fact]
    public void IsSingle_IsMultiple()
    {
        Test([], false, false);
        Test([1], true, false);
        Test([1, 2], false, true);

        void Test(int[] values, bool expectedSingle, bool expectedMultiple)
        {
            values.IsSingle().Should().Be(expectedSingle);
            values.IsMultiple().Should().Be(expectedMultiple);
        }
    }
}