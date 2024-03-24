using System.Text;

namespace BlazingDev.BlazingExtensions.Tests;

public class EnumerableExtensionsTests
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
            new Song("boring", false, false, 10),
            new Song("also boring", false, false, 10),
            // then shuffled favorite and new ones
            new Song("greatest song", true, false, 1000),
            new Song("hot stuff", false, true, 500),
            new Song("also like that", true, false, 800),
            new Song("hot and liked stuff", true, true, 400),
            // and again something boring
            new Song("zzz I'm sleeping", false, false, 10),
            new Song("zzz I'm sleeping vol2", false, true, 10),
            new Song("boring but I like it", true, false, 20),
            // and a brand new one
            new Song("hot stuff v2", false, true, 200)
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
}