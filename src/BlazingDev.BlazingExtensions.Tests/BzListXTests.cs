namespace BlazingDev.BlazingExtensions.Tests;

public class BzListXTests
{
    [Fact]
    public void Extract_RemovesMatchesFromList()
    {
        List<string> list = ["", " ", "\t", "hello", "world", ""];

        var extracted = list.Extract(x => x.LacksContent());

        list.Should().HaveCount(2);
        list.Should().BeEquivalentTo(["hello", "world"]);
        extracted.Should().HaveCount(4);
        extracted.Should().BeEquivalentTo(["", " ", "\t", ""]);

        extracted = list.Extract(x => x == "hello");
        extracted.Should().BeEquivalentTo("hello");
        list.Should().BeEquivalentTo("world");

        extracted = list.Extract(x => true);
        extracted.Should().BeEquivalentTo("world");
        list.Should().BeEmpty();

        // test that it does not fail with empty lists
        list.Extract(x => true).Should().BeEmpty();
    }
}