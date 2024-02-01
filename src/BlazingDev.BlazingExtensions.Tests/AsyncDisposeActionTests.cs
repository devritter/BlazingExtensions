namespace BlazingDev.BlazingExtensions.Tests;

public class AsyncDisposeActionTests
{
    [Fact]
    public async Task ActionIsCalledOnDispose()
    {
        var called = false;
        await using (new AsyncDisposeAction(async () => called = true))
        {
        }
        Assert.True(called);
    }

    [Fact]
    public async Task ActionIsOnlyCalledOnce()
    {
        var count = 0;
        var disposable = new AsyncDisposeAction(async () => count++);
        await disposable.DisposeAsync();
        await disposable.DisposeAsync();
        Assert.Equal(1, count);
    }
}